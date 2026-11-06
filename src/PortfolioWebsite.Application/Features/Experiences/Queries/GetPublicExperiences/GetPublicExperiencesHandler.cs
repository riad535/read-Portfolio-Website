using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetPublicExperiences;

public sealed class GetPublicExperiencesHandler
    : IRequestHandler<GetPublicExperiencesQuery, IReadOnlyList<PublicExperienceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPublicExperiencesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PublicExperienceDto>> Handle(
        GetPublicExperiencesQuery request,
        CancellationToken cancellationToken)
    {
        var experiences = await _context.Experiences
            .AsNoTracking()
            .Where(experience => experience.IsPublished)
            .OrderByDescending(experience => experience.IsCurrent)
            .ThenByDescending(experience => experience.StartDate)
            .ThenBy(experience => experience.Id)
            .Select(experience => new
            {
                experience.Role,
                experience.Organisation,
                experience.StartDate,
                experience.EndDate,
                experience.IsCurrent,
                Points = experience.Points
                    .OrderBy(point => point.DisplayOrder)
                    .ThenBy(point => point.Id)
                    .Select(point => point.Text)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return experiences
            .Select(experience => new PublicExperienceDto(
                experience.Role,
                experience.Organisation,
                FormatDateRange(experience.StartDate, experience.EndDate, experience.IsCurrent),
                experience.Points))
            .ToList();
    }

    private static string FormatDateRange(
        DateOnly startDate,
        DateOnly? endDate,
        bool isCurrent)
    {
        var start = startDate.ToString("MMM yyyy", CultureInfo.InvariantCulture);

        if (isCurrent)
        {
            return $"{start} - Present";
        }

        var end = endDate?.ToString("MMM yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
        return $"{start} - {end}";
    }
}
