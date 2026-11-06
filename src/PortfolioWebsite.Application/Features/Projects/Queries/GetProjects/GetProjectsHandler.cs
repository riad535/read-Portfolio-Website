using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Projects.Queries.GetProjects;

public sealed class GetProjectsHandler
    : IRequestHandler<GetProjectsQuery, IReadOnlyList<ProjectListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProjectListItemDto>> Handle(
        GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderBy(project => project.DisplayOrder)
            .ThenBy(project => project.Id)
            .Select(project => new ProjectListItemDto(
                project.Id,
                project.Title,
                project.DisplayOrder,
                project.IsPublished))
            .ToListAsync(cancellationToken);
    }
}
