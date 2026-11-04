# TASKS.md

Phased implementation checklist for the Portfolio Website.

**How to use (for agents):**
- Work on **one phase at a time**. Stop at the end of each phase and wait for the owner.
- Tick `[x]` when a task is done and verified.
- References like `PRD §8.4` point to sections of `docs/PRD.md`. AC-xx are acceptance criteria from `PRD §11.1`.
- All content comes from `docs/CONTENT.md`. All visuals come from `docs/DESIGN.md` (Phase 3 onwards).
- Rules are in `AGENTS.md`. Never invent `null` / `[TBD]` content.

**Status legend:** `[ ]` todo, `[x]` done, `[!]` blocked (needs owner input)

---

## Before You Start (owner checklist)

These are outside the agent's control. The agent should only report if they are missing.

**Before Phase 1**
- [ ] .NET 10 SDK installed (`dotnet --version` shows 10.x)
- [ ] Empty repo folder created with `AGENTS.md` in the root and `docs/PRD.md`, `docs/CONTENT.md`, `docs/TASKS.md`

**After Phase 1, before Phase 3** (the project and `wwwroot/` do not exist until Phase 1 is done)
- [ ] Copy `profile.jpg` to `wwwroot/images/profile.jpg` (already cropped 600x600, 38 KB)
- [ ] Copy the CV PDF to `wwwroot/files/ReadHasan_CV.pdf`
- [ ] Create `docs/DESIGN.md` (colours, fonts, style)

**Any time before Phase 5**
- [ ] Correct certificate link for "Full Stack ASP.NET Core MVC Web Development"
- [ ] GitHub repository URLs for MaleFashion and Blood Donation (or leave `null`)
- [ ] Education period / expected graduation
- [ ] Hosting provider and domain decision
- [ ] Update the matching `null` values in `docs/CONTENT.md`

> Phases 1, 2 and 4 can run without the "any time" items. Missing items must be hidden in the UI, not faked.

---

## Phase 1 - Project Setup and Layout

**Goal:** empty page runs with navbar and footer. DI is wired.

- [ ] Create project **in the repo root** (not in a sub-folder): `dotnet new mvc -n PortfolioWebsite -o . --framework net10.0`. Do not overwrite `AGENTS.md` or `docs/`.
- [ ] Confirm `PortfolioWebsite.csproj` has `<TargetFramework>net10.0</TargetFramework>`
- [ ] Add `.gitignore` for .NET (`bin/`, `obj/`, `.vs/`) and initialise Git
- [ ] Confirm `AGENTS.md` and `docs/` files are still in place and untouched
- [ ] Create folders: `Models/`, `Services/`, `wwwroot/images/`, `wwwroot/files/` (keep empty; the owner adds the assets)
- [ ] Remove default template content (Privacy page, sample text, default styles not needed)
- [ ] Ensure Bootstrap 5 is available; add Bootstrap Icons (PRD §8.1)
- [ ] Build `Views/Shared/_Layout.cshtml`: meta viewport, navbar (FR-01), `<main>`, footer (FR-14)
- [ ] Navbar links for all sections: Home, About, Education, Skills, Projects, Certifications, Experience, Achievements, Contact
- [ ] Create `wwwroot/css/site.css` with CSS variables (colours, spacing) and `wwwroot/js/site.js` (empty for now)
- [ ] Register in `Program.cs`: `AddControllersWithViews()` and (after Phase 2) the portfolio service

**Verify**
- [ ] `dotnet build` passes
- [ ] `dotnet run` shows layout with navbar and footer, no console errors
- [ ] Navbar collapses to hamburger on a narrow window

---

## Phase 2 - Models, Service and Data

**Goal:** controller returns a full `PortfolioViewModel` from mock data.

- [ ] Create `ProfileViewModel` (name, designation, location, phone, email, bio, GitHub URL, LinkedIn URL, photo path, CV path)
- [ ] Create `EducationViewModel` (degree, institution, period nullable)
- [ ] Create `SkillViewModel` (category, list of skills)
- [ ] Create `ProjectViewModel` (title, description, tech stack, architecture, nullable `LiveDemoUrl`, nullable `GitHubUrl`)
- [ ] Create `CertificationViewModel` (course name, platform, nullable verification URL)
- [ ] Create `ExperienceViewModel` (role, organisation, date range, bullet points)
- [ ] Create `AchievementViewModel` (title, year)
- [ ] Create `PortfolioViewModel` aggregating all of the above
- [ ] Create `IPortfolioService` with `PortfolioViewModel GetPortfolio()`
- [ ] Implement `MockPortfolioService` with data copied **exactly** from `docs/CONTENT.md` sections 1 to 7 (and labels from sections 8 to 9)
  - [ ] Profile and links (CONTENT §1)
  - [ ] Education (CONTENT §2), period left null
  - [ ] Skills, 6 categories (CONTENT §3)
  - [ ] Projects (CONTENT §4), MaleFashion live demo set, GitHub URLs left null
  - [ ] Certifications (CONTENT §5), second certificate URL left null
  - [ ] Experience (CONTENT §6)
  - [ ] Achievements (CONTENT §7)
- [ ] Register `AddSingleton<IPortfolioService, MockPortfolioService>()` in `Program.cs`
- [ ] Update `HomeController` to inject the service and pass `PortfolioViewModel` to `Index`

**Verify**
- [ ] `dotnet build` passes
- [ ] Temporarily render a few model values on `Index` to confirm data flows (remove afterwards)
- [ ] Data matches `docs/CONTENT.md` word for word; nothing invented

---

## Phase 3 - Sections, Styling and Interactions

**Goal:** all sections render from the model. AC-01 to AC-05 pass.

**Gate (check first, stop if any is missing and ask the owner):**
- [ ] `docs/DESIGN.md` exists
- [ ] `wwwroot/images/profile.jpg` exists
- [ ] `wwwroot/files/ReadHasan_CV.pdf` exists (if not, continue and hide the CV button)

- [ ] `_Hero.cshtml`: photo, name, designation, tagline, buttons: Download CV, GitHub, LinkedIn (FR-03, FR-13)
- [ ] `_About.cshtml`: bio and location (FR-04)
- [ ] Education section partial (FR-05)
- [ ] `_Skills.cshtml`: cards or badges grouped by category (FR-06)
- [ ] `_Projects.cshtml`: one card per project; Live Demo and GitHub buttons only if the URL exists (FR-07)
- [ ] `_Certifications.cshtml`: list with "View Certificate" only if the URL exists (FR-09)
- [ ] `_Experience.cshtml`: timeline with date range and bullets (FR-10)
- [ ] `_Achievements.cshtml`: IUPC list with year (FR-11)
- [ ] `_Contact.cshtml`: `mailto:` link, phone, location, GitHub and LinkedIn icons; no form (FR-12, PRD §6)
- [ ] Compose all partials in `Views/Home/Index.cshtml` in the order of PRD §4; each section has an `id`
- [ ] `site.js`: smooth scroll to sections (FR-02), active navbar link on scroll, back-to-top button (FR-15)
- [ ] All external links open in a new tab with `rel="noopener noreferrer"` (FR-08)
- [ ] Graceful fallback if photo or CV file is missing
- [ ] Responsive polish at 320, 768, 1024, 1440 px (no horizontal scroll)
- [ ] Apply colours, fonts and style from `docs/DESIGN.md` through CSS variables in `site.css`

**Verify**
- [ ] AC-01: all sections render from `MockPortfolioService`; no hard-coded content in views
- [ ] AC-02: navbar scrolls smoothly and highlights the active section
- [ ] AC-03: layout does not break at 320 / 768 / 1024 / 1440 px
- [ ] AC-04: external links open in new tab; Live Demo opens MaleFashion
- [ ] AC-05: CV button downloads the PDF; GitHub and LinkedIn open the correct profiles

---

## Phase 4 - SEO, Accessibility, Performance

**Goal:** AC-06 and AC-07 pass.

- [ ] Unique `<title>` and meta description in `_Layout`
- [ ] Open Graph tags: `og:title`, `og:description`, `og:image`, `og:url` (PRD §7)
- [ ] `robots.txt` and `sitemap.xml` in `wwwroot/`
- [ ] Favicon added
- [ ] Semantic HTML audit: one `h1`, sections use `section`, landmarks present
- [ ] Alt text on all images; `aria-label` on icon-only links
- [ ] Colour contrast meets WCAG AA; visible keyboard focus on links and buttons
- [ ] Performance: minify CSS/JS for production, enable static file caching, lazy-load below-the-fold images
- [ ] Custom 404 page (FR-16) and error page
- [ ] HTTPS redirection and HSTS configured for production (PRD §7 Security)
- [ ] Remove unused default files, scripts and styles

**Verify**
- [ ] Lighthouse (Chrome DevTools) 90+ in Performance, Accessibility, Best Practices, SEO
- [ ] Page title, meta description, Open Graph tags present in page source

---

## Phase 5 - Deployment and Final Review

**Goal:** site is live over HTTPS. AC-08 and AC-09 pass.

- [ ] Write `README.md`: description, screenshot, tech stack, how to run locally
- [ ] `dotnet publish -c Release` succeeds
- [ ] Deploy to the chosen host (PRD §10) and confirm HTTPS works
- [ ] Optional: GitHub Actions workflow that builds on every push to `main`
- [ ] Test on latest Chrome, Edge, Firefox, Safari and on a real phone
- [ ] Replace all remaining `null` values in `docs/CONTENT.md` with real owner-provided values (or leave hidden)
- [ ] Final read-through of all content for typos
- [ ] Update `docs/PRD.md` status from Draft to Final

**Verify**
- [ ] AC-08: publicly accessible over HTTPS, works on all target browsers
- [ ] AC-09: no `[TBD]` / placeholder left in the final content
- [ ] All acceptance criteria AC-01 to AC-09 pass

---

## Blocked / Needs Owner Input

Agents: log any blocker here instead of guessing.

| Date | Task | What is needed |
|---|---|---|
| | | |

---

## Future Ideas (do NOT implement now)

- `EfPortfolioService` with SQL Server and Entity Framework Core (PRD §9)
- Admin panel for editing content
- Blog
- Contact form via Formspree or MailKit
- Dark mode toggle
- Visit analytics
