# AGENTS.md

Instructions for AI coding agents working on this repository.
Read this file fully before making any change.

## 1. Project Summary

Personal developer portfolio website for **Md Read Hasan**.

- **Public site:** single-page portfolio (ASP.NET Core MVC, .NET 10, Bootstrap 5).
- **Admin panel:** one admin user manages **Skills, Projects and Experience** (create, edit, delete) stored in **SQL Server**.
- **Architecture:** Clean Architecture + CQRS (MediatR 12.5.0).
- Profile, Education, Certifications and Achievements stay **static in code**.

## 2. Source of Truth (read in this order)

1. `docs/PRD.md` - requirements, architecture (Section 8), database (Section 9), validation (Section 10), acceptance criteria (Section 13).
2. `docs/CONTENT.md` - **all text and data** shown on the site. Also the seed data for the database. Wins over anything else for content.
3. `docs/DESIGN.md` - colours, typography, layout and style. Public site (Sections 1 to 13) and admin panel (Section 14).
4. `docs/TASKS.md` - phased checklist. This decides what to work on now.
5. `docs/SETUP.md` - owner-facing setup guide (connection strings, user-secrets, migrations, production configuration). Read it whenever you touch configuration.
6. This file - working rules.

If anything is unclear or missing, **ask the owner**. Do not guess.

## 3. Tech Stack

| Area | Choice |
|---|---|
| Framework | ASP.NET Core MVC, .NET 10 (LTS), `net10.0` |
| Architecture | Clean Architecture, CQRS |
| CQRS | **MediatR 12.5.0, exact version** |
| Database | SQL Server, Entity Framework Core 10 |
| Auth | ASP.NET Core Identity (no Identity UI package), cookie authentication |
| UI | Razor views, Bootstrap 5, Bootstrap Icons |
| JavaScript | Small vanilla JS files only. No frontend frameworks. |
| Tests | xUnit, EF Core InMemory or SQLite in-memory |

### Package rules
- **Allowed:** MediatR (exactly `12.5.0`), `Microsoft.EntityFrameworkCore.*` 10.x, `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 10.x, xUnit and its runner packages, `Microsoft.EntityFrameworkCore.InMemory` / `Sqlite` (tests only).
- **Forbidden:** MediatR 13 or newer (licence key required), AutoMapper (commercial), FluentValidation, FluentAssertions, Moq, any JavaScript framework.
- Validation is **our own code** (see Section 5). Mapping is **manual**.
- Any other package: **stop and ask the owner first.**

## 4. Commands

Run from the repository root (the folder with `PortfolioWebsite.sln`, after Phase 4).

```bash
dotnet restore
dotnet build PortfolioWebsite.sln
dotnet run --project src/PortfolioWebsite.Web
dotnet test
dotnet publish src/PortfolioWebsite.Web -c Release

# EF Core migrations
dotnet ef migrations add <Name> -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
dotnet ef database update      -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
```

**Why `-- --environment Development`:** `dotnet ef` runs the web app's configuration in *Production* by default, so `appsettings.Development.json` and user-secrets would not be loaded and the connection string would be missing. Always add it to every `dotnet ef` command.

A task is not finished until `dotnet build PortfolioWebsite.sln` passes with **zero errors and zero warnings you introduced**.

## 5. Architecture Rules (Clean Architecture + CQRS)

Layout is defined in `docs/PRD.md` Section 8.4. Follow it exactly.

### Dependency rule
```text
Web ──► Application ──► Domain
Web ──► Infrastructure ──► Application
```
- **Domain** references nothing. Plain entities only.
- **Application** references Domain only (plus MediatR and `Microsoft.EntityFrameworkCore` abstractions). Never reference Infrastructure or Web. Never reference `Microsoft.EntityFrameworkCore.SqlServer`.
- **Infrastructure** implements Application interfaces: `ApplicationDbContext`, entity configurations, migrations, seeder, Identity setup.
- **Web** is the composition root. It references Infrastructure **only** to call `AddInfrastructure()` in `Program.cs`.
- Controllers **never** use `DbContext` directly. They send commands and queries through `ISender`.
- Handlers use `IApplicationDbContext`. No repository classes.

### CQRS conventions
- One folder per use case: `Features/<Area>/Commands/<UseCase>/` or `Queries/<UseCase>/`.
- Files per use case: `<UseCase>Command.cs` (or `Query.cs`), `<UseCase>Handler.cs`, and for commands `<UseCase>Validator.cs`.
- Commands change state, return an `int` id or nothing. Queries never change state, return DTO **records**, use `AsNoTracking()`.
- Validation: implement `IRequestValidator<TRequest>`; `ValidationBehaviour` (MediatR pipeline) runs validators and throws our own `ValidationException`. Database-dependent rules (for example unique name) go in the handler and throw the same exception.
- Missing entity: throw `NotFoundException`. Web maps it to the 404 page.
- DTOs, commands and queries are immutable records.

### Public site
- `HomeController` → `IPortfolioService.GetPortfolioAsync()` → `PortfolioService` (static parts from `IStaticContentProvider`, dynamic parts from MediatR queries) → public view models → views.
- Static content lives **only** in `StaticContentProvider`. Dynamic content lives **only** in the database.
- **Never hard-code content in `.cshtml` files.**
- Each page section is a partial view that receives only the part of the model it needs.

## 6. Coding Conventions

- C#: PascalCase types and methods, `_camelCase` private fields, nullable reference types enabled, `async` for I/O with `CancellationToken` passed through handlers.
- One type per file. File name equals type name.
- Razor: no business logic in views, only loops, conditionals and formatting. **Never use `Html.Raw` on database content.**
- CSS: public styles in `wwwroot/css/site.css`, admin styles in `wwwroot/css/admin.css`. Colours via CSS variables.
- JS: no inline `<script>` blocks. Public JS in `wwwroot/js/site.js`, admin JS in `wwwroot/js/admin.js`.
- HTML: semantic elements, one `h1` per page, every public section has an `id` matching its navbar link.
- Every image has meaningful `alt` text. Icon-only links and buttons have `aria-label`.
- External links: `target="_blank" rel="noopener noreferrer"`.
- Trim all text input before validating and saving.

## 7. Content Rules

- Use content **exactly** as written in `docs/CONTENT.md`. Do not rephrase, embellish or add skills, projects, dates or achievements.
- **Never invent** URLs, certificate links, GitHub repositories, dates, grades or contact details.
- Any value that is `null` in `docs/CONTENT.md` means **not provided yet**:
  - Do not fill it with a placeholder or fake value.
  - Keep it `null` in the model and database.
  - The view must hide the related element (for example no "GitHub" button if the URL is null).
  - List it in your final report under "Needs owner input".
- The database seeder must create Skills, Projects and Experience from `docs/CONTENT.md` exactly.
- Do not expose the email address as plain text in public page source. Render it as a `mailto:` link.
- **Admin panel UI text** (button labels, headings, validation messages, flash messages) is not portfolio content. You may write it, keep it short and neutral, and follow `docs/DESIGN.md` Section 14.

## 8. Security Rules (admin)

- **Never** commit secrets: passwords, production connection strings, API keys. The admin email and password come from `user-secrets` (local) or environment variables (production): `Admin:Email`, `Admin:Password`.
- `appsettings.Production.json` must be in `.gitignore`.
- `[Authorize]` on every admin controller except login. Public controllers stay anonymous.
- Anti-forgery token on every POST (global `AutoValidateAntiforgeryTokenAttribute`). **No state change on GET.**
- Login: generic error message, lockout enabled (5 attempts, 15 minutes), rate limited, local return URLs only.
- Admin pages send `noindex`. Do not link to admin from the public site.
- Production database: **never auto-migrate** and never run `dotnet ef database update` against it. Only generate scripts.
- Details: `docs/PRD.md` Section 7.

## 9. Scope Boundaries (do NOT do these)

Unless the owner explicitly asks:

- Do not add admin editing for Profile, Education, Certifications or Achievements.
- Do not add public registration, multiple users, roles, social login, password reset UI or two-factor authentication.
- Do not add image or file upload.
- Do not add a blog, contact form, comments, caching layer or analytics.
- Do not add a dark/light toggle or multi-language.
- Do not add packages outside Section 3. Do not upgrade MediatR.
- Do not change the PRD, the database schema outside migrations, or the folder structure.
- Do not refactor or restyle code outside the current task.

## 10. Assets

These files are provided by the owner. Do not generate, replace or rename them:

- `src/PortfolioWebsite.Web/wwwroot/images/profile.jpg` - profile photo
- `src/PortfolioWebsite.Web/wwwroot/files/ReadHasan_CV.pdf` - downloadable CV

(Before Phase 4 they live under `wwwroot/` in the repo root. Phase 4 moves them with the web project.)
If a file is missing, degrade gracefully (hide the CV button, show initials instead of the photo) and report it.

## 11. Workflow

1. Open `docs/TASKS.md` and find the first unchecked task.
2. Work on **one phase at a time**. Do not start the next phase until the owner says so.
3. Phases 4 to 9 are done on the Git branch **`feature/admin`**. Do not merge into `main` until Phase 9 is approved by the owner.
4. Small, focused changes. Prefer several small commits over one big one.
5. After each task run `dotnet build PortfolioWebsite.sln`. Run the app if the task affects UI.
6. Tick the finished task in `docs/TASKS.md` (`[ ]` to `[x]`).
7. At the end of a phase run its verification checklist and **stop**.

### Migration workflow
- One migration per logical schema change, with a descriptive name (`InitialCreate`, `AddProjectIsPublished`).
- Never edit or delete a migration that was already applied. Add a new one.
- Apply to the local database only.

### Commit messages
Short, imperative:
```text
refactor: move web project to src/PortfolioWebsite.Web
feat: add Project entity and CQRS commands
feat: add admin login with Identity
fix: hide GitHub button when URL is null
```

## 12. Definition of Done (per phase)

- [ ] `dotnet build PortfolioWebsite.sln` succeeds.
- [ ] App runs with `dotnet run --project src/PortfolioWebsite.Web`, page loads, no console errors.
- [ ] All tasks of the phase are ticked in `docs/TASKS.md`.
- [ ] Dependency rule from Section 5 holds.
- [ ] No hard-coded content in views. No `null` / `[TBD]` value was invented.
- [ ] No secrets in the repository.
- [ ] Work respects Section 9 (scope boundaries).

## 13. Final Report Format

When you finish a phase, reply with:

1. **Done:** what was implemented.
2. **Files changed:** created or modified files.
3. **Verified:** commands run and results.
4. **Needs owner input:** missing links, assets, secrets or decisions.
5. **Next:** the next phase from `docs/TASKS.md`.

Keep it short and factual.
