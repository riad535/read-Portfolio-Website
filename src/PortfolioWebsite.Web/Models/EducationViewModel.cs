namespace PortfolioWebsite.Web.Models;

public class EducationViewModel
{
    public string Degree { get; init; } = string.Empty;

    public string Institution { get; init; } = string.Empty;

    public string? Period { get; init; }

    public string? Cgpa { get; init; }
}
