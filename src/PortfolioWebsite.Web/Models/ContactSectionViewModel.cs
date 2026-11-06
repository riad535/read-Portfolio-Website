namespace PortfolioWebsite.Web.Models;

public class ContactSectionViewModel
{
    public ProfileViewModel Profile { get; init; } = new();

    public ButtonLabelsViewModel ButtonLabels { get; init; } = new();
}
