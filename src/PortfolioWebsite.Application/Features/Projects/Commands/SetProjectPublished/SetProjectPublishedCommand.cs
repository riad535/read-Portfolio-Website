using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Commands.SetProjectPublished;

public sealed record SetProjectPublishedCommand(int Id, bool IsPublished) : IRequest;
