using PortfolioWebsite.Domain.Common;

namespace PortfolioWebsite.Domain.Entities;

public class SkillCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string IconClass { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
