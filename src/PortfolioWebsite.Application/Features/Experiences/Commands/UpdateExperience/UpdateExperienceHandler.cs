using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.UpdateExperience;

public sealed class UpdateExperienceHandler : IRequestHandler<UpdateExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExperienceHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .Include(existing => existing.Points)
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (experience is null)
        {
            throw new NotFoundException(nameof(Experience), request.Id);
        }

        var points = NormalizePoints(request.Points);

        experience.Role = request.Role.Trim();
        experience.Organisation = request.Organisation.Trim();
        experience.StartDate = request.StartDate;
        experience.EndDate = request.IsCurrent ? null : request.EndDate;
        experience.IsCurrent = request.IsCurrent;
        experience.IsPublished = request.IsPublished;
        experience.UpdatedAtUtc = DateTime.UtcNow;

        _context.ExperiencePoints.RemoveRange(experience.Points);
        experience.Points = points
            .Select((point, index) => new ExperiencePoint
            {
                Text = point,
                DisplayOrder = index + 1
            })
            .ToList();

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static List<string> NormalizePoints(IReadOnlyList<string> points)
    {
        return points
            .Select(point => point.Trim())
            .Where(point => point.Length > 0)
            .ToList();
    }
}
