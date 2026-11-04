# AGENTS.md

Instructions for AI coding agents working on this repository.
Read this file fully before making any change.

## 1. Project Summary

Personal developer portfolio website for **Md Read Hasan**.
Single-page site built with **ASP.NET Core MVC (.NET 10, LTS)** and **Bootstrap 5**.
There is **no database**. All content comes from an in-memory service.

## 2. Source of Truth (read in this order)

1. `docs/PRD.md` - requirements, architecture (Section 8), acceptance criteria (Section 11).
2. `docs/CONTENT.md` - **all text and data** shown on the site. Wins over PRD Section 5 if they differ.
3. `docs/DESIGN.md` - colours, typography, layout and style. **Required from Phase 3 onwards.** If it does not exist when you reach Phase 3, stop and ask the owner. Do not invent a design.
4. `docs/TASKS.md` - phased checklist. This decides what to work on now.
5. This file - working rules.

If anything is unclear or missing, **ask the owner**. Do not guess.

## 3. Tech Stack

| Area | Choice |
|---|---|
| Framework | ASP.NET Core MVC, .NET 10 (LTS), target framework `net10.0` |
| UI | Razor views, Bootstrap 5, Bootstrap Icons |
| JavaScript | Small vanilla JS file only (`wwwroot/js/site.js`). No frontend frameworks. |
| Data | In-memory C# objects behind `IPortfolioService` |
| Database | **None** |

## 4. Commands

```bash
dotnet restore
dotnet build
dotnet run                      # run locally
dotnet publish -c Release       # production build
```

A task is not finished until `dotnet build` passes with **zero errors and zero warnings you introduced**.

## 5. Architecture Rules

- Pattern: MVC with a service layer. Folder layout is defined in `docs/PRD.md` Section 8.4. Follow it exactly.
- `HomeController` receives `IPortfolioService` by constructor injection and passes **one** `PortfolioViewModel` to `Views/Home/Index.cshtml`.
- DI registration in `Program.cs`: `builder.Services.AddSingleton<IPortfolioService, MockPortfolioService>();`
- All portfolio content lives **only** in `MockPortfolioService`. **Never hard-code content in `.cshtml` files.**
- Each page section is a partial view in `Views/Shared/` that receives only the part of the model it needs.
- Keep the design swappable: a future `EfPortfolioService : IPortfolioService` must work without changing controllers or views.

## 6. Coding Conventions

- C#: PascalCase for types and methods, `_camelCase` for private fields, nullable reference types enabled, `async` where I/O exists.
- One class per file. File name equals class name.
- Razor: no business logic in views. Only loops, conditionals and display formatting.
- CSS: custom styles go in `wwwroot/css/site.css`. Prefer Bootstrap utilities first. Use CSS variables for colours.
- JS: no inline `<script>` blocks in views. Everything goes in `wwwroot/js/site.js`.
- HTML: semantic elements (`header`, `nav`, `main`, `section`, `footer`), one `h1` per page, every section has an `id` matching its navbar link.
- Every image has meaningful `alt` text. Icon-only links have `aria-label`.
- All external links: `target="_blank" rel="noopener noreferrer"`.

## 7. Content Rules (important)

- Use content **exactly** as written in `docs/CONTENT.md`. Do not rephrase, embellish or add skills, projects, dates or achievements.
- **Never invent** URLs, certificate links, GitHub repositories, dates, grades or contact details.
- Any value that is `null` in `docs/CONTENT.md` (or marked `[TBD]` in the PRD) means **not provided yet**:
  - Do not fill it with a placeholder or fake value.
  - Leave the property `null` / empty in the model.
  - The view must hide the related element (for example, no "GitHub" button if `GitHubUrl` is null).
  - List it in your final report under "Needs owner input".
- Do not expose the email address as plain text in page source. Render it as a `mailto:` link.

## 8. Scope Boundaries (do NOT do these)

Unless the owner explicitly asks:

- Do not add a database, Entity Framework, or any persistence.
- Do not add authentication, admin panel, blog, or a contact form.
- Do not add NuGet or npm packages. If you believe one is truly needed, stop and ask first.
- Do not add JavaScript frameworks (React, Vue, jQuery plugins, etc.).
- Do not change the folder structure, target framework, or the PRD.
- Do not refactor or restyle code outside the current task.

## 9. Assets

These files are provided by the owner. Do not generate, replace or rename them:

- `wwwroot/images/profile.jpg` - profile photo
- `wwwroot/files/ReadHasan_CV.pdf` - downloadable CV

The owner copies them into `wwwroot/` **after Phase 1** (once the project and `wwwroot/` exist) and **before Phase 3**. Do not create placeholder images or a fake PDF.

If a file is missing, build the UI so it degrades gracefully (hide the CV button, show initials instead of the photo) and report it.

## 10. Workflow

1. Open `docs/TASKS.md` and find the first unchecked task.
2. Work on **one phase at a time**. Do not start the next phase until the owner says so.
3. Make small, focused changes. Prefer several small commits over one big one.
4. After each task: run `dotnet build`, and run the app if the task affects UI.
5. Tick the finished task in `docs/TASKS.md` by changing `[ ]` to `[x]`.
6. At the end of a phase, run its verification checklist and stop.

### Commit messages

Use short imperative messages, for example:

```text
feat: add PortfolioViewModel and section models
feat: implement MockPortfolioService with profile data
fix: hide GitHub button when URL is null
```

## 11. Definition of Done (per phase)

- [ ] `dotnet build` succeeds.
- [ ] App runs with `dotnet run` and the page loads without console errors.
- [ ] All tasks in the phase are ticked in `docs/TASKS.md`.
- [ ] No hard-coded content in views.
- [ ] No `null` / `[TBD]` value was invented.
- [ ] Work respects Section 8 (scope boundaries).

## 12. Final Report Format

When you finish a phase, reply with:

1. **Done:** what was implemented.
2. **Files changed:** list of created or modified files.
3. **Verified:** commands run and results.
4. **Needs owner input:** missing links, assets or decisions.
5. **Next:** the next phase from `docs/TASKS.md`.

Keep the report short and factual.
