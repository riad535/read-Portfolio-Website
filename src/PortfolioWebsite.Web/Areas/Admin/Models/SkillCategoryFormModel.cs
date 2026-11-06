namespace PortfolioWebsite.Web.Areas.Admin.Models;

public sealed class SkillCategoryFormModel
{
    public string Name { get; set; } = string.Empty;

    public string IconClass { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public string Skills { get; set; } = string.Empty;
}
