namespace PortfolioWebsite.Web.Models;

public class NavigationItemViewModel
{
    public string NavbarLabel { get; init; } = string.Empty;

    public string? SectionTitle { get; init; }

    public string AnchorId { get; init; } = string.Empty;
}
