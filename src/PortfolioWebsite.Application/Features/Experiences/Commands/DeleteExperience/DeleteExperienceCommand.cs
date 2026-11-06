using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.DeleteExperience;

public sealed record DeleteExperienceCommand(int Id) : IRequest;
