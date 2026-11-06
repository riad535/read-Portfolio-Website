using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Skills.Queries.GetPublicSkills;

public sealed class GetPublicSkillsHandler
    : IRequestHandler<GetPublicSkillsQuery, IReadOnlyList<PublicSkillCategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPublicSkillsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PublicSkillCategoryDto>> Handle(
        GetPublicSkillsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.SkillCategories
            .AsNoTracking()
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Id)
            .Select(category => new PublicSkillCategoryDto(
                category.Name,
                category.IconClass,
                category.Skills
                    .OrderBy(skill => skill.DisplayOrder)
                    .ThenBy(skill => skill.Id)
                    .Select(skill => skill.Name)
                    .ToList()))
            .ToListAsync(cancellationToken);
    }
}
