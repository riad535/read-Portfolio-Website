using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.UpdateExperience;

public sealed record UpdateExperienceCommand(
    int Id,
    string Role,
    string Organisation,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent,
    bool IsPublished,
    IReadOnlyList<string> Points) : IRequest;
