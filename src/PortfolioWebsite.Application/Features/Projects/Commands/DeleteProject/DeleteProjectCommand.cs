using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Commands.DeleteProject;

public sealed record DeleteProjectCommand(int Id) : IRequest;
