using MediatR;

namespace PortfolioWebsite.Application.Features.Skills.Commands.DeleteSkillCategory;

public sealed record DeleteSkillCategoryCommand(int Id) : IRequest;
