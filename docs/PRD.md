# Project Requirements Document (PRD)

## Personal Developer Portfolio Website - v2.0 (Dynamic Content + Admin Panel)

---

## 1. Document Metadata

| Field | Value |
|---|---|
| **Project Name** | Md Read Hasan - Professional Developer Portfolio |
| **Author / Owner** | Md Read Hasan |
| **Version / Status** | v2.0 - Draft |
| **Replaces** | v1.1 (static site, no database) |
| **Target Framework** | ASP.NET Core MVC on .NET 10 (LTS), `net10.0` |
| **Architecture** | Clean Architecture + CQRS (MediatR 12.5.0) |
| **Database** | SQL Server (Entity Framework Core 10) |
| **UI** | Razor views, Bootstrap 5, Bootstrap Icons |

**What changed from v1.1**
- Skills, Projects and Experience move from in-memory data to SQL Server and are managed from an admin panel.
- Architecture changes from a single MVC project to Clean Architecture with CQRS.
- SEO / performance and Deployment are now the **last** phases (after the admin panel).
- Phases 1 to 3 (static site) are complete and remain valid.

---

## 2. Project Overview and Objectives

### 2.1 Purpose
A modern, high-performance personal portfolio that highlights backend development skills, competitive programming background, live projects and architectural design patterns, with an admin panel so the owner can update Skills, Projects and Experience without changing code.

### 2.2 Target Audience
- Technical recruiters and HR managers
- Software engineering tech leads and hiring managers
- Potential clients and professional peers

### 2.3 Core Goals
- Showcase expertise in C#, .NET, SQL Server and enterprise design patterns.
- Make Skills, Projects and Experience editable through a secure admin panel.
- Let visitors reach the owner and download the CV in one click.
- Keep the code itself a demonstration of Clean Architecture and CQRS.

---

## 3. Scope

### 3.1 In Scope
- Single-page public portfolio (anchor-based sections), responsive.
- **Static content in code:** Profile, Education, Certifications, Achievements.
- **Dynamic content in SQL Server:** Skills (categories and items), Projects, Experience.
- Admin panel for **one** admin user: create, edit, delete (and publish/unpublish for Projects and Experience) of the three dynamic sections.
- ASP.NET Core Identity with cookie authentication for the admin.
- Database seeding of the initial data from `docs/CONTENT.md`.
- Clean Architecture, CQRS, EF Core migrations.
- SEO, accessibility, performance tuning, deployment (last phases).

### 3.2 Out of Scope
- Admin editing for Profile, Education, Certifications, Achievements (stay static).
- Public registration, multiple users, roles, social login, password reset UI, two-factor authentication.
- Image or file upload (photo and CV are placed manually in `wwwroot/`).
- Blog, contact form with email sending, comments.
- Response caching, CDN, multi-language, dark/light toggle, visit analytics.

---

## 4. Functional Requirements

Priority: **M** = Must, **S** = Should.

### 4.1 Public Site

| ID | Requirement | Priority |
|---|---|---|
| FR-01 | **Navbar:** sticky top navigation with links to every section; hamburger menu on mobile. | M |
| FR-02 | **Smooth scroll** to sections; active link highlighted while scrolling. | M |
| FR-03 | **Hero:** photo, name, designation, tagline, buttons: Download CV, GitHub, LinkedIn. | M |
| FR-04 | **About:** bio, location, contact info. | M |
| FR-05 | **Education:** degree and institution. | M |
| FR-06 | **Skills:** cards per category, skills as chips. Data from SQL Server. | M |
| FR-07 | **Projects:** cards with description, tech stack, architecture; "Live Demo" and "GitHub" only if URL exists. Only **published** projects. Data from SQL Server. | M |
| FR-08 | All external links open in a new tab with `rel="noopener noreferrer"`. | M |
| FR-09 | **Certifications:** list with "View Certificate" only if URL exists. | M |
| FR-10 | **Experience:** timeline with date range and bullet points. Only **published** entries. Data from SQL Server. | M |
| FR-11 | **Achievements:** IUPC list with year. | M |
| FR-12 | **Contact:** `mailto:` link, phone, location, GitHub and LinkedIn. No form. | M |
| FR-13 | **CV download** button downloads the PDF from `wwwroot/files`. | M |
| FR-14 | **Footer:** copyright and social icons. | S |
| FR-15 | **Back-to-top** button after scrolling. | S |
| FR-16 | **Custom 404 page.** | S |
| FR-17 | If a dynamic section has no items, **hide the whole section** (and its navbar link). | S |

### 4.2 Admin Panel

| ID | Requirement | Priority |
|---|---|---|
| FR-A01 | **Login** at `/admin/login` with email and password. No registration page. No "forgot password". | M |
| FR-A02 | **Logout** (POST request). | M |
| FR-A03 | Every admin page except login requires authentication. Anonymous users are redirected to login. | M |
| FR-A04 | **Dashboard** at `/admin` showing counts (skill categories, projects, experiences) and quick links. | M |
| FR-A05 | **Skill categories:** list, create, edit, delete. Fields: name, icon (dropdown), display order, skills (one per line). | M |
| FR-A06 | **Projects:** list, create, edit, delete. Fields in Section 9. Publish/unpublish toggle. | M |
| FR-A07 | **Experience:** list, create, edit, delete. Month pickers for start/end, "current role" checkbox, points (one per line), publish toggle. | M |
| FR-A08 | **Server-side validation** (Section 10) with clear messages next to fields. Invalid data is never saved. | M |
| FR-A09 | **Delete confirmation** modal; delete is a POST action. | M |
| FR-A10 | **Flash messages** after create, update, delete, and on errors. | M |
| FR-A11 | Changes appear on the public site **immediately** (no cache). | M |
| FR-A12 | Admin pages send `noindex`; no link to admin anywhere on the public site; `robots.txt` disallows `/admin`. | M |
| FR-A13 | "View site" link in the admin sidebar opens the public site in a new tab. | S |
| FR-A14 | Up/down buttons to change display order in lists. | S |

---

## 5. Data and Content

All text and data live in **`docs/CONTENT.md`** (single source of truth). This PRD does not repeat them.

| Section | Where it lives at runtime | Editable in admin |
|---|---|---|
| Profile, links, bio | `StaticContentProvider` (code) | No |
| Education | `StaticContentProvider` (code) | No |
| Certifications | `StaticContentProvider` (code) | No |
| Achievements | `StaticContentProvider` (code) | No |
| **Skills** | SQL Server | **Yes** |
| **Projects** | SQL Server | **Yes** |
| **Experience** | SQL Server | **Yes** |

- Initial Skills, Projects and Experience rows are **seeded** from `docs/CONTENT.md` so the site looks identical after migration.
- Values that are `null` in `CONTENT.md` stay null and their UI elements are hidden.

---

## 6. Contact Strategy

No contact form. Visitors use the `mailto:` link, phone number, GitHub and LinkedIn. Reason: a form needs email infrastructure (SMTP or third party), secrets and spam protection for little benefit on a portfolio. Revisit under Future Scope.

---

## 7. Non-Functional Requirements

| Area | Requirement |
|---|---|
| **Performance** | Public page loads in under 2 s on 4G. Compressed images, minified CSS/JS for production, static file caching. Lighthouse Performance 90+. Database queries on the public page must be read-only (`AsNoTracking`) and not repeated per section. |
| **Responsiveness** | Bootstrap breakpoints 576 / 768 / 992 / 1200. No horizontal scroll from 320 px. Admin works on tablet and phone. |
| **SEO** | Unique title and meta description, Open Graph tags, semantic HTML, `robots.txt`, `sitemap.xml` (public pages only). |
| **Accessibility** | Alt text, WCAG AA contrast, keyboard navigation, aria-labels. Lighthouse Accessibility 90+. |
| **Browser support** | Latest two versions of Chrome, Edge, Firefox, Safari. |
| **Security (public)** | HTTPS only, HSTS in production, `rel="noopener noreferrer"`, no secrets in source. |
| **Security (admin)** | See list below. |
| **Data integrity** | Validation in the Application layer, plus database constraints (lengths, required, unique, check constraints). |
| **Maintainability** | Clean Architecture dependency rule enforced by project references. Content in database or `StaticContentProvider`, never hard-coded in `.cshtml`. |
| **Backup** | Owner takes a database backup before each production migration. |

**Admin security requirements**
- ASP.NET Core Identity, cookie auth: `HttpOnly`, `Secure`, `SameSite=Lax`, 8-hour sliding expiration.
- One admin account seeded from configuration (user-secrets locally, environment variables in production). Password is never in source control.
- Password policy: minimum 10 characters, with upper, lower and digit.
- Lockout: 5 failed attempts, 15-minute lockout.
- Login endpoint rate-limited (built-in ASP.NET Core rate limiter), 10 requests per minute per IP.
- Anti-forgery token validated on every POST (global filter).
- All state changes use POST; no state change on GET.
- Return URL after login must be a local URL.
- Razor output encoding only. `Html.Raw` is never used on database content.
- Security headers: `X-Content-Type-Options: nosniff`, `X-Frame-Options: DENY` (admin), `Referrer-Policy: strict-origin-when-cross-origin`.
- Generic login error message ("Invalid email or password"); never reveal which one is wrong.

---

## 8. Technical Architecture

### 8.1 Technology and Packages

| Layer / use | Package | Notes |
|---|---|---|
| Framework | ASP.NET Core MVC, .NET 10 | |
| CQRS | **MediatR 12.5.0 (exact, pinned)** | 12.5.0 is the last Apache-2.0 release. Do **not** upgrade to 13+ (licence key required). |
| ORM | Microsoft.EntityFrameworkCore.SqlServer 10.x, .Design 10.x | |
| Identity | Microsoft.AspNetCore.Identity.EntityFrameworkCore 10.x | No Identity UI package. |
| Validation | **Own validators** (no package) | See 8.3. |
| Mapping | **Manual mapping** | No AutoMapper (commercial licence). |
| Tests | xUnit, Microsoft.EntityFrameworkCore.InMemory (or SQLite in-memory) | No FluentAssertions, no Moq. Use plain xUnit asserts. |
| UI | Bootstrap 5, Bootstrap Icons | |

Any other package requires owner approval.

### 8.2 Solution Layers and Dependency Rule

```text
Web  ──►  Application  ──►  Domain
 │             ▲
 └──►  Infrastructure ──┘
```

| Project | Depends on | Contains |
|---|---|---|
| `PortfolioWebsite.Domain` | nothing | Entities |
| `PortfolioWebsite.Application` | Domain | CQRS commands/queries/handlers, DTOs, validators, interfaces, behaviours |
| `PortfolioWebsite.Infrastructure` | Application, Domain | EF Core DbContext, configurations, migrations, seeder, Identity setup |
| `PortfolioWebsite.Web` | Application, Infrastructure | MVC controllers, views, Admin area, static content provider, composition root (`Program.cs`) |
| `PortfolioWebsite.Application.Tests` | Application, Domain, Infrastructure | Unit tests |

**Rules**
- Domain references nothing.
- Application never references Infrastructure or Web. It may reference `Microsoft.EntityFrameworkCore` (abstractions only, for `DbSet<T>`), never `SqlServer`.
- Web references Infrastructure **only** to call `AddInfrastructure()` in `Program.cs`. Controllers never use `DbContext` directly. They send commands and queries through `ISender` (MediatR).
- Application handlers use `IApplicationDbContext` (no repository classes; the DbContext already acts as repository and unit of work).

### 8.3 CQRS Conventions

- One folder per use case: `Features/<Area>/Commands/<UseCase>/` and `Features/<Area>/Queries/<UseCase>/`.
- Each use case has: `<UseCase>Command.cs` (or `Query.cs`), `<UseCase>Handler.cs`, and for commands `<UseCase>Validator.cs`.
- Commands change state and return an `int` id or nothing. Queries never change state and return DTOs (records).
- **Validation:** Application defines `IRequestValidator<TRequest>` with `IEnumerable<ValidationError> Validate(TRequest request)`. A `ValidationBehaviour<TRequest,TResponse>` (MediatR pipeline) runs all registered validators and throws `ValidationException` (own class, holds `Dictionary<string, string[]>`). Web catches it and adds errors to `ModelState`. Rules requiring database access (for example unique name) are checked inside the handler and throw the same exception.
- `NotFoundException` is thrown when an id does not exist; Web maps it to the 404 page.
- Handlers use `AsNoTracking()` for queries.
- Allowed icon names live in `Application/Common/Constants/AllowedIcons.cs`.

### 8.4 Folder Structure

```text
PortfolioWebsite/
├── PortfolioWebsite.sln
├── AGENTS.md
├── README.md
├── docs/                              # PRD.md, CONTENT.md, DESIGN.md, TASKS.md
├── src/
│   ├── PortfolioWebsite.Domain/
│   │   ├── Common/BaseEntity.cs
│   │   └── Entities/                  # SkillCategory, Skill, Project, Experience, ExperiencePoint
│   ├── PortfolioWebsite.Application/
│   │   ├── Common/
│   │   │   ├── Behaviours/            # ValidationBehaviour
│   │   │   ├── Constants/             # AllowedIcons
│   │   │   ├── Exceptions/            # ValidationException, NotFoundException
│   │   │   └── Interfaces/            # IApplicationDbContext, IRequestValidator<T>
│   │   ├── Features/
│   │   │   ├── Skills/{Commands,Queries}/
│   │   │   ├── Projects/{Commands,Queries}/
│   │   │   ├── Experiences/{Commands,Queries}/
│   │   │   └── Dashboard/Queries/GetDashboardSummary/
│   │   └── DependencyInjection.cs     # AddApplication()
│   ├── PortfolioWebsite.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── ApplicationDbContext.cs    # IdentityDbContext + IApplicationDbContext
│   │   │   ├── Configurations/            # IEntityTypeConfiguration<T> per entity
│   │   │   ├── Migrations/
│   │   │   └── Seed/DbSeeder.cs
│   │   └── DependencyInjection.cs         # AddInfrastructure(config)
│   └── PortfolioWebsite.Web/
│       ├── Areas/Admin/{Controllers,Models,Views}/
│       ├── Controllers/HomeController.cs
│       ├── Models/                    # public view models (existing)
│       ├── Services/                  # IPortfolioService, PortfolioService, IStaticContentProvider, StaticContentProvider
│       ├── Views/
│       ├── wwwroot/                   # css (site.css, admin.css), js, images, files
│       └── Program.cs
└── tests/
    └── PortfolioWebsite.Application.Tests/
```

### 8.5 Request Flows

**Public page:** `HomeController.Index` → `IPortfolioService.GetPortfolioAsync()` → `PortfolioService` combines `IStaticContentProvider` (profile, education, certifications, achievements) with three MediatR queries (`GetPublicSkillsQuery`, `GetPublicProjectsQuery`, `GetPublicExperiencesQuery`) → maps DTOs to the existing public view models → `Index.cshtml`.

**Admin write:** Admin controller action (POST) → binds an admin form model → maps to a Command → `ISender.Send` → `ValidationBehaviour` → Handler → `IApplicationDbContext.SaveChangesAsync` → redirect with flash message. On `ValidationException`, errors go to `ModelState` and the form is shown again.

### 8.6 Composition Root (`Program.cs`)

```csharp
builder.Services.AddApplication();                       // MediatR, validators, behaviours
builder.Services.AddInfrastructure(builder.Configuration); // DbContext, Identity, seeder
builder.Services.AddSingleton<IStaticContentProvider, StaticContentProvider>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddControllersWithViews(o => o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
// authentication cookie, rate limiter, security headers: see Phases 7 and 9 in docs/TASKS.md
```

### 8.7 Authentication and Authorization
- Identity (`IdentityUser`) stored in the same database. Single user, no roles.
- Admin area base controller has `[Authorize]` and `[Area("Admin")]`. Public controllers stay anonymous.
- Routes: `/admin` (dashboard), `/admin/login`, `/admin/skills`, `/admin/projects`, `/admin/experiences`.

### 8.8 Configuration and Secrets

| Key | Local development | Production |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `appsettings.Development.json` (LocalDB or SQL Express, no password) | Host environment variable or untracked `appsettings.Production.json` |
| `Admin:Email` | user-secrets | Environment variable `Admin__Email` |
| `Admin:Password` | user-secrets | Environment variable `Admin__Password` (needed only for first start) |

Never commit secrets. `appsettings.Production.json` is in `.gitignore`.

---

## 9. Database Design

SQL Server. Primary keys are `int IDENTITY`. All timestamps are UTC (`datetime2`).

### 9.1 Tables

**SkillCategories**

| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Name | nvarchar(100) | NOT NULL, UNIQUE |
| IconClass | nvarchar(50) | NOT NULL (one of allowed `bi-*` icons) |
| DisplayOrder | int | NOT NULL, default 0 |
| CreatedAtUtc, UpdatedAtUtc | datetime2 | NOT NULL |

**Skills**

| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| SkillCategoryId | int | FK to SkillCategories(Id), **ON DELETE CASCADE** |
| Name | nvarchar(60) | NOT NULL |
| DisplayOrder | int | NOT NULL, default 0 |
| (SkillCategoryId, Name) | | UNIQUE |

**Projects**

| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Title | nvarchar(150) | NOT NULL |
| Description | nvarchar(1000) | NOT NULL |
| TechStack | nvarchar(300) | NOT NULL (comma-separated, shown as chips) |
| Architecture | nvarchar(300) | NULL |
| LiveDemoUrl | nvarchar(500) | NULL |
| GitHubUrl | nvarchar(500) | NULL |
| DisplayOrder | int | NOT NULL, default 0 |
| IsPublished | bit | NOT NULL, default 1 |
| CreatedAtUtc, UpdatedAtUtc | datetime2 | NOT NULL |

**Experiences**

| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| Role | nvarchar(150) | NOT NULL |
| Organisation | nvarchar(150) | NOT NULL |
| StartDate | date | NOT NULL (first day of the month) |
| EndDate | date | NULL (first day of the month) |
| IsCurrent | bit | NOT NULL, default 0 |
| IsPublished | bit | NOT NULL, default 1 |
| CreatedAtUtc, UpdatedAtUtc | datetime2 | NOT NULL |
| CHECK | | `EndDate IS NULL OR EndDate >= StartDate` and `IsCurrent = 0 OR EndDate IS NULL` |

**ExperiencePoints**

| Column | Type | Constraints |
|---|---|---|
| Id | int | PK, identity |
| ExperienceId | int | FK to Experiences(Id), **ON DELETE CASCADE** |
| Text | nvarchar(500) | NOT NULL |
| DisplayOrder | int | NOT NULL, default 0 |

**Identity tables** (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, and so on) are created by the Identity migration. Only `AspNetUsers` is used.

### 9.2 Ordering Rules (public site)
- Skill categories by `DisplayOrder`, skills inside by `DisplayOrder`.
- Projects (published only) by `DisplayOrder`.
- Experiences (published only): current roles first, then `StartDate` descending. Points by `DisplayOrder`.
- Date text is formatted as `MMM yyyy`, and `Present` for current roles (example: `Aug 2023 - Present`).

### 9.3 Migrations and Seeding
- Migrations live in `PortfolioWebsite.Infrastructure/Persistence/Migrations`. Never edit a migration that has been applied. Add a new one.
- Commands (from the repo root):
  ```bash
  dotnet ef migrations add <Name> -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
  dotnet ef database update -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
  ```
- `dotnet ef` loads Production configuration by default; the trailing `-- --environment Development` makes it read `appsettings.Development.json` and user-secrets. The Web project must reference `Microsoft.EntityFrameworkCore.Design` (PrivateAssets=all) because it is the startup project.
- `DbSeeder` runs at startup:
  - Seeds skill categories, projects and experiences from `docs/CONTENT.md` **only if the tables are empty**.
  - Creates the admin user from `Admin:Email` and `Admin:Password` **only if no user exists**.
- Development: the app may auto-apply migrations at startup. **Production: never auto-migrate.** Generate an idempotent SQL script (`dotnet ef migrations script --idempotent`) and run it on the production database after a backup.

---

## 10. Validation Rules (Application layer)

| Entity | Rule |
|---|---|
| **SkillCategory** | Name required, 1-100 chars, unique (case-insensitive). Icon must be in `AllowedIcons`. DisplayOrder >= 0. Skills: 1-30 items; each trimmed, 1-60 chars, unique inside the category (case-insensitive). Blank lines are ignored. |
| **Project** | Title 1-150. Description 1-1000. TechStack 1-300. Architecture max 300 (optional). LiveDemoUrl and GitHubUrl optional, max 500, must be absolute `http` or `https` URLs. DisplayOrder >= 0. |
| **Experience** | Role 1-150. Organisation 1-150. StartDate required. If `IsCurrent` is true, EndDate must be empty. If `IsCurrent` is false, EndDate is required and must be on or after StartDate. Points: 0-10 items, each 1-500 chars; blank lines ignored. |

`AllowedIcons`: `bi-code-slash`, `bi-diagram-3`, `bi-database`, `bi-phone`, `bi-tools`, `bi-puzzle`, `bi-cpu`, `bi-globe`, `bi-cloud`, `bi-shield-check`.

All text inputs are trimmed before validation and saving.

---

## 11. Future Scope

- Caching of the public page with invalidation after admin changes.
- Admin editing for Profile, Education, Certifications, Achievements.
- Image upload for projects (screenshots).
- Drag-and-drop reordering.
- Two-factor authentication, audit log.
- Contact form (Formspree or MailKit).
- Dark/light toggle, visit analytics, blog.

---

## 12. Deployment Plan

Deployment is the **last phase**, after the admin panel and SEO work.

- **Hosting:** [TBD: provider that offers ASP.NET Core and SQL Server, for example Somee, SmarterASP.NET, or Azure App Service + Azure SQL]
- **Database:** production SQL Server created by the owner; connection string as environment variable.
- **Migrations:** idempotent script applied manually after a backup.
- **First start:** `Admin__Email` and `Admin__Password` set so the seeder creates the admin. Change or remove the password variable afterwards.
- **Publish:** `dotnet publish src/PortfolioWebsite.Web -c Release`.
- **Source control:** GitHub, `main` is stable. Admin work happens on branch `feature/admin` and is merged after Phase 9.
- **CI (optional):** GitHub Actions workflow that builds and runs tests on every push.
- **Domain / HTTPS:** [TBD]. HTTPS is mandatory (cookies are `Secure`).

---

## 13. Acceptance Criteria and Milestones

### 13.1 Acceptance Criteria

| ID | Criterion | Phase |
|---|---|---|
| AC-01 | All sections render from the services/database; no content hard-coded in views. | 3, 6 |
| AC-02 | Navbar scrolls smoothly and highlights the active section. | 3 |
| AC-03 | Layout does not break at 320 / 768 / 1024 / 1440 px. | 3 |
| AC-04 | External links open in a new tab; Live Demo opens MaleFashion. | 3 |
| AC-05 | CV button downloads the PDF; GitHub and LinkedIn open the correct profiles. | 3 |
| AC-06 | Lighthouse 90+ in Performance, Accessibility, Best Practices, SEO (public page). | 10 |
| AC-07 | Title, meta description, Open Graph tags present. | 10 |
| AC-08 | Site publicly reachable over HTTPS; works on latest Chrome, Edge, Firefox, Safari. | 11 |
| AC-09 | No `[TBD]` or placeholder left in final content. | 11 |
| AC-10 | Solution follows the dependency rule of Section 8.2 (checked via project references). The public site looks the same as before the restructure. | 4 |
| AC-11 | Skills, Projects, Experience on the public site come from SQL Server. Unpublished projects and experiences are not shown. Empty sections are hidden. | 6 |
| AC-12 | `/admin` and all admin pages require login; anonymous users are redirected. | 7 |
| AC-13 | Wrong credentials are rejected with a generic message; account locks after 5 failed attempts. | 7 |
| AC-14 | Admin can create, edit and delete skill categories and skills; changes appear on the public site immediately. | 8 |
| AC-15 | Admin can create, edit, delete and publish/unpublish projects; changes appear immediately. | 8 |
| AC-16 | Admin can create, edit, delete experiences, including a current role; changes appear immediately. | 8 |
| AC-17 | Invalid input shows field errors and nothing is saved. | 8 |
| AC-18 | Every POST requires an anti-forgery token; delete requires confirmation; no state change on GET. | 8, 9 |
| AC-19 | No secret (password, connection string for production) is in the repository. | 9 |
| AC-20 | Admin pages are `noindex`, excluded from `sitemap.xml`, disallowed in `robots.txt`. | 10 |
| AC-21 | `dotnet test` passes (validators and handlers). | 9 |

### 13.2 Milestones

| Phase | Deliverable |
|---|---|
| 1-3 (done) | Static site: layout, models, service, all sections, design |
| 4 | Clean Architecture restructure (no behaviour change) |
| 5 | Domain and Application layers (CQRS) |
| 6 | Infrastructure: SQL Server, migrations, seeding; public site reads from DB |
| 7 | Identity, login, admin layout and dashboard |
| 8 | Admin CRUD for Skills, Projects, Experience |
| 9 | Security hardening, tests, merge |
| 10 | SEO, accessibility, performance |
| 11 | Deployment and final review |

---

## 14. Open Items (owner action required)

- Correct certificate link for "Full Stack ASP.NET Core MVC Web Development".
- GitHub repository URLs for MaleFashion and Blood Donation.
- Education period / expected graduation.
- Admin email and a strong password (stored in user-secrets, never in files).
- SQL Server for local development (LocalDB or SQL Express) and `dotnet-ef` tool installed.
- Hosting provider, production SQL Server and domain.
