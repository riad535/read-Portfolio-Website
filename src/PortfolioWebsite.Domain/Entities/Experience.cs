using PortfolioWebsite.Domain.Common;

namespace PortfolioWebsite.Domain.Entities;

public class Experience : BaseEntity
{
    public string Role { get; set; } = string.Empty;

    public string Organisation { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public bool IsPublished { get; set; } = true;

    public ICollection<ExperiencePoint> Points { get; set; } = new List<ExperiencePoint>();
}
