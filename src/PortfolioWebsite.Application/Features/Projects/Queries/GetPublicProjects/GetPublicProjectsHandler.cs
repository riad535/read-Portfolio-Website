using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Projects.Queries.GetPublicProjects;

public sealed class GetPublicProjectsHandler
    : IRequestHandler<GetPublicProjectsQuery, IReadOnlyList<PublicProjectDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPublicProjectsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PublicProjectDto>> Handle(
        GetPublicProjectsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(project => project.IsPublished)
            .OrderBy(project => project.DisplayOrder)
            .ThenBy(project => project.Id)
            .Select(project => new PublicProjectDto(
                project.Title,
                project.Description,
                project.TechStack,
                project.Architecture,
                project.LiveDemoUrl,
                project.GitHubUrl))
            .ToListAsync(cancellationToken);
    }
}
