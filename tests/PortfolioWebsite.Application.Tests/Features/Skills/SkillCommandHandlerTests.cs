using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Skills.Commands.CreateSkillCategory;
using PortfolioWebsite.Application.Features.Skills.Commands.DeleteSkillCategory;
using PortfolioWebsite.Application.Features.Skills.Commands.UpdateSkillCategory;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Skills;

public sealed class SkillCommandHandlerTests
{
    [Fact]
    public async Task CreateSkillCategoryHandler_ValidCommand_PersistsTrimmedValuesAndReturnsId()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new CreateSkillCategoryHandler(database.Context);

        var id = await handler.Handle(
            new CreateSkillCategoryCommand(
                "  Backend  ",
                " bi-code-slash ",
                2,
                new[] { " C# ", string.Empty, " .NET Core " }),
            CancellationToken.None);

        Assert.True(id > 0);

        database.Context.ChangeTracker.Clear();
        var category = await database.Context.SkillCategories
            .Include(existing => existing.Skills)
            .SingleAsync();

        Assert.Equal(id, category.Id);
        Assert.Equal("Backend", category.Name);
        Assert.Equal("bi-code-slash", category.IconClass);
        Assert.Equal(2, category.DisplayOrder);
        Assert.Equal(
            new[] { "C#", ".NET Core" },
            category.Skills.OrderBy(skill => skill.DisplayOrder).Select(skill => skill.Name));
        Assert.Equal(
            new[] { 1, 2 },
            category.Skills.OrderBy(skill => skill.DisplayOrder).Select(skill => skill.DisplayOrder));
        Assert.NotEqual(default, category.CreatedAtUtc);
        Assert.NotEqual(default, category.UpdatedAtUtc);
    }

    [Fact]
    public async Task CreateSkillCategoryHandler_DuplicateNameIgnoringCase_ThrowsValidationException()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.SkillCategories.Add(new SkillCategory
        {
            Name = "Backend",
            IconClass = "bi-code-slash",
            DisplayOrder = 1
        });
        await database.Context.SaveChangesAsync();

        var handler = new CreateSkillCategoryHandler(database.Context);

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(
                new CreateSkillCategoryCommand(
                    "backend",
                    "bi-database",
                    2,
                    new[] { "SQL Server" }),
                CancellationToken.None));

        Assert.True(exception.Errors.ContainsKey(nameof(CreateSkillCategoryCommand.Name)));
    }

    [Fact]
    public async Task CreateSkillCategoryHandler_DuplicateSkillsIgnoringCase_ThrowsValidationException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new CreateSkillCategoryHandler(database.Context);

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(
                new CreateSkillCategoryCommand(
                    "Backend",
                    "bi-code-slash",
                    1,
                    new[] { "C#", "c#" }),
                CancellationToken.None));

        Assert.True(exception.Errors.ContainsKey(nameof(CreateSkillCategoryCommand.Skills)));
    }

    [Fact]
    public async Task UpdateSkillCategoryHandler_ValidCommand_ReplacesSkillsWithSequentialOrder()
    {
        await using var database = await TestDatabase.CreateAsync();
        var category = new SkillCategory
        {
            Name = "Old name",
            IconClass = "bi-code-slash",
            DisplayOrder = 1,
            Skills =
            {
                new Skill { Name = "Old one", DisplayOrder = 1 },
                new Skill { Name = "Old two", DisplayOrder = 2 }
            }
        };
        database.Context.SkillCategories.Add(category);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new UpdateSkillCategoryHandler(database.Context);

        await handler.Handle(
            new UpdateSkillCategoryCommand(
                category.Id,
                "  Updated  ",
                "bi-database",
                5,
                new[] { " New one ", " New two " }),
            CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        var updated = await database.Context.SkillCategories
            .Include(existing => existing.Skills)
            .SingleAsync();

        Assert.Equal("Updated", updated.Name);
        Assert.Equal("bi-database", updated.IconClass);
        Assert.Equal(5, updated.DisplayOrder);

        var skills = await database.Context.Skills
            .AsNoTracking()
            .OrderBy(skill => skill.DisplayOrder)
            .ToListAsync();

        Assert.Equal(2, skills.Count);
        Assert.Equal(new[] { "New one", "New two" }, skills.Select(skill => skill.Name));
        Assert.Equal(new[] { 1, 2 }, skills.Select(skill => skill.DisplayOrder));
        Assert.DoesNotContain(skills, skill => skill.Name.StartsWith("Old"));
    }

    [Fact]
    public async Task UpdateSkillCategoryHandler_KeepingOwnName_DoesNotThrow()
    {
        await using var database = await TestDatabase.CreateAsync();
        var category = new SkillCategory
        {
            Name = "Backend",
            IconClass = "bi-code-slash",
            DisplayOrder = 1,
            Skills = { new Skill { Name = "C#", DisplayOrder = 1 } }
        };
        database.Context.SkillCategories.Add(category);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new UpdateSkillCategoryHandler(database.Context);

        await handler.Handle(
            new UpdateSkillCategoryCommand(
                category.Id,
                "backend",
                "bi-code-slash",
                1,
                new[] { "C#" }),
            CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        var updated = await database.Context.SkillCategories.SingleAsync();
        Assert.Equal("backend", updated.Name);
    }

    [Fact]
    public async Task UpdateSkillCategoryHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new UpdateSkillCategoryHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new UpdateSkillCategoryCommand(
                    999,
                    "Backend",
                    "bi-code-slash",
                    1,
                    new[] { "C#" }),
                CancellationToken.None));
    }

    [Fact]
    public async Task DeleteSkillCategoryHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new DeleteSkillCategoryHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new DeleteSkillCategoryCommand(999),
                CancellationToken.None));
    }

    [Fact]
    public async Task DeleteSkillCategoryHandler_ExistingCategory_DeletesChildSkills()
    {
        await using var database = await TestDatabase.CreateAsync();
        var category = new SkillCategory
        {
            Name = "Backend",
            IconClass = "bi-code-slash",
            DisplayOrder = 1,
            Skills =
            {
                new Skill { Name = "C#", DisplayOrder = 1 },
                new Skill { Name = ".NET", DisplayOrder = 2 }
            }
        };
        database.Context.SkillCategories.Add(category);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new DeleteSkillCategoryHandler(database.Context);
        await handler.Handle(new DeleteSkillCategoryCommand(category.Id), CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        Assert.Equal(0, await database.Context.SkillCategories.CountAsync());
        Assert.Equal(0, await database.Context.Skills.CountAsync());
    }
}
