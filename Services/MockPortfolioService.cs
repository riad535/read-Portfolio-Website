using PortfolioWebsite.Models;

namespace PortfolioWebsite.Services;

public class MockPortfolioService : IPortfolioService
{
    public PortfolioViewModel GetPortfolio()
    {
        return new PortfolioViewModel
        {
            PageTitle = "Md Read Hasan - .NET Backend Developer Portfolio",
            MetaDescription = "Portfolio of Md Read Hasan, a Software Engineering student and C#/.NET backend developer. Projects, skills, certifications and contact details.",
            FooterText = "(c) {current year} Md Read Hasan. All rights reserved.",
            Profile = CreateProfile(),
            Education = CreateEducation(),
            Skills = CreateSkills(),
            Projects = CreateProjects(),
            Certifications = CreateCertifications(),
            Experiences = CreateExperiences(),
            Achievements = CreateAchievements(),
            Navigation = CreateNavigation(),
            ButtonLabels = CreateButtonLabels()
        };
    }

    private static ProfileViewModel CreateProfile()
    {
        return new ProfileViewModel
        {
            FullName = "Md Read Hasan",
            Designation = "Software Developer (Specializing in C#, .NET, & Backend Architecture)",
            Tagline = "Building clean, scalable backend systems with C# and .NET.",
            Location = "Keshabpur, Jashore, Bangladesh",
            Phone = "01948597458",
            Email = "readhasan535@gmail.com",
            GitHubUrl = "https://github.com/riad535",
            LinkedInUrl = "https://www.linkedin.com/in/read-hasan-61a05438b",
            PhotoPath = "/images/profile.jpg",
            PhotoAlt = "Portrait of Md Read Hasan",
            CvPath = "/files/ReadHasan_CV.pdf",
            CvDownloadName = "ReadHasan_CV.pdf",
            Bio = "Dedicated Software Engineering student at Metropolitan University with a solid foundation in programming, application development, and problem-solving. Experienced in building full-stack web and mobile applications using .NET, C++, React, and Flutter, while specializing in clean backend architectures and relational database design."
        };
    }

    private static EducationViewModel CreateEducation()
    {
        return new EducationViewModel
        {
            Degree = "B.Sc. in Software Engineering",
            Institution = "Metropolitan University",
            Period = null,
            Cgpa = null
        };
    }

    private static List<SkillViewModel> CreateSkills()
    {
        return
        [
            new SkillViewModel
            {
                Category = "Backend & Languages",
                Skills =
                [
                    "C#",
                    ".NET Core",
                    "ASP.NET Core MVC",
                    "ASP.NET Web API",
                    "LINQ",
                    "C++",
                    "PHP (Laravel)"
                ]
            },
            new SkillViewModel
            {
                Category = "Architecture & Patterns",
                Skills =
                [
                    "Mediator Pattern",
                    "Repository Pattern",
                    "N-Tier Architecture",
                    "Clean Architecture",
                    "CQRS",
                    "RESTful API"
                ]
            },
            new SkillViewModel
            {
                Category = "Database & ORM",
                Skills =
                [
                    "SQL Server",
                    "Entity Framework Core",
                    "SQL Migrations"
                ]
            },
            new SkillViewModel
            {
                Category = "Frontend & Mobile",
                Skills =
                [
                    "React",
                    "JavaScript",
                    "HTML5",
                    "CSS3",
                    "Bootstrap",
                    "Flutter"
                ]
            },
            new SkillViewModel
            {
                Category = "Tools & Environment",
                Skills =
                [
                    "Visual Studio",
                    "VS Code",
                    "Git",
                    "GitHub",
                    "Windows CLI",
                    "Node.js",
                    "npm"
                ]
            },
            new SkillViewModel
            {
                Category = "Problem Solving",
                Skills =
                [
                    "Data Structures & Algorithms",
                    "Competitive Programming"
                ]
            }
        ];
    }

    private static List<ProjectViewModel> CreateProjects()
    {
        return
        [
            new ProjectViewModel
            {
                Title = "MaleFashion E-Commerce Platform",
                Description = "Custom fashion e-commerce application managing complex product variations, inventory stock, user accounts, and ordering logic.",
                TechStack = "ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap",
                Architecture = "Clean Architecture, Mediator, Repository Pattern, CQRS Pattern",
                LiveDemoUrl = "https://malefashion001-001-site1.ftempurl.com/",
                GitHubUrl = null
            },
            new ProjectViewModel
            {
                Title = "Blood Donation Management System",
                Description = "Developed a responsive Blood Donation Management System featuring role-based access. Empowered admins to manage inventory, donor verification, and system records while enabling donors to schedule donations and patients to place urgent real-time blood requests.",
                TechStack = "Laravel, React, JavaScript, HTML5/CSS3",
                Architecture = "MVC (Model-View-Controller)",
                LiveDemoUrl = null,
                GitHubUrl = "https://github.com/riad535/Blood-Bank-Management-System"
            }
        ];
    }

    private static List<CertificationViewModel> CreateCertifications()
    {
        return
        [
            new CertificationViewModel
            {
                CourseName = "Professional Programming with C#",
                Platform = "DevSkill",
                VerificationUrl = "https://devskill.com/certificate/937/5waplbc2?referralCode=READ019957"
            },
            new CertificationViewModel
            {
                CourseName = "Full Stack ASP.NET Core MVC Web Development",
                Platform = "DevSkill",
                VerificationUrl = null
            }
        ];
    }

    private static List<ExperienceViewModel> CreateExperiences()
    {
        return
        [
            new ExperienceViewModel
            {
                Role = "Programming Mentor",
                Organisation = "MU Programming Olympiad & ACM Workshop",
                DateRange = "Aug 2023 - Present",
                Points =
                [
                    "Actively engaged in content creation, problem design, and mentoring students in C++ and Competitive Programming algorithms."
                ]
            },
            new ExperienceViewModel
            {
                Role = "Laravel Backend Developer Intern",
                Organisation = "RootSoft IT",
                DateRange = "Sept 2025 - Nov 2025",
                Points =
                [
                    "Engineered relational database models using SQL and PHP/Laravel.",
                    "Developed optimized API endpoints for internal web services."
                ]
            }
        ];
    }

    private static List<AchievementViewModel> CreateAchievements()
    {
        return
        [
            new AchievementViewModel
            {
                Title = "MU Inter University Programming Contest (IUPC) - Participant",
                Year = 2024
            },
            new AchievementViewModel
            {
                Title = "SUST Inter University Programming Contest (IUPC) - Participant",
                Year = 2023
            },
            new AchievementViewModel
            {
                Title = "SEC Inter University Programming Contest (IUPC) - Participant",
                Year = 2022
            }
        ];
    }

    private static List<NavigationItemViewModel> CreateNavigation()
    {
        return
        [
            new NavigationItemViewModel
            {
                NavbarLabel = "Home",
                SectionTitle = null,
                AnchorId = "home"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "About",
                SectionTitle = "About Me",
                AnchorId = "about"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Education",
                SectionTitle = "Education",
                AnchorId = "education"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Skills",
                SectionTitle = "Technical Skills",
                AnchorId = "skills"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Projects",
                SectionTitle = "Projects",
                AnchorId = "projects"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Certifications",
                SectionTitle = "Certifications",
                AnchorId = "certifications"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Experience",
                SectionTitle = "Experience",
                AnchorId = "experience"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Achievements",
                SectionTitle = "Achievements",
                AnchorId = "achievements"
            },
            new NavigationItemViewModel
            {
                NavbarLabel = "Contact",
                SectionTitle = "Get In Touch",
                AnchorId = "contact"
            }
        ];
    }

    private static ButtonLabelsViewModel CreateButtonLabels()
    {
        return new ButtonLabelsViewModel
        {
            DownloadCv = "Download CV",
            GitHub = "GitHub",
            LinkedIn = "LinkedIn",
            LiveDemo = "Live Demo",
            ViewCertificate = "View Certificate",
            SendEmail = "Send an Email"
        };
    }
}
