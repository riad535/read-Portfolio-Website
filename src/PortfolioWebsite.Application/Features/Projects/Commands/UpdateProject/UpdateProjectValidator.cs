using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Projects.Commands.UpdateProject;

public sealed class UpdateProjectValidator : IRequestValidator<UpdateProjectCommand>
{
    public IEnumerable<ValidationError> Validate(UpdateProjectCommand request)
    {
        var errors = new List<ValidationError>();

        var title = request.Title?.Trim() ?? string.Empty;
        if (title.Length is < 1 or > 150)
        {
            errors.Add(new ValidationError(
                nameof(UpdateProjectCommand.Title),
                "Title is required and must be 150 characters or fewer."));
        }

        var description = request.Description?.Trim() ?? string.Empty;
        if (description.Length is < 1 or > 1000)
        {
            errors.Add(new ValidationError(
                nameof(UpdateProjectCommand.Description),
                "Description is required and must be 1000 characters or fewer."));
        }

        var techStack = request.TechStack?.Trim() ?? string.Empty;
        if (techStack.Length is < 1 or > 300)
        {
            errors.Add(new ValidationError(
                nameof(UpdateProjectCommand.TechStack),
                "Tech stack is required and must be 300 characters or fewer."));
        }

        var architecture = request.Architecture?.Trim() ?? string.Empty;
        if (architecture.Length > 300)
        {
            errors.Add(new ValidationError(
                nameof(UpdateProjectCommand.Architecture),
                "Architecture must be 300 characters or fewer."));
        }

        ValidateUrl(
            request.LiveDemoUrl,
            nameof(UpdateProjectCommand.LiveDemoUrl),
            errors);

        ValidateUrl(
            request.GitHubUrl,
            nameof(UpdateProjectCommand.GitHubUrl),
            errors);

        if (request.DisplayOrder < 0)
        {
            errors.Add(new ValidationError(
                nameof(UpdateProjectCommand.DisplayOrder),
                "Display order must be zero or greater."));
        }

        return errors;
    }

    private static void ValidateUrl(
        string? value,
        string propertyName,
        ICollection<ValidationError> errors)
    {
        var url = value?.Trim() ?? string.Empty;
        if (url.Length == 0)
        {
            return;
        }

        if (url.Length > 500 || !IsAbsoluteHttpUrl(url))
        {
            errors.Add(new ValidationError(
                propertyName,
                "Enter a valid absolute http or https URL of 500 characters or fewer."));
        }
    }

    private static bool IsAbsoluteHttpUrl(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
