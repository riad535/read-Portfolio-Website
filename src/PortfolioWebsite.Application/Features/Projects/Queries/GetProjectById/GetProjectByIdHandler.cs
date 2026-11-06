using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Projects.Queries.GetProjectById;

public sealed class GetProjectByIdHandler
    : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetProjectByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDetailDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .Where(existing => existing.Id == request.Id)
            .Select(existing => new ProjectDetailDto(
                existing.Id,
                existing.Title,
                existing.Description,
                existing.TechStack,
                existing.Architecture,
                existing.LiveDemoUrl,
                existing.GitHubUrl,
                existing.DisplayOrder,
                existing.IsPublished))
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        return project;
    }
}
