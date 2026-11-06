namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetPublicExperiences;

public sealed record PublicExperienceDto(
    string Role,
    string Organisation,
    string DateRangeText,
    IReadOnlyList<string> Points);
