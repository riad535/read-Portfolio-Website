namespace PortfolioWebsite.Models;

public class CertificationsSectionViewModel
{
    public List<CertificationViewModel> Certifications { get; init; } = [];

    public ButtonLabelsViewModel ButtonLabels { get; init; } = new();
}
