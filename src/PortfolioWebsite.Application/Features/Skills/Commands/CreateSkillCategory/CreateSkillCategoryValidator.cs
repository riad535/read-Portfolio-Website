using PortfolioWebsite.Application.Common.Constants;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Skills.Commands.CreateSkillCategory;

public sealed class CreateSkillCategoryValidator : IRequestValidator<CreateSkillCategoryCommand>
{
    public IEnumerable<ValidationError> Validate(CreateSkillCategoryCommand request)
    {
        var errors = new List<ValidationError>();

        var name = request.Name?.Trim() ?? string.Empty;
        if (name.Length is < 1 or > 100)
        {
            errors.Add(new ValidationError(
                nameof(CreateSkillCategoryCommand.Name),
                "Name is required and must be 100 characters or fewer."));
        }

        var iconClass = request.IconClass?.Trim() ?? string.Empty;
        if (!AllowedIcons.All.Contains(iconClass))
        {
            errors.Add(new ValidationError(
                nameof(CreateSkillCategoryCommand.IconClass),
                "Select a valid icon."));
        }

        if (request.DisplayOrder < 0)
        {
            errors.Add(new ValidationError(
                nameof(CreateSkillCategoryCommand.DisplayOrder),
                "Display order must be zero or greater."));
        }

        ValidateSkills(request.Skills, errors);

        return errors;
    }

    private static void ValidateSkills(
        IReadOnlyList<string>? skills,
        ICollection<ValidationError> errors)
    {
        var normalizedSkills = (skills ?? Array.Empty<string>())
            .Select(skill => skill?.Trim() ?? string.Empty)
            .Where(skill => skill.Length > 0)
            .ToList();

        if (normalizedSkills.Count is < 1 or > 30)
        {
            errors.Add(new ValidationError(
                nameof(CreateSkillCategoryCommand.Skills),
                "Skills must contain between 1 and 30 items."));
        }

        if (normalizedSkills.Any(skill => skill.Length > 60))
        {
            errors.Add(new ValidationError(
                nameof(CreateSkillCategoryCommand.Skills),
                "Each skill must be 60 characters or fewer."));
        }

        if (normalizedSkills.Count != normalizedSkills.Distinct(StringComparer.OrdinalIgnoreCase).Count())
        {
            errors.Add(new ValidationError(
                nameof(CreateSkillCategoryCommand.Skills),
                "Skill names must be unique within a category."));
        }
    }
}
