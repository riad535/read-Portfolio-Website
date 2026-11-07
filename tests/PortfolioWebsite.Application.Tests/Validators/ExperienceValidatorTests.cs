using PortfolioWebsite.Application.Features.Experiences.Commands.CreateExperience;

namespace PortfolioWebsite.Application.Tests.Validators;

public sealed class ExperienceValidatorTests
{
    private static readonly DateOnly Start = new(2024, 1, 1);
    private static readonly DateOnly End = new(2024, 6, 1);

    private readonly CreateExperienceValidator _validator = new();

    [Fact]
    public void CreateExperienceValidator_EmptyRole_ReturnsRoleError()
    {
        var errors = _validator.Validate(CreateCommand(role: string.Empty, endDate: End));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.Role));
    }

    [Fact]
    public void CreateExperienceValidator_EmptyOrganisation_ReturnsOrganisationError()
    {
        var errors = _validator.Validate(CreateCommand(organisation: string.Empty, endDate: End));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.Organisation));
    }

    [Fact]
    public void CreateExperienceValidator_MissingStartDate_ReturnsStartDateError()
    {
        var command = new CreateExperienceCommand(
            "Developer",
            "Company",
            default,
            End,
            IsCurrent: false,
            IsPublished: true,
            new[] { "Work" });

        var errors = _validator.Validate(command);

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.StartDate));
    }

    [Fact]
    public void CreateExperienceValidator_CurrentRoleWithEndDate_ReturnsEndDateError()
    {
        var errors = _validator.Validate(CreateCommand(endDate: End, isCurrent: true));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.EndDate));
    }

    [Fact]
    public void CreateExperienceValidator_NotCurrentWithoutEndDate_ReturnsEndDateError()
    {
        var errors = _validator.Validate(CreateCommand(endDate: null, isCurrent: false));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.EndDate));
    }

    [Fact]
    public void CreateExperienceValidator_EndDateBeforeStartDate_ReturnsEndDateError()
    {
        var errors = _validator.Validate(CreateCommand(endDate: Start.AddDays(-1)));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.EndDate));
    }

    [Fact]
    public void CreateExperienceValidator_EndDateEqualStartDate_Passes()
    {
        var errors = _validator.Validate(CreateCommand(endDate: Start));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateExperienceValidator_MoreThanTenPoints_ReturnsPointsError()
    {
        var points = Enumerable.Range(1, 11).Select(index => $"Point {index}").ToArray();

        var errors = _validator.Validate(CreateCommand(endDate: End, points: points));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.Points));
    }

    [Fact]
    public void CreateExperienceValidator_PointOverFiveHundredCharacters_ReturnsPointsError()
    {
        var errors = _validator.Validate(CreateCommand(endDate: End, points: new[] { new string('a', 501) }));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateExperienceCommand.Points));
    }

    [Fact]
    public void CreateExperienceValidator_BlankPoints_AreIgnored()
    {
        var errors = _validator.Validate(CreateCommand(endDate: End, points: new[] { string.Empty, "   " }));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateExperienceValidator_ValidCurrentRole_Passes()
    {
        var errors = _validator.Validate(CreateCommand(endDate: null, isCurrent: true));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateExperienceValidator_ValidFinishedRole_Passes()
    {
        var errors = _validator.Validate(CreateCommand(endDate: End));

        Assert.Empty(errors);
    }

    private static CreateExperienceCommand CreateCommand(
        string role = "Developer",
        string organisation = "Company",
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        bool isCurrent = false,
        IReadOnlyList<string>? points = null)
    {
        return new CreateExperienceCommand(
            role,
            organisation,
            startDate ?? Start,
            endDate,
            isCurrent,
            IsPublished: true,
            points ?? new[] { "Work" });
    }
}
