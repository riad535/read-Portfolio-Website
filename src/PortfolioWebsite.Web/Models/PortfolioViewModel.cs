namespace PortfolioWebsite.Web.Models;

public class PortfolioViewModel
{
    public string PageTitle { get; init; } = string.Empty;

    public string MetaDescription { get; init; } = string.Empty;

    public string FooterText { get; init; } = string.Empty;

    public ProfileViewModel Profile { get; init; } = new();

    public EducationViewModel Education { get; init; } = new();

    public List<SkillViewModel> Skills { get; init; } = [];

    public List<ProjectViewModel> Projects { get; init; } = [];

    public List<CertificationViewModel> Certifications { get; init; } = [];

    public List<ExperienceViewModel> Experiences { get; init; } = [];

    public List<AchievementViewModel> Achievements { get; init; } = [];

    public List<NavigationItemViewModel> Navigation { get; init; } = [];

    public ButtonLabelsViewModel ButtonLabels { get; init; } = new();
}
