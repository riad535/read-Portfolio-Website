# CONTENT.md

Single source of truth for **all text and data** shown on the portfolio.
Static sections are written into `StaticContentProvider`. Dynamic sections are loaded into SQL Server by `DbSeeder`. Both must be filled from this file **exactly**. Do not rephrase, add or remove anything.

## Data Source Status (v2.0)

| Section | Runtime source | Editable in admin panel |
|---|---|---|
| 1. Profile | `StaticContentProvider` (code) | No |
| 2. Education | `StaticContentProvider` (code) | No |
| 3. Skills | **SQL Server** (seeded from this file) | **Yes** |
| 4. Projects | **SQL Server** (seeded from this file) | **Yes** |
| 5. Certifications | `StaticContentProvider` (code) | No |
| 6. Experience | **SQL Server** (seeded from this file) | **Yes** |
| 7. Achievements | `StaticContentProvider` (code) | No |
| 8-10. Labels, buttons, SEO | Code / views | No |

For the three dynamic sections this file is **only the initial (seed) data**. After the first seed, the database is the live source and the admin panel changes it. Seeding runs only when the tables are empty.

**Rules for agents**
- `null` means **not provided yet**. Keep the model property `null` and **hide** the related UI element. Never invent a value.
- Items marked `(proposed)` are drafts by the document author. Use them as written, the owner may edit them later.
- Field names in backticks map to the properties of the view models (static sections) or to the entities and columns in `docs/PRD.md` Section 9 (dynamic sections).

---

## 1. Profile (`ProfileViewModel`)

**Static section** (code, not in the database, not editable in admin).

| Field | Value |
|---|---|
| `FullName` | Md Read Hasan |
| `Designation` | Software Developer (Specializing in C#, .NET, & Backend Architecture) |
| `Tagline` (proposed) | Building clean, scalable backend systems with C# and .NET. |
| `Location` | Keshabpur, Jashore, Bangladesh |
| `Phone` | 01948597458 |
| `Email` | readhasan535@gmail.com |
| `GitHubUrl` | https://github.com/riad535 |
| `LinkedInUrl` | https://www.linkedin.com/in/read-hasan-61a05438b |
| `PhotoPath` | /images/profile.jpg |
| `PhotoAlt` | Portrait of Md Read Hasan |
| `CvPath` | /files/ReadHasan_CV.pdf |
| `CvDownloadName` | ReadHasan_CV.pdf |

**Bio**

Dedicated Software Engineering student at Metropolitan University with a solid foundation in programming, application development, and problem-solving. Experienced in building full-stack web and mobile applications using .NET, C++, React, and Flutter, while specializing in clean backend architectures and relational database design.

---

## 2. Education (`EducationViewModel`)

**Static section** (code, not in the database, not editable in admin).

| Field | Value |
|---|---|
| `Degree` | B.Sc. in Software Engineering |
| `Institution` | Metropolitan University |
| `Period` | null |
| `Cgpa` | null |

---

## 3. Skills (seed data for `SkillCategory` and `Skill`)

**Dynamic section** (SQL Server, editable in admin). Each row is one `SkillCategories` record. `Name` maps to the category name, `IconClass` to a Bootstrap Icon, `DisplayOrder` to the `#` column. Each item in `Skills` becomes one `Skills` record; its `DisplayOrder` is its position in the list (starting at 1).

| # (`DisplayOrder`) | `Name` | `IconClass` | `Skills` |
|---|---|---|---|
| 1 | Backend & Languages | bi-code-slash | C#, .NET Core, ASP.NET Core MVC, ASP.NET Web API, LINQ, C++, PHP (Laravel) |
| 2 | Architecture & Patterns | bi-diagram-3 | Mediator Pattern, Repository Pattern, N-Tier Architecture, Clean Architecture, CQRS, RESTful API |
| 3 | Database & ORM | bi-database | SQL Server, Entity Framework Core, SQL Migrations |
| 4 | Frontend & Mobile | bi-phone | React, JavaScript, HTML5, CSS3, Bootstrap, Flutter |
| 5 | Tools & Environment | bi-tools | Visual Studio, VS Code, Git, GitHub, Windows CLI, Node.js, npm |
| 6 | Problem Solving | bi-puzzle | Data Structures & Algorithms, Competitive Programming |

---

## 4. Projects (seed data for `Project`)

**Dynamic section** (SQL Server, editable in admin). `DisplayOrder` and `IsPublished` are database fields added for the admin panel. Public site shows only `IsPublished = true`, ordered by `DisplayOrder`.

### 4.1 MaleFashion E-Commerce Platform

| Field | Value |
|---|---|
| `Title` | MaleFashion E-Commerce Platform |
| `Description` | Custom fashion e-commerce application managing complex product variations, inventory stock, user accounts, and ordering logic. |
| `TechStack` | ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap |
| `Architecture` | Clean Architecture, Mediator, Repository Pattern, CQRS Pattern |
| `LiveDemoUrl` | https://malefashion001-001-site1.ftempurl.com/ |
| `GitHubUrl` | null |
| `DisplayOrder` | 1 |
| `IsPublished` | true |

### 4.2 Blood Donation Management System

| Field | Value |
|---|---|
| `Title` | Blood Donation Management System |
| `Description` | Responsive web application to maintain blood donor pools, blood group records, and emergency requests. |
| `TechStack` | Laravel, React, JavaScript, HTML5/CSS3 |
| `Architecture` | null |
| `LiveDemoUrl` | null |
| `GitHubUrl` | null |
| `DisplayOrder` | 2 |
| `IsPublished` | true |

---

## 5. Certifications (`CertificationViewModel`)

**Static section** (code, not in the database, not editable in admin).

| # | `CourseName` | `Platform` | `VerificationUrl` |
|---|---|---|---|
| 1 | Professional Programming with C# | DevSkill | https://devskill.com/certificate/937/5waplbc2?referralCode=READ019957 |
| 2 | Full Stack ASP.NET Core MVC Web Development | DevSkill | null |

---

## 6. Experience (seed data for `Experience` and `ExperiencePoint`)

**Dynamic section** (SQL Server, editable in admin). Dates are stored with month precision (first day of the month). The public site orders current roles first, then by `StartDate` descending, and shows only `IsPublished = true`.

Date text on the site is **generated** as `MMM yyyy - MMM yyyy`, or `MMM yyyy - Present` for a current role. Because of this, "Sept 2025" in the original text is displayed as **Sep 2025** (accepted).

### 6.1 Programming Mentor

| Field | Value |
|---|---|
| `Role` | Programming Mentor |
| `Organisation` | MU Programming Olympiad & ACM Workshop |
| `StartDate` | 2023-08 |
| `EndDate` | null |
| `IsCurrent` | true |
| `IsPublished` | true |
| Displayed as | Aug 2023 - Present |

`Points` (stored as `ExperiencePoint` rows, `DisplayOrder` = position starting at 1)
- Actively engaged in content creation, problem design, and mentoring students in C++ and Competitive Programming algorithms.

### 6.2 Laravel Backend Developer Intern

| Field | Value |
|---|---|
| `Role` | Laravel Backend Developer Intern |
| `Organisation` | RootSoft IT |
| `StartDate` | 2025-09 |
| `EndDate` | 2025-11 |
| `IsCurrent` | false |
| `IsPublished` | true |
| Displayed as | Sep 2025 - Nov 2025 |

`Points` (stored as `ExperiencePoint` rows, `DisplayOrder` = position starting at 1)
- Engineered relational database models using SQL and PHP/Laravel.
- Developed optimized API endpoints for internal web services.

---

## 7. Achievements (`AchievementViewModel`)

**Static section** (code, not in the database, not editable in admin). Display newest first.

| # | `Title` | `Year` |
|---|---|---|
| 1 | MU Inter University Programming Contest (IUPC) - Participant | 2024 |
| 2 | SUST Inter University Programming Contest (IUPC) - Participant | 2023 |
| 3 | SEC Inter University Programming Contest (IUPC) - Participant | 2022 |

---

## 8. Section Headings and Navbar Labels

If a dynamic section (Skills, Projects, Experience) has no visible items, hide the section **and** its navbar link.

Use these exact labels for navbar links and section titles. The `id` is the HTML anchor.

| Order | Navbar label | Section title | `id` |
|---|---|---|---|
| 1 | Home | (Hero, no title) | `home` |
| 2 | About | About Me | `about` |
| 3 | Education | Education | `education` |
| 4 | Skills | Technical Skills | `skills` |
| 5 | Projects | Projects | `projects` |
| 6 | Certifications | Certifications | `certifications` |
| 7 | Experience | Experience | `experience` |
| 8 | Achievements | Achievements | `achievements` |
| 9 | Contact | Get In Touch | `contact` |

---

## 9. Button and Link Labels

| Purpose | Label | Behaviour |
|---|---|---|
| CV download | Download CV | Downloads `CvPath`. Hide if the file is missing. |
| GitHub (profile) | GitHub | Opens `GitHubUrl` in a new tab |
| LinkedIn | LinkedIn | Opens `LinkedInUrl` in a new tab |
| Project demo | Live Demo | Show only if `LiveDemoUrl` is not null. New tab. |
| Project code | GitHub | Show only if the project's `GitHubUrl` is not null. New tab. |
| Certificate | View Certificate | Show only if `VerificationUrl` is not null. New tab. |
| Email | Send an Email | `mailto:` link using `Email` |

---

## 10. Page Metadata (SEO)

| Field | Value |
|---|---|
| `<title>` | Md Read Hasan - .NET Backend Developer Portfolio |
| `meta description` | Portfolio of Md Read Hasan, a Software Engineering student and C#/.NET backend developer. Projects, skills, certifications and contact details. |
| `og:title` | Md Read Hasan - .NET Backend Developer Portfolio |
| `og:description` | C#, .NET Core and clean architecture projects by Md Read Hasan. |
| `og:image` | /images/profile.jpg |
| `Footer text` | (c) {current year} Md Read Hasan. All rights reserved. |

(The SEO values above are `(proposed)`.)

---

## 11. Admin Account (not stored here)

The admin email and password are **not** part of this file and must never be committed. They are supplied through configuration:

| Key | Local | Production |
|---|---|---|
| `Admin:Email` | `dotnet user-secrets` | Environment variable `Admin__Email` |
| `Admin:Password` | `dotnet user-secrets` | Environment variable `Admin__Password` (first start only) |

---

## 12. Open Items (`null` values to be filled by the owner)

- Education `Period` and `Cgpa`
- Certificate #2 `VerificationUrl`
- MaleFashion `GitHubUrl`
- Blood Donation `Architecture`, `LiveDemoUrl`, `GitHubUrl`

(Project values can be filled later through the admin panel. Update this file as well if you want a fresh seed to match.)
