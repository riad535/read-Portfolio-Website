using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategories;

public sealed class GetSkillCategoriesHandler
    : IRequestHandler<GetSkillCategoriesQuery, IReadOnlyList<SkillCategoryListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSkillCategoriesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SkillCategoryListItemDto>> Handle(
        GetSkillCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.SkillCategories
            .AsNoTracking()
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Id)
            .Select(category => new SkillCategoryListItemDto(
                category.Id,
                category.Name,
                category.IconClass,
                category.DisplayOrder,
                category.Skills.Count))
            .ToListAsync(cancellationToken);
    }
}
