using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Skills.Commands.UpdateSkillCategory;

public sealed class UpdateSkillCategoryHandler : IRequestHandler<UpdateSkillCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSkillCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateSkillCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.SkillCategories
            .Include(existing => existing.Skills)
            .FirstOrDefaultAsync(existing => existing.Id == request.Id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(SkillCategory), request.Id);
        }

        var name = request.Name.Trim();
        var iconClass = request.IconClass.Trim();
        var skills = NormalizeSkills(request.Skills);

        var nameExists = await _context.SkillCategories
            .AnyAsync(
                existing =>
                    existing.Id != request.Id &&
                    existing.Name.ToLower() == name.ToLower(),
                cancellationToken);

        if (nameExists)
        {
            throw new ValidationException(new[]
            {
                new ValidationError(
                    nameof(UpdateSkillCategoryCommand.Name),
                    "A skill category with this name already exists.")
            });
        }

        EnsureUniqueSkills(skills);

        category.Name = name;
        category.IconClass = iconClass;
        category.DisplayOrder = request.DisplayOrder;
        category.UpdatedAtUtc = DateTime.UtcNow;

        _context.Skills.RemoveRange(category.Skills);
        category.Skills = skills
            .Select((skill, index) => new Skill
            {
                Name = skill,
                DisplayOrder = index + 1
            })
            .ToList();

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static List<string> NormalizeSkills(IReadOnlyList<string> skills)
    {
        return skills
            .Select(skill => skill.Trim())
            .Where(skill => skill.Length > 0)
            .ToList();
    }

    private static void EnsureUniqueSkills(IReadOnlyCollection<string> skills)
    {
        var uniqueSkills = skills.Distinct(StringComparer.OrdinalIgnoreCase);

        if (skills.Count != uniqueSkills.Count())
        {
            throw new ValidationException(new[]
            {
                new ValidationError(
                    nameof(UpdateSkillCategoryCommand.Skills),
                    "Skill names must be unique within a category.")
            });
        }
    }
}
