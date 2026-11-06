using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Skills.Commands.DeleteSkillCategory;

public sealed class DeleteSkillCategoryValidator : IRequestValidator<DeleteSkillCategoryCommand>
{
    public IEnumerable<ValidationError> Validate(DeleteSkillCategoryCommand request)
    {
        if (request.Id <= 0)
        {
            yield return new ValidationError(
                nameof(DeleteSkillCategoryCommand.Id),
                "A valid skill category id is required.");
        }
    }
}
