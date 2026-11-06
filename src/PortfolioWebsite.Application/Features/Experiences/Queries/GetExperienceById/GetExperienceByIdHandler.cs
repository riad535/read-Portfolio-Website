using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetExperienceById;

public sealed class GetExperienceByIdHandler
    : IRequestHandler<GetExperienceByIdQuery, ExperienceDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetExperienceByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExperienceDetailDto> Handle(
        GetExperienceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var experience = await _context.Experiences
            .AsNoTracking()
            .Where(existing => existing.Id == request.Id)
            .Select(existing => new ExperienceDetailDto(
                existing.Id,
                existing.Role,
                existing.Organisation,
                existing.StartDate,
                existing.EndDate,
                existing.IsCurrent,
                existing.IsPublished,
                existing.Points
                    .OrderBy(point => point.DisplayOrder)
                    .ThenBy(point => point.Id)
                    .Select(point => point.Text)
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (experience is null)
        {
            throw new NotFoundException(nameof(Experience), request.Id);
        }

        return experience;
    }
}
