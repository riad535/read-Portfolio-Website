using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Projects.Commands.UpdateProject;

public sealed class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        project.Title = request.Title.Trim();
        project.Description = request.Description.Trim();
        project.TechStack = request.TechStack.Trim();
        project.Architecture = NormalizeOptional(request.Architecture);
        project.LiveDemoUrl = NormalizeOptional(request.LiveDemoUrl);
        project.GitHubUrl = NormalizeOptional(request.GitHubUrl);
        project.DisplayOrder = request.DisplayOrder;
        project.IsPublished = request.IsPublished;
        project.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
