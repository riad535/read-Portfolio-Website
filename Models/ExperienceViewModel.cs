namespace PortfolioWebsite.Models;

public class ExperienceViewModel
{
    public string Role { get; init; } = string.Empty;

    public string Organisation { get; init; } = string.Empty;

    public string DateRange { get; init; } = string.Empty;

    public List<string> Points { get; init; } = [];
}
