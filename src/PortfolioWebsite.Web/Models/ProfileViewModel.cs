namespace PortfolioWebsite.Web.Models;

public class ProfileViewModel
{
    public string FullName { get; init; } = string.Empty;

    public string Designation { get; init; } = string.Empty;

    public string Tagline { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? GitHubUrl { get; init; }

    public string? LinkedInUrl { get; init; }

    public string PhotoPath { get; init; } = string.Empty;

    public string PhotoAlt { get; init; } = string.Empty;

    public string CvPath { get; init; } = string.Empty;

    public string CvDownloadName { get; init; } = string.Empty;

    public string Bio { get; init; } = string.Empty;
}
