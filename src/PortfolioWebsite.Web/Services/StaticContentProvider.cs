using PortfolioWebsite.Web.Models;

namespace PortfolioWebsite.Web.Services;

public class StaticContentProvider : IStaticContentProvider
{
    public PortfolioViewModel GetStaticContent()
    {
        return new PortfolioViewModel
        {
            PageTitle = "Md Read Hasan - .NET Backend Developer Portfolio",
            MetaDescription = "Portfolio of Md Read Hasan, a Software Engineering student and C#/.NET backend developer. Projects, skills, certifications and contact details.",
            OgTitle = "Md Read Hasan - .NET Backend Developer Portfolio",
            OgDescription = "C#, .NET Core and clean architecture projects by Md Read Hasan.",
            OgImagePath = "/images/profile.jpg",
            FooterText = "(c) {current year} Md Read Hasan. All rights reserved.",
            Profile = CreateProfile(),
            Education = CreateEducation(),
            Certifications = CreateCertifications(),
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
