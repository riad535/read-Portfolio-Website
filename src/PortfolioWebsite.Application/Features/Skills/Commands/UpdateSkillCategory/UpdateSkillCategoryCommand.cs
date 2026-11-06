using MediatR;

namespace PortfolioWebsite.Application.Features.Skills.Commands.UpdateSkillCategory;

public sealed record UpdateSkillCategoryCommand(
    int Id,
    string Name,
    string IconClass,
    int DisplayOrder,
    IReadOnlyList<string> Skills) : IRequest;
