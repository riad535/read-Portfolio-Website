using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.DeleteExperience;

public sealed class DeleteExperienceHandler : IRequestHandler<DeleteExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteExperienceHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .Include(existing => existing.Points)
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (experience is null)
        {
            throw new NotFoundException(nameof(Experience), request.Id);
        }

        _context.Experiences.Remove(experience);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
