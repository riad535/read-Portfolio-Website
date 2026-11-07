using PortfolioWebsite.Application.Features.Dashboard.Queries.GetDashboardSummary;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Dashboard;

public sealed class DashboardQueryHandlerTests
{
    [Fact]
    public async Task GetDashboardSummaryHandler_ReturnsCorrectCounts()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.SkillCategories.AddRange(
            new SkillCategory { Name = "Category one", IconClass = "bi-code-slash", DisplayOrder = 1 },
            new SkillCategory { Name = "Category two", IconClass = "bi-database", DisplayOrder = 2 });
        database.Context.Projects.AddRange(
            CreateProject("Project one", isPublished: true),
            CreateProject("Project two", isPublished: false),
            CreateProject("Project three", isPublished: true));
        database.Context.Experiences.AddRange(
            CreateExperience("Role one", isPublished: true),
            CreateExperience("Role two", isPublished: false),
            CreateExperience("Role three", isPublished: true),
            CreateExperience("Role four", isPublished: false));
        await database.Context.SaveChangesAsync();

        var handler = new GetDashboardSummaryHandler(database.Context);
        var result = await handler.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        Assert.Equal(2, result.SkillCategoryCount);
        Assert.Equal(3, result.ProjectCount);
        Assert.Equal(4, result.ExperienceCount);
    }

    private static Project CreateProject(string title, bool isPublished)
    {
        return new Project
        {
            Title = title,
            Description = "Description",
            TechStack = "C#, .NET",
            DisplayOrder = 1,
            IsPublished = isPublished
        };
    }

    private static Experience CreateExperience(string role, bool isPublished)
    {
        return new Experience
        {
            Role = role,
            Organisation = "Organisation",
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 2, 1),
            IsCurrent = false,
            IsPublished = isPublished
        };
    }
}
