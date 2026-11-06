using MediatR;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.CreateExperience;

public sealed class CreateExperienceHandler : IRequestHandler<CreateExperienceCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateExperienceHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var points = NormalizePoints(request.Points);

        var experience = new Experience
        {
            Role = request.Role.Trim(),
            Organisation = request.Organisation.Trim(),
            StartDate = request.StartDate,
            EndDate = request.IsCurrent ? null : request.EndDate,
            IsCurrent = request.IsCurrent,
            IsPublished = request.IsPublished,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            Points = points
                .Select((point, index) => new ExperiencePoint
                {
                    Text = point,
                    DisplayOrder = index + 1
                })
                .ToList()
        };

        _context.Experiences.Add(experience);
        await _context.SaveChangesAsync(cancellationToken);

        return experience.Id;
    }

    private static List<string> NormalizePoints(IReadOnlyList<string> points)
    {
        return points
            .Select(point => point.Trim())
            .Where(point => point.Length > 0)
            .ToList();
    }
}
