namespace PortfolioWebsite.Domain.Entities;

public class ExperiencePoint
{
    public int Id { get; set; }

    public int ExperienceId { get; set; }

    public Experience Experience { get; set; } = null!;

    public string Text { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
