using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Features.Skills.Commands.CreateSkillCategory;

public sealed class CreateSkillCategoryHandler : IRequestHandler<CreateSkillCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSkillCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateSkillCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        var iconClass = request.IconClass.Trim();
        var skills = NormalizeSkills(request.Skills);

        var nameExists = await _context.SkillCategories
            .AnyAsync(category => category.Name.ToLower() == name.ToLower(), cancellationToken);

        if (nameExists)
        {
            throw new ValidationException(new[]
            {
                new ValidationError(
                    nameof(CreateSkillCategoryCommand.Name),
                    "A skill category with this name already exists.")
            });
        }

        EnsureUniqueSkills(skills);

        var now = DateTime.UtcNow;

        var category = new SkillCategory
        {
            Name = name,
            IconClass = iconClass,
            DisplayOrder = request.DisplayOrder,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            Skills = skills
                .Select((skill, index) => new Skill
                {
                    Name = skill,
                    DisplayOrder = index + 1
                })
                .ToList()
        };

        _context.SkillCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
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
                    nameof(CreateSkillCategoryCommand.Skills),
                    "Skill names must be unique within a category.")
            });
        }
    }
}
