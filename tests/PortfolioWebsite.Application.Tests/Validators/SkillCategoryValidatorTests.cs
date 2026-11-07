using PortfolioWebsite.Application.Features.Skills.Commands.CreateSkillCategory;

namespace PortfolioWebsite.Application.Tests.Validators;

public sealed class SkillCategoryValidatorTests
{
    private readonly CreateSkillCategoryValidator _validator = new();

    [Fact]
    public void CreateSkillCategoryValidator_EmptyName_ReturnsNameError()
    {
        var errors = _validator.Validate(CreateCommand(name: string.Empty));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.Name));
    }

    [Fact]
    public void CreateSkillCategoryValidator_NameAtMaximumLength_Passes()
    {
        var errors = _validator.Validate(CreateCommand(name: new string('a', 100)));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateSkillCategoryValidator_NameOverMaximumLength_ReturnsNameError()
    {
        var errors = _validator.Validate(CreateCommand(name: new string('a', 101)));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.Name));
    }

    [Fact]
    public void CreateSkillCategoryValidator_IconNotAllowed_ReturnsIconError()
    {
        var errors = _validator.Validate(CreateCommand(iconClass: "bi-unknown"));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.IconClass));
    }

    [Fact]
    public void CreateSkillCategoryValidator_NegativeDisplayOrder_ReturnsDisplayOrderError()
    {
        var errors = _validator.Validate(CreateCommand(displayOrder: -1));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.DisplayOrder));
    }

    [Fact]
    public void CreateSkillCategoryValidator_ZeroSkills_ReturnsSkillsError()
    {
        var errors = _validator.Validate(CreateCommand(skills: Array.Empty<string>()));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.Skills));
    }

    [Fact]
    public void CreateSkillCategoryValidator_MoreThanThirtySkills_ReturnsSkillsError()
    {
        var skills = Enumerable.Range(1, 31).Select(index => $"Skill {index}").ToArray();

        var errors = _validator.Validate(CreateCommand(skills: skills));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.Skills));
    }

    [Fact]
    public void CreateSkillCategoryValidator_SkillOverSixtyCharacters_ReturnsSkillsError()
    {
        var errors = _validator.Validate(CreateCommand(skills: new[] { new string('a', 61) }));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.Skills));
    }

    [Fact]
    public void CreateSkillCategoryValidator_DuplicateSkillsIgnoringCase_ReturnsSkillsError()
    {
        var errors = _validator.Validate(CreateCommand(skills: new[] { "C#", "c#" }));

        Assert.Contains(errors, error => error.PropertyName == nameof(CreateSkillCategoryCommand.Skills));
    }

    [Fact]
    public void CreateSkillCategoryValidator_BlankLines_AreIgnored()
    {
        var errors = _validator.Validate(CreateCommand(skills: new[] { string.Empty, "   ", "C#" }));

        Assert.Empty(errors);
    }

    [Fact]
    public void CreateSkillCategoryValidator_ValidInput_Passes()
    {
        var errors = _validator.Validate(CreateCommand());

        Assert.Empty(errors);
    }

    private static CreateSkillCategoryCommand CreateCommand(
        string name = "Backend",
        string iconClass = "bi-code-slash",
        int displayOrder = 1,
        IReadOnlyList<string>? skills = null)
    {
        return new CreateSkillCategoryCommand(
            name,
            iconClass,
            displayOrder,
            skills ?? new[] { "C#" });
    }
}
