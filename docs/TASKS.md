# TASKS.md

Phased implementation checklist for the Portfolio Website (v2.0: dynamic content + admin panel).

**How to use (for agents):**
- Work on **one phase at a time**. Stop at the end of each phase and wait for the owner.
- Tick `[x]` when a task is done and verified.
- References like `PRD §8.2` point to sections of `docs/PRD.md`. AC-xx are acceptance criteria from `PRD §13.1`.
- All content comes from `docs/CONTENT.md`. All visuals come from `docs/DESIGN.md` (admin panel: Section 14).
- Rules are in `AGENTS.md`. Never invent `null` / `[TBD]` content. Never commit secrets.
- Phases 4 to 9 are done on the Git branch `feature/admin`.

**Status legend:** `[ ]` todo, `[x]` done, `[!]` blocked (needs owner input)

**Phase overview**

| Phase | Name | Status |
|---|---|---|
| 1 | Project setup and layout | Done |
| 2 | Models, service and data (static) | Done |
| 3 | Sections, styling and interactions | Done |
| 4 | Clean Architecture restructure | Todo |
| 5 | Domain and Application layers (CQRS) | Todo |
| 6 | Infrastructure: SQL Server, migrations, seeding, public site from DB | Todo |
| 7 | Identity, login, admin layout and dashboard | Todo |
| 8 | Admin CRUD: Skills, Projects, Experience | Todo |
| 9 | Security hardening, tests, merge | Todo |
| 10 | SEO, accessibility, performance (was Phase 4) | Todo |
| 11 | Deployment and final review (was Phase 5) | Todo |

---

## Before You Start (owner checklist)

These are outside the agent's control. The agent should only report if they are missing.

**Done already**
- [x] .NET 10 SDK installed
- [x] Repo folder with `AGENTS.md` and `docs/`
- [x] `profile.jpg` and CV copied into `wwwroot/`
- [x] `docs/DESIGN.md` created

**Before Phase 4**
- [ ] Commit everything on `main` (clean working tree) so the restructure can be rolled back
- [ ] Create branch: `git checkout -b feature/admin`

**Before Phase 6** (guide: `docs/SETUP.md`)
- [ ] SQL Server for development installed (LocalDB with Visual Studio, or SQL Server Express)
- [ ] `dotnet-ef` tool installed: `dotnet tool install --global dotnet-ef`

**Before Phase 7** (the agent cannot know these)
- [ ] Choose the admin email and a strong password (10+ characters, upper, lower, digit)
- [ ] Store them in user-secrets (full guide in `docs/SETUP.md` Section 4):
  ```bash
  dotnet user-secrets init
  dotnet user-secrets set "Admin:Email" "your-email@example.com"
  dotnet user-secrets set "Admin:Password" "YourStrongPassword1"
  ```

**Any time before Phase 11**
- [ ] Correct certificate link for "Full Stack ASP.NET Core MVC Web Development"
- [ ] GitHub repository URLs for MaleFashion and Blood Donation (or leave `null`)
- [ ] Education period / expected graduation
- [ ] Hosting provider (with SQL Server), production database and domain decision
- [ ] Update the matching `null` values in `docs/CONTENT.md` (or enter them later through the admin panel for Projects)

> Missing items must be hidden in the UI, not faked.

---

## Phase 1 - Project Setup and Layout (DONE)

**Goal:** empty page runs with navbar and footer. DI is wired.

- [x] Create project **in the repo root** (not in a sub-folder): `dotnet new mvc -n PortfolioWebsite -o . --framework net10.0`. Do not overwrite `AGENTS.md` or `docs/`.
- [x] Confirm `PortfolioWebsite.csproj` has `<TargetFramework>net10.0</TargetFramework>`
- [x] Add `.gitignore` for .NET (`bin/`, `obj/`, `.vs/`) and initialise Git
- [x] Confirm `AGENTS.md` and `docs/` files are still in place and untouched
- [x] Create folders: `Models/`, `Services/`, `wwwroot/images/`, `wwwroot/files/` (keep empty; the owner adds the assets)
- [x] Remove default template content (Privacy page, sample text, default styles not needed)
- [x] Ensure Bootstrap 5 is available; add Bootstrap Icons (PRD §8.1)
- [x] Build `Views/Shared/_Layout.cshtml`: meta viewport, navbar (FR-01), `<main>`, footer (FR-14)
- [x] Navbar links for all sections: Home, About, Education, Skills, Projects, Certifications, Experience, Achievements, Contact
- [x] Create `wwwroot/css/site.css` with CSS variables (colours, spacing) and `wwwroot/js/site.js` (empty for now)
- [x] Register in `Program.cs`: `AddControllersWithViews()` and (after Phase 2) the portfolio service

**Verify**
- [x] `dotnet build` passes
- [x] `dotnet run` shows layout with navbar and footer, no console errors
- [x] Navbar collapses to hamburger on a narrow window

---

## Phase 2 - Models, Service and Data (DONE)

**Goal:** controller returns a full `PortfolioViewModel` from mock data.

- [x] Create `ProfileViewModel` (name, designation, location, phone, email, bio, GitHub URL, LinkedIn URL, photo path, CV path)
- [x] Create `EducationViewModel` (degree, institution, period nullable)
- [x] Create `SkillViewModel` (category, list of skills)
- [x] Create `ProjectViewModel` (title, description, tech stack, architecture, nullable `LiveDemoUrl`, nullable `GitHubUrl`)
- [x] Create `CertificationViewModel` (course name, platform, nullable verification URL)
- [x] Create `ExperienceViewModel` (role, organisation, date range, bullet points)
- [x] Create `AchievementViewModel` (title, year)
- [x] Create `PortfolioViewModel` aggregating all of the above
- [x] Create `IPortfolioService` with `PortfolioViewModel GetPortfolio()`
- [x] Implement `MockPortfolioService` with data copied **exactly** from `docs/CONTENT.md` sections 1 to 7 (and labels from sections 8 to 9)
  - [x] Profile and links (CONTENT §1)
  - [x] Education (CONTENT §2), period left null
  - [x] Skills, 6 categories (CONTENT §3)
  - [x] Projects (CONTENT §4), MaleFashion live demo set, GitHub URLs left null
  - [x] Certifications (CONTENT §5), second certificate URL left null
  - [x] Experience (CONTENT §6)
  - [x] Achievements (CONTENT §7)
- [x] Register `AddSingleton<IPortfolioService, MockPortfolioService>()` in `Program.cs`
- [x] Update `HomeController` to inject the service and pass `PortfolioViewModel` to `Index`

**Verify**
- [x] `dotnet build` passes
- [x] Temporarily render a few model values on `Index` to confirm data flows (remove afterwards)
- [x] Data matches `docs/CONTENT.md` word for word; nothing invented

---

## Phase 3 - Sections, Styling and Interactions (DONE)

**Goal:** all sections render from the model. AC-01 to AC-05 pass.

**Gate (check first, stop if any is missing and ask the owner):**
- [x] `docs/DESIGN.md` exists
- [x] `wwwroot/images/profile.jpg` exists
- [x] `wwwroot/files/ReadHasan_CV.pdf` exists (if not, continue and hide the CV button)

- [x] `_Hero.cshtml`: photo, name, designation, tagline, buttons: Download CV, GitHub, LinkedIn (FR-03, FR-13)
- [x] `_About.cshtml`: bio and location (FR-04)
- [x] Education section partial (FR-05)
- [x] `_Skills.cshtml`: cards or badges grouped by category (FR-06)
- [x] `_Projects.cshtml`: one card per project; Live Demo and GitHub buttons only if the URL exists (FR-07)
- [x] `_Certifications.cshtml`: list with "View Certificate" only if the URL exists (FR-09)
- [x] `_Experience.cshtml`: timeline with date range and bullets (FR-10)
- [x] `_Achievements.cshtml`: IUPC list with year (FR-11)
- [x] `_Contact.cshtml`: `mailto:` link, phone, location, GitHub and LinkedIn icons; no form (FR-12, PRD §6)
- [x] Compose all partials in `Views/Home/Index.cshtml` in the order of PRD §4; each section has an `id`
- [x] `site.js`: smooth scroll to sections (FR-02), active navbar link on scroll, back-to-top button (FR-15)
- [x] All external links open in a new tab with `rel="noopener noreferrer"` (FR-08)
- [x] Graceful fallback if photo or CV file is missing
- [x] Responsive polish at 320, 768, 1024, 1440 px (no horizontal scroll)
- [x] Apply colours, fonts and style from `docs/DESIGN.md` through CSS variables in `site.css`

**Verify**
- [x] AC-01: all sections render from `MockPortfolioService`; no hard-coded content in views
- [x] AC-02: navbar scrolls smoothly and highlights the active section
- [x] AC-03: layout does not break at 320 / 768 / 1024 / 1440 px
- [x] AC-04: external links open in new tab; Live Demo opens MaleFashion
- [x] AC-05: CV button downloads the PDF; GitHub and LinkedIn open the correct profiles

---

---

## Phase 4 - Clean Architecture Restructure

**Goal:** the same site, now in a multi-project solution. **No behaviour or visual change.** (AC-10)

- [ ] Confirm working tree is clean and you are on branch `feature/admin`
- [x] Create solution file at the repo root: `PortfolioWebsite.sln`
- [x] Create folders `src/` and `tests/`
- [x] Create class libraries (`net10.0`): `src/PortfolioWebsite.Domain`, `src/PortfolioWebsite.Application`, `src/PortfolioWebsite.Infrastructure`
- [x] Move the existing web project into `src/PortfolioWebsite.Web/` using `git mv` (keep history). Include `Controllers/`, `Models/`, `Services/`, `Views/`, `wwwroot/` (with `profile.jpg` and CV), `Program.cs`, `appsettings*.json`, `Properties/`
- [x] Rename `PortfolioWebsite.csproj` to `PortfolioWebsite.Web.csproj`; set root namespace to `PortfolioWebsite.Web` and update all namespaces and `using` statements
- [x] Add all projects to the solution (`dotnet sln add`)
- [x] Set project references exactly as in `PRD §8.2`:
  - Application → Domain
  - Infrastructure → Application, Domain
  - Web → Application, Infrastructure
- [x] Remove default placeholder `Class1.cs` files
- [x] Update `.gitignore` for the new layout; add `appsettings.Production.json` and `*.user`
- [x] Update `README`/docs paths if any mention the old layout (do not change content of PRD sections)
- [x] Confirm `AGENTS.md` commands work (`dotnet build PortfolioWebsite.sln`, `dotnet run --project src/PortfolioWebsite.Web`)

**Verify**
- [x] `dotnet build PortfolioWebsite.sln` passes with no warnings you introduced
- [x] `dotnet run --project src/PortfolioWebsite.Web` shows the site; photo, CV download and all sections work
- [ ] Owner compares the site with the version before the restructure: it is identical
- [x] Domain has no project references; Application references only Domain

---

## Phase 5 - Domain and Application Layers (CQRS)

**Goal:** all business logic for Skills, Projects, Experience exists as commands and queries, independent of the database. (PRD §8.3, §10)

**Domain** (`src/PortfolioWebsite.Domain`)
- [x] `Common/BaseEntity` (`Id`, `CreatedAtUtc`, `UpdatedAtUtc`)
- [x] Entities per `PRD §9.1`: `SkillCategory`, `Skill`, `Project`, `Experience`, `ExperiencePoint` (with navigation collections initialised)

**Application setup** (`src/PortfolioWebsite.Application`)
- [x] Add package `MediatR` exactly `12.5.0` (pin the version, do not use a range)
- [x] Add package `Microsoft.EntityFrameworkCore` 10.x (abstractions only)
- [x] `Common/Interfaces/IApplicationDbContext` (`DbSet<>` for all five entities, `SaveChangesAsync`)
- [x] `Common/Interfaces/IRequestValidator<TRequest>` and `ValidationError`
- [x] `Common/Exceptions/ValidationException` (holds `Dictionary<string,string[]>`) and `NotFoundException`
- [x] `Common/Behaviours/ValidationBehaviour<TRequest,TResponse>` (MediatR pipeline)
- [x] `Common/Constants/AllowedIcons` (list from `PRD §10`)
- [x] `DependencyInjection.AddApplication()` registers MediatR, the behaviour and all validators from the assembly

**Skills** (`Features/Skills`)
- [x] Commands: `CreateSkillCategory`, `UpdateSkillCategory`, `DeleteSkillCategory` (each with handler and validator)
- [x] Queries: `GetSkillCategories` (admin list), `GetSkillCategoryById`, `GetPublicSkills` (ordered, public DTO)
- [x] Update replaces the skill items of the category (skills come as a list of names; blank lines ignored; trimmed)

**Projects** (`Features/Projects`)
- [x] Commands: `CreateProject`, `UpdateProject`, `DeleteProject`, `SetProjectPublished` (each with handler; validators for create/update)
- [x] Queries: `GetProjects` (admin list), `GetProjectById`, `GetPublicProjects` (published only, by `DisplayOrder`)

**Experiences** (`Features/Experiences`)
- [x] Commands: `CreateExperience`, `UpdateExperience`, `DeleteExperience`, `SetExperiencePublished`
- [x] Queries: `GetExperiences` (admin list), `GetExperienceById`, `GetPublicExperiences` (published only; current first, then `StartDate` descending; date text formatted `MMM yyyy`, `Present` for current)

**Dashboard**
- [x] Query `GetDashboardSummary` (counts of categories, projects, experiences)

**Rules**
- [x] Validation rules exactly as in `PRD §10`; unique category name and unique skills per category are enforced in the handler
- [x] All queries use `AsNoTracking()`; timestamps are set in the handlers or DbContext (UTC)

**Verify**
- [x] `dotnet build PortfolioWebsite.sln` passes
- [x] Application has no reference to Infrastructure, Web or SqlServer
- [x] MediatR version in the csproj is exactly `12.5.0`

---

## Phase 6 - Infrastructure: SQL Server, Migrations, Seeding

**Goal:** data lives in SQL Server; the public site reads Skills, Projects and Experience from it and looks identical to before. (AC-01, AC-11)

**Gate (stop and ask if missing):** SQL Server (LocalDB or Express) available and `dotnet ef` installed.

**Infrastructure** (`src/PortfolioWebsite.Infrastructure`)
- [x] Add packages: `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Design` (PrivateAssets=all), `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (all 10.x)
- [x] `Persistence/ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationDbContext`; set `CreatedAtUtc`/`UpdatedAtUtc` automatically in `SaveChangesAsync`
- [x] `Persistence/Configurations/*` (`IEntityTypeConfiguration<T>`): max lengths, required, unique indexes, FKs with cascade delete, defaults and check constraints exactly as `PRD §9.1`
- [x] `DependencyInjection.AddInfrastructure(IConfiguration)`: DbContext with SQL Server using `ConnectionStrings:DefaultConnection`, registers `IApplicationDbContext`
- [x] Migration `InitialCreate` (portfolio tables + Identity tables)
- [x] `Persistence/Seed/DbSeeder`:
  - [x] Seeds skill categories, skills, projects, experiences from `docs/CONTENT.md` **only when tables are empty**
  - [x] Does not create the admin user yet (Phase 7)

**Web** (`src/PortfolioWebsite.Web`)
- [x] Add `ConnectionStrings:DefaultConnection` to `appsettings.Development.json` (SQL Express, JSON-escaped: `Server=.\\SQLEXPRESS;Database=PortfolioDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True`; see `docs/SETUP.md` Section 3). Never put a password in this file
- [x] `Program.cs`: call `AddApplication()` and `AddInfrastructure(...)`; in **Development only** apply migrations and run the seeder at startup
- [x] Rename `MockPortfolioService` to `StaticContentProvider` (interface `IStaticContentProvider`). Keep **only** Profile, Education, Certifications, Achievements in it. Remove Skills, Projects, Experience from it
- [x] Change `IPortfolioService` to `Task<PortfolioViewModel> GetPortfolioAsync()`; implement `PortfolioService` (static parts from the provider, dynamic parts via `GetPublicSkills/Projects/Experiences` queries; manual mapping DTO to existing view models)
- [x] `HomeController.Index` becomes async and uses `IPortfolioService`
- [x] Hide a dynamic section (and its navbar link) when it has no items (FR-17)
- [x] Register services in `Program.cs` as in `PRD §8.6`

**Verify**
- [x] `dotnet ef database update` creates the database (apply to local only)
- [x] Database contains the seeded rows; seeding twice does not duplicate them
- [ ] Public site looks identical to before for Skills, Projects, Experience (owner check)
- [x] Setting a project `IsPublished = 0` in the database hides it on the site; emptying a table hides the section
- [x] Application and Domain still have no reference to SqlServer or Web

---

## Phase 7 - Identity, Login, Admin Layout and Dashboard

**Goal:** a secure admin area with login, logout and a dashboard. (AC-12, AC-13)

**Gate:** `Admin:Email` and `Admin:Password` exist in user-secrets (see owner checklist). If missing, stop and ask.

- [x] Configure Identity (`AddIdentity<IdentityUser, IdentityRole>` + EF stores, no Identity UI package): password min 10 with upper, lower, digit; lockout 5 attempts / 15 minutes; unique email
- [x] Cookie settings: `HttpOnly`, `Secure`, `SameSite=Lax`, 8-hour sliding expiration, login path `/admin/login`
- [x] Extend `DbSeeder`: create the admin user from `Admin:Email` / `Admin:Password` **only if no user exists**; never log the password
- [x] Add global `AutoValidateAntiforgeryTokenAttribute`
- [x] Add built-in rate limiter: login POST max 10 requests per minute per IP
- [x] Admin area: `Areas/Admin` with base controller `[Area("Admin")] [Authorize]`
- [x] `AccountController`: `GET/POST /admin/login` (generic error "Invalid email or password"; local return URL only), `POST /admin/logout`
- [x] `DashboardController` at `/admin`: counts via `GetDashboardSummary`, quick links to Skills, Projects, Experience
- [x] Admin layout `_AdminLayout.cshtml` with sidebar, topbar, flash message area (TempData), per `DESIGN.md §14`; `wwwroot/css/admin.css` and `wwwroot/js/admin.js`
- [x] Admin pages include `<meta name="robots" content="noindex, nofollow">`
- [x] Sidebar: Dashboard, Skills, Projects, Experience, View Site (new tab), Logout. Skills/Projects/Experience links may show a "coming soon" page until Phase 8
- [x] No link to `/admin` anywhere on the public site

**Verify**
- [x] AC-12: opening `/admin` while logged out redirects to `/admin/login`
- [ ] AC-13: wrong password shows the generic error; 5 wrong attempts lock the account
- [ ] Correct login reaches the dashboard; logout returns to login and `/admin` is protected again
- [x] A POST without anti-forgery token is rejected
- [x] Admin password is not present in any tracked file (`git grep`)
- [ ] Admin layout is usable at 360 px and 1440 px

---

## Phase 8 - Admin CRUD: Skills, Projects, Experience

**Goal:** the owner manages the three sections from the admin panel. (AC-14 to AC-18)

**Common (all three sections)**
- [x] List page: table in a card, "Add new" button, Edit and Delete actions, empty state (per `DESIGN.md §14`)
- [x] Create and Edit forms with server-side validation; show errors next to fields; keep entered values on error
- [x] Controllers map `ValidationException` to `ModelState`, `NotFoundException` to the 404 page
- [x] Delete is a POST action with a confirmation modal (`DESIGN.md §14`)
- [x] Flash messages after create, update, delete
- [x] Changes appear on the public site immediately (no caching)
- [x] Admin form models live in `Areas/Admin/Models` and map **manually** to commands and DTOs

**Skills** (`/admin/skills`)
- [x] List shows name, icon, number of skills, display order
- [x] Form: name, icon (dropdown of `AllowedIcons`), display order, skills textarea (one per line)
- [x] Edit pre-fills the skills textarea from existing items

**Projects** (`/admin/projects`)
- [x] List shows title, display order, status badge (Published / Draft)
- [x] Form: title, description, tech stack (comma-separated), architecture, live demo URL, GitHub URL, display order, published checkbox
- [x] Quick publish/unpublish toggle in the list (POST)

**Experiences** (`/admin/experiences`)
- [x] List shows role, organisation, date range text, status badge
- [x] Form: role, organisation, start month, end month, "current role" checkbox (disables end month), points textarea (one per line), published checkbox
- [x] Month inputs (`type="month"`) map to the first day of the month
- [x] Quick publish/unpublish toggle in the list (POST)

**Optional (Should)**
- [ ] Up/down buttons to change display order in Skills and Projects lists (FR-A14)

**Verify**
- [ ] AC-14: create, edit, delete a skill category and its skills; public site updates immediately
- [ ] AC-15: create, edit, delete, publish and unpublish a project; public site updates immediately
- [ ] AC-16: create, edit, delete an experience, including a current role (shows "Present")
- [ ] AC-17: invalid input (empty title, bad URL, end before start, duplicate category name) shows errors and saves nothing
- [ ] AC-18: every state change is a POST with token; delete asks for confirmation
- [ ] Deleting all items of a section hides that section on the public site

---

## Phase 9 - Security Hardening, Tests, Merge

**Goal:** the admin is safe and tested. (AC-18, AC-19, AC-21)

**Hardening**
- [ ] Security headers middleware: `X-Content-Type-Options: nosniff`, `Referrer-Policy: strict-origin-when-cross-origin`, `X-Frame-Options: DENY` (admin area)
- [ ] Friendly error handling for admin (no stack traces in production); log failed logins without passwords
- [ ] Review: no `Html.Raw` on database content; no GET endpoint changes state; all admin controllers have `[Authorize]`
- [ ] Secret scan: no passwords or production connection strings in tracked files (`git grep -i password`, check history of this branch)
- [ ] Confirm `appsettings.Production.json` is ignored by Git
- [ ] Confirm production never auto-migrates (Development only)

**Tests** (`tests/PortfolioWebsite.Application.Tests`, xUnit, plain asserts, no FluentAssertions/Moq)
- [ ] Add test project and references (Application, Domain, Infrastructure); EF InMemory or SQLite in-memory
- [ ] Validator tests for SkillCategory, Project, Experience rules in `PRD §10`
- [ ] Handler tests: create, update, delete for each feature; unique name rule; update replaces skills/points
- [ ] Query tests: public queries hide unpublished items and order correctly; experience date text format

**Merge**
- [ ] `dotnet build PortfolioWebsite.sln` and `dotnet test` pass
- [ ] Owner reviews and approves, then merges `feature/admin` into `main`

**Verify**
- [ ] AC-18 and AC-19 confirmed
- [ ] AC-21: `dotnet test` passes

---

## Phase 10 - SEO, Accessibility, Performance (was Phase 4)

**Goal:** AC-06, AC-07, AC-20 pass. Run this on the finished site including the admin exclusions.

- [ ] Unique `<title>` and meta description in the public layout (values in `CONTENT.md §10`)
- [ ] Open Graph tags: `og:title`, `og:description`, `og:image`, `og:url`
- [ ] `robots.txt` (with `Disallow: /admin`) and `sitemap.xml` (public page only) in `wwwroot/`
- [ ] Confirm admin pages have `noindex` and are not in the sitemap (AC-20)
- [ ] Favicon added
- [ ] Semantic HTML audit: one `h1`, sections use `section`, landmarks present
- [ ] Alt text on all images; `aria-label` on icon-only links
- [ ] Colour contrast meets WCAG AA; visible keyboard focus on links and buttons
- [ ] Performance: minify CSS/JS for production, static file caching, lazy-load below-the-fold images; public page runs a small number of DB queries (one per dynamic section, `AsNoTracking`)
- [ ] Custom 404 page (FR-16) and error page
- [ ] HTTPS redirection and HSTS configured for production
- [ ] Remove unused default files, scripts and styles

**Verify**
- [ ] Lighthouse (Chrome DevTools) on the public page: 90+ in Performance, Accessibility, Best Practices, SEO
- [ ] Title, meta description, Open Graph tags present in page source

---

## Phase 11 - Deployment and Final Review (was Phase 5)

**Goal:** site and admin are live over HTTPS. AC-08 and AC-09 pass.

**Gate (owner):** hosting with SQL Server chosen, production database created, domain decision made.

- [ ] Write `README.md`: description, screenshot, tech stack, architecture overview, how to run locally (SQL Server, user-secrets, migrations)
- [ ] `dotnet publish src/PortfolioWebsite.Web -c Release` succeeds
- [ ] Generate the idempotent migration script: `dotnet ef migrations script --idempotent -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -o deploy/migrate.sql` (do **not** commit the output if it contains anything sensitive)
- [ ] Owner takes a database backup, then runs the script on the production database
- [ ] Configure production settings as environment variables: connection string, `Admin__Email`, `Admin__Password` (first start only)
- [ ] Deploy to the chosen host (PRD §12) and confirm HTTPS works
- [ ] First start creates the admin; log in on production and change or remove the password variable afterwards
- [ ] Production seed check: Skills, Projects, Experience appear on the public site
- [ ] Optional: GitHub Actions workflow that builds and runs tests on every push to `main`
- [ ] Test on latest Chrome, Edge, Firefox, Safari and a real phone (public site and admin)
- [ ] Replace all remaining `null` values in `docs/CONTENT.md` with real owner-provided values (or leave hidden)
- [ ] Final read-through of all content for typos
- [ ] Update `docs/PRD.md` status from Draft to Final

**Verify**
- [ ] AC-08: publicly accessible over HTTPS, works on all target browsers
- [ ] AC-09: no `[TBD]` / placeholder left in the final content
- [ ] Admin login works on production; wrong password is rejected
- [ ] All acceptance criteria AC-01 to AC-21 pass

---

## Blocked / Needs Owner Input

Agents: log any blocker here instead of guessing.

| Date | Task | What is needed |
|---|---|---|
| | | |

---

## Future Ideas (do NOT implement now)

- Cache the public page with invalidation after admin changes
- Admin editing for Profile, Education, Certifications, Achievements
- Image upload for projects
- Drag-and-drop reordering
- Two-factor authentication and audit log
- Contact form via Formspree or MailKit
- Dark mode toggle
- Visit analytics
- Blog
