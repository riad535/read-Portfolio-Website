using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.SetExperiencePublished;

public sealed record SetExperiencePublishedCommand(int Id, bool IsPublished) : IRequest;
