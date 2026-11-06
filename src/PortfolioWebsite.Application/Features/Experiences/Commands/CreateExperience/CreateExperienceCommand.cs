using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.CreateExperience;

public sealed record CreateExperienceCommand(
    string Role,
    string Organisation,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent,
    bool IsPublished,
    IReadOnlyList<string> Points) : IRequest<int>;
