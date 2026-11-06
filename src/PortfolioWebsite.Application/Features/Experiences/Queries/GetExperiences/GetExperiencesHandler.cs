using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetExperiences;

public sealed class GetExperiencesHandler
    : IRequestHandler<GetExperiencesQuery, IReadOnlyList<ExperienceListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetExperiencesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ExperienceListItemDto>> Handle(
        GetExperiencesQuery request,
        CancellationToken cancellationToken)
    {
        var experiences = await _context.Experiences
            .AsNoTracking()
            .OrderByDescending(experience => experience.IsCurrent)
            .ThenByDescending(experience => experience.StartDate)
            .ThenBy(experience => experience.Id)
            .Select(experience => new
            {
                experience.Id,
                experience.Role,
                experience.Organisation,
                experience.StartDate,
                experience.EndDate,
                experience.IsCurrent,
                experience.IsPublished
            })
            .ToListAsync(cancellationToken);

        return experiences
            .Select(experience => new ExperienceListItemDto(
                experience.Id,
                experience.Role,
                experience.Organisation,
                FormatDateRange(experience.StartDate, experience.EndDate, experience.IsCurrent),
                experience.IsPublished))
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
