using MediatR;
using PortfolioWebsite.Application.Features.Experiences.Queries.GetPublicExperiences;
using PortfolioWebsite.Application.Features.Projects.Queries.GetPublicProjects;
using PortfolioWebsite.Application.Features.Skills.Queries.GetPublicSkills;
using PortfolioWebsite.Web.Models;

namespace PortfolioWebsite.Web.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IStaticContentProvider _staticContentProvider;
    private readonly ISender _sender;

    public PortfolioService(IStaticContentProvider staticContentProvider, ISender sender)
    {
        _staticContentProvider = staticContentProvider;
        _sender = sender;
    }

    public async Task<PortfolioViewModel> GetPortfolioAsync(
        CancellationToken cancellationToken = default)
    {
        var staticContent = _staticContentProvider.GetStaticContent();

        var skillCategories = await _sender.Send(new GetPublicSkillsQuery(), cancellationToken);
        var projects = await _sender.Send(new GetPublicProjectsQuery(), cancellationToken);
        var experiences = await _sender.Send(new GetPublicExperiencesQuery(), cancellationToken);

        var skills = skillCategories
            .Select(category => new SkillViewModel
            {
                Category = category.Name,
                Skills = category.Skills.ToList()
            })
            .ToList();

        var projectViewModels = projects
            .Select(project => new ProjectViewModel
            {
                Title = project.Title,
                Description = project.Description,
                TechStack = project.TechStack,
                Architecture = project.Architecture,
                LiveDemoUrl = project.LiveDemoUrl,
                GitHubUrl = project.GitHubUrl
            })
            .ToList();

        var experienceViewModels = experiences
            .Select(experience => new ExperienceViewModel
            {
                Role = experience.Role,
                Organisation = experience.Organisation,
                DateRange = experience.DateRangeText,
                Points = experience.Points.ToList()
            })
            .ToList();

        var hiddenAnchors = new HashSet<string>();
        AddHiddenAnchor(hiddenAnchors, "skills", skills.Count);
        AddHiddenAnchor(hiddenAnchors, "projects", projectViewModels.Count);
        AddHiddenAnchor(hiddenAnchors, "experience", experienceViewModels.Count);

        var navigation = staticContent.Navigation
            .Where(item => !hiddenAnchors.Contains(item.AnchorId))
            .ToList();

        return new PortfolioViewModel
        {
            PageTitle = staticContent.PageTitle,
            MetaDescription = staticContent.MetaDescription,
            FooterText = staticContent.FooterText,
            Profile = staticContent.Profile,
            Education = staticContent.Education,
            Skills = skills,
            Projects = projectViewModels,
            Certifications = staticContent.Certifications,
            Experiences = experienceViewModels,
            Achievements = staticContent.Achievements,
            Navigation = navigation,
            ButtonLabels = staticContent.ButtonLabels
        };
    }

    private static void AddHiddenAnchor(HashSet<string> hiddenAnchors, string anchorId, int itemCount)
    {
        if (itemCount == 0)
        {
            hiddenAnchors.Add(anchorId);
        }
    }
}
