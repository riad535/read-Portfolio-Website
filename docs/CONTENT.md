# CONTENT.md

Single source of truth for **all text and data** shown on the portfolio.
`MockPortfolioService` must be filled from this file **exactly**. Do not rephrase, add or remove anything.

**Rules for agents**
- `null` means **not provided yet**. Keep the model property `null` and **hide** the related UI element. Never invent a value.
- Items marked `(proposed)` are drafts by the document author. Use them as written, the owner may edit them later.
- Field names in backticks map to the properties of the view models in `docs/PRD.md` Section 8.4.

---

## 1. Profile (`ProfileViewModel`)

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

| Field | Value |
|---|---|
| `Degree` | B.Sc. in Software Engineering |
| `Institution` | Metropolitan University |
| `Period` | null |
| `Cgpa` | null |

---

## 3. Skills (`SkillViewModel`)

Display in this order. Each category has a list of skills (one badge per item).

| # | `Category` | `Skills` |
|---|---|---|
| 1 | Backend & Languages | C#, .NET Core, ASP.NET Core MVC, ASP.NET Web API, LINQ, C++, PHP (Laravel) |
| 2 | Architecture & Patterns | Mediator Pattern, Repository Pattern, N-Tier Architecture, Clean Architecture, CQRS, RESTful API |
| 3 | Database & ORM | SQL Server, Entity Framework Core, SQL Migrations |
| 4 | Frontend & Mobile | React, JavaScript, HTML5, CSS3, Bootstrap, Flutter |
| 5 | Tools & Environment | Visual Studio, VS Code, Git, GitHub, Windows CLI, Node.js, npm |
| 6 | Problem Solving | Data Structures & Algorithms, Competitive Programming |

---

## 4. Projects (`ProjectViewModel`)

Display in this order.

### 4.1 MaleFashion E-Commerce Platform

| Field | Value |
|---|---|
| `Title` | MaleFashion E-Commerce Platform |
| `Description` | Custom fashion e-commerce application managing complex product variations, inventory stock, user accounts, and ordering logic. |
| `TechStack` | ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap |
| `Architecture` | Clean Architecture, Mediator, Repository Pattern, CQRS Pattern |
| `LiveDemoUrl` | https://malefashion001-001-site1.ftempurl.com/ |
| `GitHubUrl` | null |

### 4.2 Blood Donation Management System

| Field | Value |
|---|---|
| `Title` | Blood Donation Management System |
| `Description` | Responsive web application to maintain blood donor pools, blood group records, and emergency requests. |
| `TechStack` | Laravel, React, JavaScript, HTML5/CSS3 |
| `Architecture` | null |
| `LiveDemoUrl` | null |
| `GitHubUrl` | null |

---

## 5. Certifications (`CertificationViewModel`)

| # | `CourseName` | `Platform` | `VerificationUrl` |
|---|---|---|---|
| 1 | Professional Programming with C# | DevSkill | https://devskill.com/certificate/937/5waplbc2?referralCode=READ019957 |
| 2 | Full Stack ASP.NET Core MVC Web Development | DevSkill | null |

---

## 6. Experience (`ExperienceViewModel`)

Display newest first.

### 6.1 Programming Mentor

| Field | Value |
|---|---|
| `Role` | Programming Mentor |
| `Organisation` | MU Programming Olympiad & ACM Workshop |
| `DateRange` | Aug 2023 - Present |

`Points`
- Actively engaged in content creation, problem design, and mentoring students in C++ and Competitive Programming algorithms.

### 6.2 Laravel Backend Developer Intern

| Field | Value |
|---|---|
| `Role` | Laravel Backend Developer Intern |
| `Organisation` | RootSoft IT |
| `DateRange` | Sept 2025 - Nov 2025 |

`Points`
- Engineered relational database models using SQL and PHP/Laravel.
- Developed optimized API endpoints for internal web services.

---

## 7. Achievements (`AchievementViewModel`)

Display newest first.

| # | `Title` | `Year` |
|---|---|---|
| 1 | MU Inter University Programming Contest (IUPC) - Participant | 2024 |
| 2 | SUST Inter University Programming Contest (IUPC) - Participant | 2023 |
| 3 | SEC Inter University Programming Contest (IUPC) - Participant | 2022 |

---

## 8. Section Headings and Navbar Labels

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

## 11. Open Items (`null` values to be filled by the owner)

- Education `Period` and `Cgpa`
- Certificate #2 `VerificationUrl`
- MaleFashion `GitHubUrl`
- Blood Donation `Architecture`, `LiveDemoUrl`, `GitHubUrl`
