using MediatR;

namespace PortfolioWebsite.Application.Features.Skills.Commands.CreateSkillCategory;

public sealed record CreateSkillCategoryCommand(
    string Name,
    string IconClass,
    int DisplayOrder,
    IReadOnlyList<string> Skills) : IRequest<int>;
