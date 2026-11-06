using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Experiences.Commands.CreateExperience;

public sealed class CreateExperienceValidator : IRequestValidator<CreateExperienceCommand>
{
    public IEnumerable<ValidationError> Validate(CreateExperienceCommand request)
    {
        var errors = new List<ValidationError>();

        var role = request.Role?.Trim() ?? string.Empty;
        if (role.Length is < 1 or > 150)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.Role),
                "Role is required and must be 150 characters or fewer."));
        }

        var organisation = request.Organisation?.Trim() ?? string.Empty;
        if (organisation.Length is < 1 or > 150)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.Organisation),
                "Organisation is required and must be 150 characters or fewer."));
        }

        if (request.StartDate == default)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.StartDate),
                "Start date is required."));
        }
        else if (request.IsCurrent && request.EndDate is not null)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.EndDate),
                "End date must be empty for a current role."));
        }
        else if (!request.IsCurrent && request.EndDate is null)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.EndDate),
                "End date is required when the role is not current."));
        }
        else if (!request.IsCurrent &&
                 request.EndDate is not null &&
                 request.EndDate < request.StartDate)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.EndDate),
                "End date must be on or after the start date."));
        }

        ValidatePoints(request.Points, errors);

        return errors;
    }

    private static void ValidatePoints(
        IReadOnlyList<string>? points,
        ICollection<ValidationError> errors)
    {
        var normalizedPoints = (points ?? Array.Empty<string>())
            .Select(point => point?.Trim() ?? string.Empty)
            .Where(point => point.Length > 0)
            .ToList();

        if (normalizedPoints.Count > 10)
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.Points),
                "Points must contain 10 items or fewer."));
        }

        if (normalizedPoints.Any(point => point.Length > 500))
        {
            errors.Add(new ValidationError(
                nameof(CreateExperienceCommand.Points),
                "Each point must be 500 characters or fewer."));
        }
    }
}
