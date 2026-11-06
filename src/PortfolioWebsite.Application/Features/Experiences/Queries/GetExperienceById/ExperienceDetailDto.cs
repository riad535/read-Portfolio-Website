namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetExperienceById;

public sealed record ExperienceDetailDto(
    int Id,
    string Role,
    string Organisation,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent,
    bool IsPublished,
    IReadOnlyList<string> Points);
