using MediatR;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var project = new Project
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            TechStack = request.TechStack.Trim(),
            Architecture = NormalizeOptional(request.Architecture),
            LiveDemoUrl = NormalizeOptional(request.LiveDemoUrl),
            GitHubUrl = NormalizeOptional(request.GitHubUrl),
            DisplayOrder = request.DisplayOrder,
            IsPublished = request.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return project.Id;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
