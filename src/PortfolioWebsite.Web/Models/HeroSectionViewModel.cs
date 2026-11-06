namespace PortfolioWebsite.Web.Models;

public class HeroSectionViewModel
{
    public ProfileViewModel Profile { get; init; } = new();

    public ButtonLabelsViewModel ButtonLabels { get; init; } = new();
}
