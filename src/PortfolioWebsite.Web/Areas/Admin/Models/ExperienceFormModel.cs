namespace PortfolioWebsite.Web.Areas.Admin.Models;

public sealed class ExperienceFormModel
{
    public string Role { get; set; } = string.Empty;

    public string Organisation { get; set; } = string.Empty;

    public string StartDate { get; set; } = string.Empty;

    public string? EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public string Points { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = true;
}
