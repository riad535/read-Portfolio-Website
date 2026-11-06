using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Domain.Entities;
using PortfolioWebsite.Infrastructure.Persistence;

namespace PortfolioWebsite.Infrastructure.Persistence.Seed;

public class DbSeeder
{
    private readonly ApplicationDbContext _context;

    public DbSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedSkillCategoriesAsync(cancellationToken);
        await SeedProjectsAsync(cancellationToken);
        await SeedExperiencesAsync(cancellationToken);
    }

    private async Task SeedSkillCategoriesAsync(CancellationToken cancellationToken)
    {
        if (await _context.SkillCategories.AnyAsync(cancellationToken))
        {
            return;
        }

        var categories = new[]
        {
            CreateSkillCategory(
                "Backend & Languages",
                "bi-code-slash",
                1,
                "C#",
                ".NET Core",
                "ASP.NET Core MVC",
                "ASP.NET Web API",
                "LINQ",
                "C++",
                "PHP (Laravel)"),
            CreateSkillCategory(
                "Architecture & Patterns",
                "bi-diagram-3",
                2,
                "Mediator Pattern",
                "Repository Pattern",
                "N-Tier Architecture",
                "Clean Architecture",
                "CQRS",
                "RESTful API"),
            CreateSkillCategory(
                "Database & ORM",
                "bi-database",
                3,
                "SQL Server",
                "Entity Framework Core",
                "SQL Migrations"),
            CreateSkillCategory(
                "Frontend & Mobile",
                "bi-phone",
                4,
                "React",
                "JavaScript",
                "HTML5",
                "CSS3",
                "Bootstrap",
                "Flutter"),
            CreateSkillCategory(
                "Tools & Environment",
                "bi-tools",
                5,
                "Visual Studio",
                "VS Code",
                "Git",
                "GitHub",
                "Windows CLI",
                "Node.js",
                "npm"),
            CreateSkillCategory(
                "Problem Solving",
                "bi-puzzle",
                6,
                "Data Structures & Algorithms",
                "Competitive Programming")
        };

        _context.SkillCategories.AddRange(categories);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProjectsAsync(CancellationToken cancellationToken)
    {
        if (await _context.Projects.AnyAsync(cancellationToken))
        {
            return;
        }

        _context.Projects.AddRange(
            new Project
            {
                Title = "MaleFashion E-Commerce Platform",
                Description = "Custom fashion e-commerce application managing complex product variations, inventory stock, user accounts, and ordering logic.",
                TechStack = "ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap",
                Architecture = "Clean Architecture, Mediator, Repository Pattern, CQRS Pattern",
                LiveDemoUrl = "https://malefashion001-001-site1.ftempurl.com/",
                GitHubUrl = null,
                DisplayOrder = 1,
                IsPublished = true
            },
            new Project
            {
                Title = "Blood Donation Management System",
                Description = "Responsive web application to maintain blood donor pools, blood group records, and emergency requests.",
                TechStack = "Laravel, React, JavaScript, HTML5/CSS3",
                Architecture = null,
                LiveDemoUrl = null,
                GitHubUrl = null,
                DisplayOrder = 2,
                IsPublished = true
            });

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedExperiencesAsync(CancellationToken cancellationToken)
    {
        if (await _context.Experiences.AnyAsync(cancellationToken))
        {
            return;
        }

        _context.Experiences.AddRange(
            new Experience
            {
                Role = "Programming Mentor",
                Organisation = "MU Programming Olympiad & ACM Workshop",
                StartDate = new DateOnly(2023, 8, 1),
                EndDate = null,
                IsCurrent = true,
                IsPublished = true,
                Points =
                {
                    new ExperiencePoint
                    {
                        Text = "Actively engaged in content creation, problem design, and mentoring students in C++ and Competitive Programming algorithms.",
                        DisplayOrder = 1
                    }
                }
            },
            new Experience
            {
                Role = "Laravel Backend Developer Intern",
                Organisation = "RootSoft IT",
                StartDate = new DateOnly(2025, 9, 1),
                EndDate = new DateOnly(2025, 11, 1),
                IsCurrent = false,
                IsPublished = true,
                Points =
                {
                    new ExperiencePoint
                    {
                        Text = "Engineered relational database models using SQL and PHP/Laravel.",
                        DisplayOrder = 1
                    },
                    new ExperiencePoint
                    {
                        Text = "Developed optimized API endpoints for internal web services.",
                        DisplayOrder = 2
                    }
                }
            });

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static SkillCategory CreateSkillCategory(
        string name,
        string iconClass,
        int displayOrder,
        params string[] skills)
    {
        var category = new SkillCategory
        {
            Name = name,
            IconClass = iconClass,
            DisplayOrder = displayOrder
        };

        for (var index = 0; index < skills.Length; index++)
        {
            category.Skills.Add(new Skill
            {
                Name = skills[index],
                DisplayOrder = index + 1
            });
        }

        return category;
    }
}
