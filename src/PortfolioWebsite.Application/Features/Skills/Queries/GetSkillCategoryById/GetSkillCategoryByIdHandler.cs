using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategoryById;

public sealed class GetSkillCategoryByIdHandler
    : IRequestHandler<GetSkillCategoryByIdQuery, SkillCategoryDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetSkillCategoryByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SkillCategoryDetailDto> Handle(
        GetSkillCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _context.SkillCategories
            .AsNoTracking()
            .Where(existing => existing.Id == request.Id)
            .Select(existing => new SkillCategoryDetailDto(
                existing.Id,
                existing.Name,
                existing.IconClass,
                existing.DisplayOrder,
                existing.Skills
                    .OrderBy(skill => skill.DisplayOrder)
                    .ThenBy(skill => skill.Id)
                    .Select(skill => skill.Name)
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(SkillCategory), request.Id);
        }

        return category;
    }
}
