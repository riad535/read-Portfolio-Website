using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Projects.Queries.GetProjectById;
using PortfolioWebsite.Application.Features.Projects.Queries.GetProjects;
using PortfolioWebsite.Application.Features.Projects.Queries.GetPublicProjects;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Projects;

public sealed class ProjectQueryHandlerTests
{
    [Fact]
    public async Task GetPublicProjectsHandler_ExcludesUnpublishedAndOrdersByDisplayOrder()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.Projects.AddRange(
            CreateProject("Published second", displayOrder: 2, isPublished: true),
            CreateProject("Unpublished", displayOrder: 0, isPublished: false),
            CreateProject("Published first", displayOrder: 1, isPublished: true));
        await database.Context.SaveChangesAsync();

        var handler = new GetPublicProjectsHandler(database.Context);
        var result = await handler.Handle(new GetPublicProjectsQuery(), CancellationToken.None);

        Assert.Equal(
            new[] { "Published first", "Published second" },
            result.Select(project => project.Title));
    }

    [Fact]
    public async Task GetProjectsHandler_IncludesUnpublishedItems()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.Projects.AddRange(
            CreateProject("Published", displayOrder: 2, isPublished: true),
            CreateProject("Draft", displayOrder: 1, isPublished: false));
        await database.Context.SaveChangesAsync();

        var handler = new GetProjectsHandler(database.Context);
        var result = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, project => project.Title == "Draft" && !project.IsPublished);
        Assert.Equal(new[] { "Draft", "Published" }, result.Select(project => project.Title));
    }

    [Fact]
    public async Task GetProjectByIdHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new GetProjectByIdHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetProjectByIdQuery(999), CancellationToken.None));
    }

    private static Project CreateProject(string title, int displayOrder, bool isPublished)
    {
        return new Project
        {
            Title = title,
            Description = $"{title} description",
            TechStack = "C#, .NET",
            DisplayOrder = displayOrder,
            IsPublished = isPublished
        };
    }
}
