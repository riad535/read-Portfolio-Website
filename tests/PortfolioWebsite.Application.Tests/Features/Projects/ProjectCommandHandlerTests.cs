using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Projects.Commands.CreateProject;
using PortfolioWebsite.Application.Features.Projects.Commands.DeleteProject;
using PortfolioWebsite.Application.Features.Projects.Commands.SetProjectPublished;
using PortfolioWebsite.Application.Features.Projects.Commands.UpdateProject;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Projects;

public sealed class ProjectCommandHandlerTests
{
    [Fact]
    public async Task CreateProjectHandler_ValidCommand_PersistsTrimmedValuesAndReturnsId()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new CreateProjectHandler(database.Context);

        var id = await handler.Handle(
            new CreateProjectCommand(
                "  Project title  ",
                "  Project description  ",
                "  C#, .NET  ",
                "   ",
                " ",
                null,
                3,
                IsPublished: false),
            CancellationToken.None);

        Assert.True(id > 0);

        database.Context.ChangeTracker.Clear();
        var project = await database.Context.Projects.SingleAsync();

        Assert.Equal(id, project.Id);
        Assert.Equal("Project title", project.Title);
        Assert.Equal("Project description", project.Description);
        Assert.Equal("C#, .NET", project.TechStack);
        Assert.Null(project.Architecture);
        Assert.Null(project.LiveDemoUrl);
        Assert.Null(project.GitHubUrl);
        Assert.Equal(3, project.DisplayOrder);
        Assert.False(project.IsPublished);
        Assert.NotEqual(default, project.CreatedAtUtc);
        Assert.NotEqual(default, project.UpdatedAtUtc);
    }

    [Fact]
    public async Task UpdateProjectHandler_ValidCommand_UpdatesValues()
    {
        await using var database = await TestDatabase.CreateAsync();
        var project = new Project
        {
            Title = "Old title",
            Description = "Old description",
            TechStack = "Old stack",
            Architecture = "Old architecture",
            LiveDemoUrl = "https://old.example.com",
            GitHubUrl = "https://github.com/old/repository",
            DisplayOrder = 1,
            IsPublished = true
        };
        database.Context.Projects.Add(project);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new UpdateProjectHandler(database.Context);

        await handler.Handle(
            new UpdateProjectCommand(
                project.Id,
                "  New title  ",
                "  New description  ",
                "  New stack  ",
                "  ",
                null,
                " https://github.com/new/repository ",
                7,
                IsPublished: false),
            CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        var updated = await database.Context.Projects.SingleAsync();

        Assert.Equal("New title", updated.Title);
        Assert.Equal("New description", updated.Description);
        Assert.Equal("New stack", updated.TechStack);
        Assert.Null(updated.Architecture);
        Assert.Null(updated.LiveDemoUrl);
        Assert.Equal("https://github.com/new/repository", updated.GitHubUrl);
        Assert.Equal(7, updated.DisplayOrder);
        Assert.False(updated.IsPublished);
    }

    [Fact]
    public async Task UpdateProjectHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new UpdateProjectHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new UpdateProjectCommand(
                    999,
                    "Title",
                    "Description",
                    "Stack",
                    null,
                    null,
                    null,
                    1,
                    IsPublished: true),
                CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProjectHandler_ExistingProject_RemovesIt()
    {
        await using var database = await TestDatabase.CreateAsync();
        var project = new Project
        {
            Title = "Title",
            Description = "Description",
            TechStack = "Stack",
            DisplayOrder = 1,
            IsPublished = true
        };
        database.Context.Projects.Add(project);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new DeleteProjectHandler(database.Context);
        await handler.Handle(new DeleteProjectCommand(project.Id), CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        Assert.Equal(0, await database.Context.Projects.CountAsync());
    }

    [Fact]
    public async Task DeleteProjectHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new DeleteProjectHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new DeleteProjectCommand(999), CancellationToken.None));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SetProjectPublishedHandler_ChangesFlag(bool isPublished)
    {
        await using var database = await TestDatabase.CreateAsync();
        var project = new Project
        {
            Title = "Title",
            Description = "Description",
            TechStack = "Stack",
            DisplayOrder = 1,
            IsPublished = !isPublished
        };
        database.Context.Projects.Add(project);
        await database.Context.SaveChangesAsync();
        database.Context.ChangeTracker.Clear();

        var handler = new SetProjectPublishedHandler(database.Context);
        await handler.Handle(
            new SetProjectPublishedCommand(project.Id, isPublished),
            CancellationToken.None);

        database.Context.ChangeTracker.Clear();
        var updated = await database.Context.Projects.SingleAsync();
        Assert.Equal(isPublished, updated.IsPublished);
    }
}
