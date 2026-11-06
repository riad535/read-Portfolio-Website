using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Skills.Commands.DeleteSkillCategory;

public sealed class DeleteSkillCategoryHandler : IRequestHandler<DeleteSkillCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSkillCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteSkillCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.SkillCategories
            .Include(existing => existing.Skills)
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(SkillCategory), request.Id);
        }

        _context.SkillCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
