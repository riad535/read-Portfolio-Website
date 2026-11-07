using PortfolioWebsite.Application.Features.Projects.Commands.CreateProject;

namespace PortfolioWebsite.Application.Tests.Validators;

public sealed class ProjectValidatorTests
{
    private readonly CreateProjectValidator _validator = new();

    [Fact]
    public void CreateProjectValidator_EmptyTitle_ReturnsTitleError()
    {
        var errors = _validator.Validate(CreateCommand(title: string.Empty));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.Title));
    }

    [Fact]
    public void CreateProjectValidator_TitleAtMaximumLength_Passes()
    {
        var errors = _validator.Validate(CreateCommand(title: new string('a', 150)));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateProjectValidator_TitleOverMaximumLength_ReturnsTitleError()
    {
        var errors = _validator.Validate(CreateCommand(title: new string('a', 151)));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.Title));
    }

    [Fact]
    public void CreateProjectValidator_DescriptionAtMaximumLength_Passes()
    {
        var errors = _validator.Validate(CreateCommand(description: new string('a', 1000)));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateProjectValidator_DescriptionOverMaximumLength_ReturnsDescriptionError()
    {
        var errors = _validator.Validate(CreateCommand(description: new string('a', 1001)));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.Description));
    }

    [Fact]
    public void CreateProjectValidator_EmptyTechStack_ReturnsTechStackError()
    {
        var errors = _validator.Validate(CreateCommand(techStack: string.Empty));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.TechStack));
    }

    [Fact]
    public void CreateProjectValidator_ArchitectureAtMaximumLength_Passes()
    {
        var errors = _validator.Validate(CreateCommand(architecture: new string('a', 300)));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateProjectValidator_ArchitectureOverMaximumLength_ReturnsArchitectureError()
    {
        var errors = _validator.Validate(CreateCommand(architecture: new string('a', 301)));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.Architecture));
    }

    [Fact]
    public void CreateProjectValidator_RelativeUrl_ReturnsUrlError()
    {
        var errors = _validator.Validate(CreateCommand(liveDemoUrl: "/projects/demo"));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.LiveDemoUrl));
    }

    [Fact]
    public void CreateProjectValidator_FtpUrl_ReturnsUrlError()
    {
        var errors = _validator.Validate(CreateCommand(gitHubUrl: "ftp://example.com/repository"));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.GitHubUrl));
    }

    [Theory]
    [InlineData("http://example.com")]
    [InlineData("https://example.com")]
    public void CreateProjectValidator_HttpAndHttpsUrls_Pass(string url)
    {
        var errors = _validator.Validate(CreateCommand(liveDemoUrl: url, gitHubUrl: url));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateProjectValidator_UrlOverMaximumLength_ReturnsUrlError()
    {
        var url = "https://example.com/" + new string('a', 500);

        var errors = _validator.Validate(CreateCommand(liveDemoUrl: url));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.LiveDemoUrl));
    }

    [Fact]
    public void CreateProjectValidator_NegativeDisplayOrder_ReturnsDisplayOrderError()
    {
        var errors = _validator.Validate(CreateCommand(displayOrder: -1));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateProjectCommand.DisplayOrder));
    }

    [Fact]
    public void CreateProjectValidator_ValidInputWithOptionalFields_Passes()
    {
        var errors = _validator.Validate(CreateCommand());

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateProjectValidator_ValidInputWithoutOptionalFields_Passes()
    {
        var errors = _validator.Validate(CreateCommand(
            architecture: null,
            liveDemoUrl: null,
            gitHubUrl: null));

        Assert.Empty(errors);
    }

    private static CreateProjectCommand CreateCommand(
        string title = "Project",
        string description = "Description",
        string techStack = "C#, .NET",
        string? architecture = "Clean Architecture",
        string? liveDemoUrl = "https://example.com",
        string? gitHubUrl = "https://github.com/example/repository",
        int displayOrder = 1)
    {
        return new CreateProjectCommand(
            title,
            description,
            techStack,
            architecture,
            liveDemoUrl,
            gitHubUrl,
            displayOrder,
            IsPublished: true);
    }
}
