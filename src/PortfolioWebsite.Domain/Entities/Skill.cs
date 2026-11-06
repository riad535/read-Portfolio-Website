namespace PortfolioWebsite.Domain.Entities;

public class Skill
{
    public int Id { get; set; }

    public int SkillCategoryId { get; set; }

    public SkillCategory SkillCategory { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
