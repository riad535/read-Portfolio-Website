using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Skills.Queries.GetPublicSkills;
using PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategoryById;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Skills;

public sealed class SkillQueryHandlerTests
{
    [Fact]
    public async Task GetPublicSkillsHandler_ReturnsCategoriesAndSkillsInDisplayOrder()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.SkillCategories.AddRange(
            new SkillCategory
            {
                Name = "Second category",
                IconClass = "bi-database",
                DisplayOrder = 2,
                Skills =
                {
                    new Skill { Name = "Second skill", DisplayOrder = 2 },
                    new Skill { Name = "First skill", DisplayOrder = 1 }
                }
            },
            new SkillCategory
            {
                Name = "First category",
                IconClass = "bi-code-slash",
                DisplayOrder = 1,
                Skills = { new Skill { Name = "Only skill", DisplayOrder = 1 } }
            });
        await database.Context.SaveChangesAsync();

        var handler = new GetPublicSkillsHandler(database.Context);
        var result = await handler.Handle(new GetPublicSkillsQuery(), CancellationToken.None);

        Assert.Equal(
            new[] { "First category", "Second category" },
            result.Select(category => category.Name));
        Assert.Equal(new[] { "Only skill" }, result[0].Skills);
        Assert.Equal(new[] { "First skill", "Second skill" }, result[1].Skills);
    }

    [Fact]
    public async Task GetSkillCategoryByIdHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new GetSkillCategoryByIdHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetSkillCategoryByIdQuery(999), CancellationToken.None));
    }
}
