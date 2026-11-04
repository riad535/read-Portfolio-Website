# Project Requirements Document (PRD)

## Personal Developer Portfolio Website


## 1. Document Metadata

| **Field**             | **Value**                                                                                       |
|-----------------------|-------------------------------------------------------------------------------------------------|
| **Project Name**      | Md Read Hasan - Professional Developer Portfolio                                                |
| **Author / Owner**    | Md Read Hasan                                                                                   |
| **Version / Status**  | v1.1 - Draft (items marked [TBD] must be filled before implementation is considered complete) |
| **Target Framework**  | ASP.NET Core MVC on .NET 8 (LTS), In-Memory / Static Mock Data approach                         |
| **Styling Framework** | Bootstrap 5 (Responsive UI)                                                                     |
| **Data Storage**      | None (no database). All content served from an in-memory service.                               |

## 2. Project Overview & Objectives

### 2.1 Purpose

To design and build a clean, modern, and high-performance personal portfolio website that highlights professional backend development skills, competitive programming background, live project deployments, and architectural design patterns.

### 2.2 Target Audience

- Technical Recruiters & HR Managers
- Software Engineering Tech Leads & Hiring Managers

- Potential Clients & Professional Peers

### 2.3 Core Goals

- Showcase deep technical expertise in **C#, .NET Core, SQL Server, and enterprise design patterns**.
- Highlight academic background (B.Sc. in Software Engineering at Metropolitan University) and competitive programming / mentorship milestones (IUPC participations).

- Feature live production project deployments (e.g., MaleFashion E-Commerce) alongside open-source GitHub repositories.
- Make it easy for visitors to reach the owner (GitHub, LinkedIn, email) and download the CV in one click.

## 3. Scope

### 3.1 In Scope

- Single-page portfolio (one Index view) with anchor-based sections.
- Static / hard-coded data served through an in-memory service (no database).

- Fully responsive layout (mobile, tablet, desktop) using Bootstrap 5.
- Profile photo, downloadable CV (PDF), and links to GitHub, LinkedIn and email.

- Basic SEO metadata, accessibility best practices, and public deployment.

### 3.2 Out of Scope (v1)

- Database or any persistent storage (SQL Server, EF Core).
- Admin panel / CMS for editing content.

- User authentication, registration or login.
- Blog or article publishing.

- Server-side contact form with email sending (see Section 6).
- Multi-language support and dark/light theme toggle (can be considered later).

## 4. Functional Requirements

Page sections appear in this order. Priority: **M** = Must, **S** = Should.

| **ID** | **Requirement**                                                                                                                                                  | **Priority** |
|--------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------|
| FR-01  | **Navbar:** sticky top navigation with links to every section; collapses into a hamburger menu on mobile.                                                        | M            |
| FR-02  | **Smooth scroll:** clicking a navbar link scrolls smoothly to the matching section; active link is highlighted while scrolling.                                  | M            |
| FR-03  | **Hero:** shows profile photo, full name, designation, short tagline, and buttons: "Download CV", "GitHub", "LinkedIn".                                          | M            |
| FR-04  | **About:** professional bio and location.                                                                                                                        | M            |
| FR-05  | **Education:** degree, institution and study period.                                                                                                             | M            |
| FR-06  | **Skills:** grouped by category (Backend, Architecture, Database, Frontend, Tools, Problem Solving), displayed as cards or badges.                               | M            |
| FR-07  | **Projects:** one card per project with description, tech stack, architecture, and action buttons. "Live Demo" and "GitHub" buttons appear only if a URL exists. | M            |
| FR-08  | **External links:** all external links (Live Demo, GitHub, LinkedIn, certificates) open in a new tab with rel="noopener noreferrer".                             | M            |
| FR-09  | **Certifications:** list of courses with institution and "View Certificate" link.                                                                                | M            |
| FR-10  | **Experience:** timeline of work experience and mentorship with date range and bullet points.                                                                    | M            |
| FR-11  | **Achievements:** list of IUPC participations with year.                                                                                                         | M            |
| FR-12  | **Contact:** email (mailto link), phone, location, GitHub and LinkedIn icons. No server-side form (Section 6).                                                   | M            |
| FR-13  | **CV download:** the "Download CV" button downloads the PDF from wwwroot/files.                                                                                  | M            |
| FR-14  | **Footer:** copyright line and social icons.                                                                                                                     | S            |
| FR-15  | **Back-to-top** button appears after scrolling down.                                                                                                             | S            |
| FR-16  | **Custom 404 page** for unknown routes.                                                                                                                          | S            |

## 5. Data & Content Specifications

### 5.1 Profile Summary & Contact

- **Full Name:** Md Read Hasan
- **Designation:** Software Developer (Specializing in C#, .NET, & Backend Architecture)

- **Location:** Keshabpur, Jashore, Bangladesh
- **Phone:** 01948597458

- **Email:** readhasan535@gmail.com
- **Professional Bio:** Dedicated Software Engineering student at Metropolitan University with a solid foundation in programming, application development, and problem-solving. Experienced in building full-stack web and mobile applications using .NET, C++, React, and Flutter, while specializing in clean backend architectures and relational database design.

### 5.2 Online Presence & Assets

| **Item**          | **Value**                                                                                                                                                                                                                                         |
|-------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **GitHub**        | <https://github.com/riad535>                                                                                                                                                                                                                      |
| **LinkedIn**      | <https://www.linkedin.com/in/read-hasan-61a05438b>                                                                                                                                                                                                |
| **Profile Photo** | Provided by owner (portrait, plain white background). Stored at wwwroot/images/profile.jpg. Prepare before use: crop to square (head and shoulders), resize to about 600x600 px, compress to under 150 KB. Alt text: "Portrait of Md Read Hasan". |
| **CV / Resume**   | Stored at wwwroot/files/ReadHasan_CV.pdf (uploaded manually by owner). Linked from Hero and Contact sections.                                                                                                                                     |

Note: the LinkedIn link above is the clean profile URL. The original shared link contained tracking parameters (utm_source, etc.) which are not needed.

### 5.3 Education

- **Degree:** B.Sc. in Software Engineering
- **Institution:** Metropolitan University

- **Period / Expected Graduation:** **[TBD]**
- **CGPA / Highlights (optional):** **[TBD]**

### 5.4 Technical Skills & Architectural Patterns

| **Category**                | **Skills & Technologies**                                                                        |
|-----------------------------|--------------------------------------------------------------------------------------------------|
| **Backend & Languages**     | C#, .NET Core, ASP.NET Core MVC, ASP.NET Web API, LINQ, C++, PHP (Laravel)                       |
| **Architecture & Patterns** | Mediator Pattern, Repository Pattern, N-Tier Architecture, Clean Architecture, CQRS, RESTful API |
| **Database & ORM**          | SQL Server, Entity Framework Core, SQL Migrations                                                |
| **Frontend & Mobile**       | React, JavaScript, HTML5, CSS3, Bootstrap, Flutter                                               |
| **Tools & Environment**     | Visual Studio, VS Code, Git, GitHub, Windows CLI, Node.js, npm                                   |
| **Problem Solving**         | Data Structures & Algorithms, Competitive Programming                                            |

### 5.5 Certifications & Professional Training

| **Course Name**                                 | **Platform** | **Verification Link**                                                                     |
|-------------------------------------------------|--------------|-------------------------------------------------------------------------------------------|
| **Professional Programming with C#**            | DevSkill     | [View Certificate](https://devskill.com/certificate/937/5waplbc2?referralCode=READ019957) |
| **Full Stack ASP.NET Core MVC Web Development** | DevSkill     | **[TBD: correct link - currently same as the course above]**                    |

### 5.6 Projects Showcase Data

**1. MaleFashion E-Commerce Platform**

- **Description:** Custom fashion e-commerce application managing complex product variations, inventory stock, user accounts, and ordering logic.
- **Tech Stack:** ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap.

- **Architecture:** Clean Architecture, Mediator, Repository Pattern, CQRS Pattern.
- **Live Demo:** <https://malefashion001-001-site1.ftempurl.com/>

- **GitHub Repository:** **[TBD]**

**2. Blood Donation Management System**

- **Description:** Responsive web application to maintain blood donor pools, blood group records, and emergency requests.
- **Tech Stack:** Laravel, React, JavaScript, HTML5/CSS3.

- **Live Demo:** **\[TBD or N/A\]**
- **GitHub Repository:** **\[TBD or N/A\]**

### 5.7 Work Experience & Mentorship Timeline

**Laravel Backend Developer Intern** \| RootSoft IT (Sept 2025 - Nov 2025)

- Engineered relational database models using SQL and PHP/Laravel.
- Developed optimized API endpoints for internal web services.

**Programming Mentor** \| MU Programming Olympiad & ACM Workshop (Aug 2023 - Present)

- Actively engaged in content creation, problem design, and mentoring students in C++ and Competitive Programming algorithms.

### 5.8 Achievements & Competitive Programming (IUPC)

- **SEC Inter University Programming Contest (IUPC)** - 2022 Participant
- **SUST Inter University Programming Contest (IUPC)** - 2023 Participant

- **MU Inter University Programming Contest (IUPC)** - 2024 Participant

## 6. Contact Strategy

**Decision:** No contact form in v1.

Reason: there is no database and no backend infrastructure for email. A form would need SMTP configuration or a third-party service, adding secrets management, spam protection and failure handling for little benefit on a portfolio.

- Primary contact: **mailto:** link on the email address (opens the visitor's mail app).
- Secondary: GitHub and LinkedIn buttons, phone number as text.

- Spam mitigation: avoid exposing the email as plain text in page source where possible (render via mailto link / small script).
- Future option: add a form later using Formspree (no backend code) or SMTP via MailKit (see Section 10).

## 7. Non-Functional Requirements

| **Area**            | **Requirement**                                                                                                                                                                                       |
|---------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Performance**     | Page loads in under 2 seconds on a typical 4G connection. Images compressed (profile photo under 150 KB), CSS/JS minified, static file caching enabled. Target Lighthouse Performance 90+.            |
| **Responsiveness**  | Bootstrap 5 breakpoints: \<576 px (mobile), 576-767 px, 768-991 px (tablet), 992-1199 px, 1200 px+ (desktop). No horizontal scrolling at any width from 320 px up.                                    |
| **SEO**             | Unique \<title\> and meta description; Open Graph tags (og:title, og:description, og:image, og:url); semantic HTML (header, nav, main, section, footer); one h1 per page; robots.txt and sitemap.xml. |
| **Accessibility**   | Alt text on images; colour contrast meeting WCAG AA; keyboard-navigable menu with visible focus; aria-labels on icon-only links. Target Lighthouse Accessibility 90+.                                 |
| **Browser support** | Latest two versions of Chrome, Edge, Firefox and Safari; Chrome and Safari on mobile.                                                                                                                 |
| **Security**        | HTTPS only (HSTS in production); external links use rel="noopener noreferrer"; no secrets in source code.                                                                                             |
| **Maintainability** | Content separated from views through IPortfolioService; no content hard-coded in .cshtml files.                                                                                                       |

## 8. Technical Architecture & Data Strategy

### 8.1 Technology Choices

- **Framework:** ASP.NET Core MVC, .NET 8 (LTS).
- **UI:** Razor views, Bootstrap 5, small vanilla JavaScript file for scroll behaviour. Icons: Bootstrap Icons.

- **Data:** in-memory C# objects exposed through an interface (no database).

### 8.2 Dependency Injection (Program.cs)

```csharp
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IPortfolioService, MockPortfolioService>();
```

Singleton is appropriate because the data is static and read-only.

### 8.3 Data Flow

HomeController receives IPortfolioService by constructor injection, calls GetPortfolio() to get one PortfolioViewModel, and passes it to Views/Home/Index.cshtml. Each section is rendered by a partial view that receives only the part of the model it needs.

### 8.4 Folder Structure

```text
PortfolioWebsite/
├── Controllers/
│   └── HomeController.cs            # Injects IPortfolioService
├── Models/
│   ├── PortfolioViewModel.cs        # Aggregate: holds all sections below
│   ├── ProfileViewModel.cs          # Personal details, contact, social links
│   ├── EducationViewModel.cs        # Degree, institution, period
│   ├── SkillViewModel.cs            # Skill categories
│   ├── ProjectViewModel.cs          # Projects + optional URLs
│   ├── CertificationViewModel.cs    # Course, platform, verification link
│   ├── ExperienceViewModel.cs       # Work & mentorship timeline
│   └── AchievementViewModel.cs      # IUPC achievements
├── Services/
│   ├── IPortfolioService.cs         # GetPortfolio() contract
│   └── MockPortfolioService.cs      # In-memory data provider
├── Views/
│   ├── Home/
│   │   └── Index.cshtml             # Composes section partials
│   └── Shared/
│       ├── _Layout.cshtml           # Master layout
│       ├── _Hero.cshtml             # Section partials
│       ├── _About.cshtml
│       ├── _Skills.cshtml
│       ├── _Projects.cshtml
│       ├── _Certifications.cshtml
│       ├── _Experience.cshtml
│       ├── _Achievements.cshtml
│       └── _Contact.cshtml
├── wwwroot/
│   ├── css/site.css
│   ├── js/site.js                   # Scroll, back-to-top
│   ├── images/profile.jpg           # Profile photo (+ og-image.jpg)
│   └── files/ReadHasan_CV.pdf       # Downloadable CV
└── Program.cs                       # DI registration & middleware
```

## 9. Future Scope & Migration Plan

The interface-based design lets the data source change without touching controllers or views.

- Add a new class **EfPortfolioService : IPortfolioService** that reads from SQL Server using Entity Framework Core.
- Change one line in Program.cs: replace MockPortfolioService with EfPortfolioService (register as Scoped).

- Add DbContext, entities and migrations; seed the database with the current mock data.
- Controller, view models and Razor views remain unchanged.

Possible later features: admin panel for editing content, blog, contact form (Formspree or MailKit), dark mode, visit analytics.

## 10. Deployment Plan

- **Hosting:** **\[TBD: e.g. Somee / SmarterASP.NET (same type of host as the MaleFashion demo) or Azure App Service\]**
- **Publish:** dotnet publish -c Release, then upload or deploy through the host's tool.

- **Source control:** GitHub repository with a main branch (stable) and feature branches for new work.
- **CI/CD (optional):** GitHub Actions workflow to build on every push.

- **Domain:** **\[TBD: free host subdomain or custom domain\]**. HTTPS must be enabled.

## 11. Acceptance Criteria & Milestones

### 11.1 Acceptance Criteria

| **ID** | **Criterion**                                                                                        |
|--------|------------------------------------------------------------------------------------------------------|
| AC-01  | All sections render fully from MockPortfolioService data; no content is hard-coded in views.         |
| AC-02  | Navbar links scroll smoothly to the correct sections and highlight the active one.                   |
| AC-03  | Layout does not break at 320 px, 768 px, 1024 px and 1440 px widths.                                 |
| AC-04  | Every external link opens in a new tab; "Live Demo" opens the MaleFashion site.                      |
| AC-05  | "Download CV" downloads ReadHasan_CV.pdf; GitHub and LinkedIn buttons open the correct profiles.     |
| AC-06  | Lighthouse scores 90+ in Performance, Accessibility, Best Practices and SEO.                         |
| AC-07  | Page title, meta description and Open Graph tags are present.                                        |
| AC-08  | The site is publicly accessible over HTTPS and works on the latest Chrome, Edge, Firefox and Safari. |
| AC-09  | No [TBD] item remains in the final content.                                                        |

### 11.2 Milestones

| **Phase**   | **Deliverable**                                                    | **Done when**                              |
|-------------|--------------------------------------------------------------------|--------------------------------------------|
| **Phase 1** | Project setup, Bootstrap, layout, DI registration                  | Empty page runs with navbar and footer     |
| **Phase 2** | View models, IPortfolioService, MockPortfolioService with all data | Controller returns full PortfolioViewModel |
| **Phase 3** | All section partials and styling, photo and CV added               | AC-01 to AC-05 pass                        |
| **Phase 4** | SEO, accessibility, performance tuning, 404 page                   | AC-06 and AC-07 pass                       |
| **Phase 5** | Deployment, final content review, remove all TBD                   | AC-08 and AC-09 pass                       |

## 12. Open Items (Owner Action Required)

- Correct certificate link for "Full Stack ASP.NET Core MVC Web Development".
- GitHub repository URLs for MaleFashion and Blood Donation projects (or mark N/A).

- Education period / expected graduation (and CGPA if desired).
- Final CV PDF to place in wwwroot/files/.

- Hosting provider and domain decision.
