namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetExperiences;

public sealed record ExperienceListItemDto(
    int Id,
    string Role,
    string Organisation,
    string DateRangeText,
    bool IsPublished);
