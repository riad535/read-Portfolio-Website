using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Features.Experiences.Queries.GetExperienceById;
using PortfolioWebsite.Application.Features.Experiences.Queries.GetExperiences;
using PortfolioWebsite.Application.Features.Experiences.Queries.GetPublicExperiences;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Tests.Features.Experiences;

public sealed class ExperienceQueryHandlerTests
{
    [Fact]
    public async Task GetPublicExperiencesHandler_ExcludesUnpublishedOrdersCurrentFirstAndFormatsDates()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.Experiences.AddRange(
            new Experience
            {
                Role = "Current role",
                Organisation = "Current organisation",
                StartDate = new DateOnly(2023, 8, 1),
                EndDate = null,
                IsCurrent = true,
                IsPublished = true,
                Points =
                {
                    new ExperiencePoint { Text = "Second point", DisplayOrder = 2 },
                    new ExperiencePoint { Text = "First point", DisplayOrder = 1 }
                }
            },
            new Experience
            {
                Role = "Finished role",
                Organisation = "Finished organisation",
                StartDate = new DateOnly(2025, 9, 1),
                EndDate = new DateOnly(2025, 11, 1),
                IsCurrent = false,
                IsPublished = true,
                Points = { new ExperiencePoint { Text = "Point", DisplayOrder = 1 } }
            },
            new Experience
            {
                Role = "Older finished role",
                Organisation = "Older organisation",
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 2, 1),
                IsCurrent = false,
                IsPublished = true
            },
            new Experience
            {
                Role = "Unpublished role",
                Organisation = "Unpublished organisation",
                StartDate = new DateOnly(2026, 1, 1),
                EndDate = null,
                IsCurrent = true,
                IsPublished = false
            });
        await database.Context.SaveChangesAsync();

        var handler = new GetPublicExperiencesHandler(database.Context);
        var result = await handler.Handle(new GetPublicExperiencesQuery(), CancellationToken.None);

        Assert.Equal(
            new[] { "Current role", "Finished role", "Older finished role" },
            result.Select(experience => experience.Role));
        Assert.Equal("Aug 2023 - Present", result[0].DateRangeText);
        Assert.Equal("Sep 2025 - Nov 2025", result[1].DateRangeText);
        Assert.Equal(new[] { "First point", "Second point" }, result[0].Points);
    }

    [Fact]
    public async Task GetExperiencesHandler_IncludesUnpublishedItems()
    {
        await using var database = await TestDatabase.CreateAsync();
        database.Context.Experiences.AddRange(
            new Experience
            {
                Role = "Published role",
                Organisation = "Published organisation",
                StartDate = new DateOnly(2025, 1, 1),
                EndDate = new DateOnly(2025, 2, 1),
                IsCurrent = false,
                IsPublished = true
            },
            new Experience
            {
                Role = "Draft role",
                Organisation = "Draft organisation",
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 2, 1),
                IsCurrent = false,
                IsPublished = false
            });
        await database.Context.SaveChangesAsync();

        var handler = new GetExperiencesHandler(database.Context);
        var result = await handler.Handle(new GetExperiencesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, experience => experience.Role == "Draft role" && !experience.IsPublished);
    }

    [Fact]
    public async Task GetExperienceByIdHandler_MissingId_ThrowsNotFoundException()
    {
        await using var database = await TestDatabase.CreateAsync();
        var handler = new GetExperienceByIdHandler(database.Context);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetExperienceByIdQuery(999), CancellationToken.None));
    }
}
