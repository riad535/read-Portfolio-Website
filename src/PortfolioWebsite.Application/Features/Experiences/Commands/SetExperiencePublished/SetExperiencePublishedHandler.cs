using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.SetExperiencePublished;

public sealed class SetExperiencePublishedHandler : IRequestHandler<SetExperiencePublishedCommand>
{
    private readonly IApplicationDbContext _context;

    public SetExperiencePublishedHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        SetExperiencePublishedCommand request,
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (experience is null)
        {
            throw new NotFoundException(nameof(Experience), request.Id);
        }

        experience.IsPublished = request.IsPublished;
        experience.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
