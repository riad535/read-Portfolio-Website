using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Experiences.Commands.CreateExperience;
using PortfolioWebsite.Application.Features.Experiences.Commands.DeleteExperience;
using PortfolioWebsite.Application.Features.Experiences.Commands.SetExperiencePublished;
using PortfolioWebsite.Application.Features.Experiences.Commands.UpdateExperience;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Experiences;

public sealed class ExperienceCommandHandlerTests
{
    private static readonly DateOnly Start = new(2024, 1, 1);
    private static readonly DateOnly End = new(2024, 6, 1);

    [Fact]
    public async Task CreateExperienceHandler_CurrentRole_PersistsTrimmedValuesAndOrderedPoints()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new CreateExperienceHandler(database.Context);

        var id = await handler.Handle(
            new CreateExperienceCommand(
                "  Developer  ",
                "  Company  ",
                Start,
                End,
                IsCurrent: true,
                IsPublished: true,
                new[] { " First point ", string.Empty, " Second point " }),
            CancellationToken.None);

        Assert.True(id > 0);

        database.Context.ChangeTracker.Clear();
        var experience = await database.Context.Experiences
            .Include(existing => existing.Points)
            .SingleAsync();

        Assert.Equal(id, experience.Id);
        Assert.Equal("Developer", experience.Role);
        Assert.Equal("Company", experience.Organisation);
        Assert.Equal(Start, experience.StartDate);
        Assert.Null(experience.EndDate);
        Assert.True(experience.IsCurrent);
        Assert.True(experience.IsPublished);
        Assert.Equal(
            new[] { "First point", "Second point" },
            experience.Points.OrderBy(point => point.DisplayOrder).Select(point => point.Text));
        Assert.Equal(
            new[] { 1, 2 },
            experience.Points.OrderBy(point => point.DisplayOrder).Select(point => point.DisplayOrder));
    }

    [Fact]
    public async Task UpdateExperienceHandler_ValidCommand_ReplacesPointsWithSequentialOrder()
    {
        await using var database = await TestDatabase.CreateAsync();
        var experience = new Experience
        {
            Role = "Old role",
            Organisation = "Old organisation",
            StartDate = Start,
            EndDate = End,
            IsCurrent = false,
            IsPublished = true,
            Points =
            {
                new ExperiencePoint { Text = "Old one", DisplayOrder = 1 },
                new ExperiencePoint { Text = "Old two", DisplayOrder = 2 }
            }
        };
        database.Context.Experiences.Add(experience);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new UpdateExperienceHandler(database.Context);

        await handler.Handle(
            new UpdateExperienceCommand(
                experience.Id,
                "  New role  ",
                "  New organisation  ",
                Start,
                null,
                IsCurrent: true,
                IsPublished: false,
                new[] { " New one ", " New two " }),
            CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        var updated = await database.Context.Experiences
            .Include(existing => existing.Points)
            .SingleAsync();

        Assert.Equal("New role", updated.Role);
        Assert.Equal("New organisation", updated.Organisation);
        Assert.Null(updated.EndDate);
        Assert.True(updated.IsCurrent);
        Assert.False(updated.IsPublished);

        var points = await database.Context.ExperiencePoints
            .AsNoTracking()
            .OrderBy(point => point.DisplayOrder)
            .ToListAsync();

        Assert.Equal(2, points.Count);
        Assert.Equal(new[] { "New one", "New two" }, points.Select(point => point.Text));
        Assert.Equal(new[] { 1, 2 }, points.Select(point => point.DisplayOrder));
        Assert.DoesNotContain(points, point => point.Text.StartsWith("Old"));
    }

    [Fact]
    public async Task UpdateExperienceHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new UpdateExperienceHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new UpdateExperienceCommand(
                    999,
                    "Role",
                    "Organisation",
                    Start,
                    null,
                    IsCurrent: true,
                    IsPublished: true,
                    Array.Empty<string>()),
                CancellationToken.None));
    }

    [Fact]
    public async Task DeleteExperienceHandler_ExistingExperience_DeletesChildPoints()
    {
        await using var database = await TestDatabase.CreateAsync();
        var experience = new Experience
        {
            Role = "Role",
            Organisation = "Organisation",
            StartDate = Start,
            EndDate = End,
            IsCurrent = false,
            IsPublished = true,
            Points =
            {
                new ExperiencePoint { Text = "Point one", DisplayOrder = 1 },
                new ExperiencePoint { Text = "Point two", DisplayOrder = 2 }
            }
        };
        database.Context.Experiences.Add(experience);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new DeleteExperienceHandler(database.Context);
        await handler.Handle(new DeleteExperienceCommand(experience.Id), CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        Assert.Equal(0, await database.Context.Experiences.CountAsync());
        Assert.Equal(0, await database.Context.ExperiencePoints.CountAsync());
    }

    [Fact]
    public async Task DeleteExperienceHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new DeleteExperienceHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new DeleteExperienceCommand(999), CancellationToken.None));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SetExperiencePublishedHandler_ChangesFlag(bool isPublished)
    {
        await using var database = await TestDatabase.CreateAsync();
        var experience = new Experience
        {
            Role = "Role",
            Organisation = "Organisation",
            StartDate = Start,
            EndDate = End,
            IsCurrent = false,
            IsPublished = !isPublished
        };
        database.Context.Experiences.Add(experience);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new SetExperiencePublishedHandler(database.Context);
        await handler.Handle(
            new SetExperiencePublishedCommand(experience.Id, isPublished),
            CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        var updated = await database.Context.Experiences.SingleAsync();
        Assert.Equal(isPublished, updated.IsPublished);
    }
}
