using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Projects.Commands.SetProjectPublished;

public sealed class SetProjectPublishedHandler : IRequestHandler<SetProjectPublishedCommand>
{
    private readonly IApplicationDbContext _context;

    public SetProjectPublishedHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        SetProjectPublishedCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        project.IsPublished = request.IsPublished;
        project.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
