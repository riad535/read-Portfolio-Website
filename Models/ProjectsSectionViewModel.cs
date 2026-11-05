namespace PortfolioWebsite.Models;

public class ProjectsSectionViewModel
{
    public List<ProjectViewModel> Projects { get; init; } = [];

    public ButtonLabelsViewModel ButtonLabels { get; init; } = new();
}
