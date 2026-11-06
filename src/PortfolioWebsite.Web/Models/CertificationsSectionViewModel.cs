namespace PortfolioWebsite.Web.Models;

public class CertificationsSectionViewModel
{
    public List<CertificationViewModel> Certifications { get; init; } = [];

    public ButtonLabelsViewModel ButtonLabels { get; init; } = new();
}
