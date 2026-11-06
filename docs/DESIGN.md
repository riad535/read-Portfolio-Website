# DESIGN.md

Visual specification for the portfolio website. Public site (Sections 1 to 13) applies from **Phase 3**. Admin panel (Sections 14 and 15) applies from **Phase 7**.
Content comes from `docs/CONTENT.md`. Structure and behaviour come from `docs/PRD.md`. This file only decides **how it looks**.

**Rules for agents**
- Follow this file exactly. Do not invent colours, fonts or layouts that are not listed here.
- Put all colours, spacing and radii in CSS variables in `wwwroot/css/site.css` (Section 3). Never hard-code hex values elsewhere.
- No new UI libraries, animation libraries or NuGet/npm packages. Use Bootstrap 5 utilities, Bootstrap Icons, and custom CSS.
- Do not add portfolio text that is not in `docs/CONTENT.md`. Decorative numbers, icons and lines are allowed (Section 8). Admin panel UI text is allowed as listed in Section 14.

---

## 1. Design Direction

**Style:** modern, minimal, developer-focused. Dark theme with one cool accent. Lots of whitespace, clear hierarchy, soft cards, subtle motion.

**Mood keywords:** professional, calm, confident, technical.

**Why this direction:** the current page is plain white with default Bootstrap styling and no visual identity. The new look should feel like a polished engineer portfolio: strong hero, consistent cards, clear sections, accent colour used sparingly to guide the eye.

**Single theme only** (dark). No light/dark toggle in v1.

---

## 2. Colour Palette

| Role | Name | Hex | Use |
|---|---|---|---|
| Background | Midnight | `#0B1020` | Page background |
| Background alt | Deep Navy | `#0F1629` | Alternate sections (every second section) |
| Surface | Slate Card | `#141C33` | Cards, navbar (scrolled), footer |
| Surface hover | Slate Lift | `#1A2442` | Card hover |
| Border | Line | `#222D4D` | Card borders, dividers |
| Text primary | Mist | `#E6EAF2` | Headings, body text |
| Text muted | Steel | `#94A3B8` | Secondary text, captions, dates |
| Accent 1 | Sky | `#38BDF8` | Links, icons, active nav, highlights |
| Accent 2 | Indigo | `#818CF8` | Gradient partner, badges |
| On accent | Ink | `#0B1020` | Text on accent buttons |
| Success/hover glow | Sky glow | `rgba(56, 189, 248, 0.15)` | Shadows, focus halo |

**Gradient (primary buttons, hero ring, section underline):**
`linear-gradient(135deg, #38BDF8 0%, #818CF8 100%)`

**Rules**
- Use at most **two accent colours** (Sky, Indigo). No other bright colours.
- Body text must stay `#E6EAF2` on dark backgrounds. Muted text only for secondary info.
- All text/background pairs must meet **WCAG AA** (4.5:1). The palette above does.

---

## 3. CSS Variables (copy into `site.css`)

```css
:root {
  /* colours */
  --bg: #0B1020;
  --bg-alt: #0F1629;
  --surface: #141C33;
  --surface-hover: #1A2442;
  --border: #222D4D;
  --text: #E6EAF2;
  --text-muted: #94A3B8;
  --accent: #38BDF8;
  --accent-2: #818CF8;
  --on-accent: #0B1020;
  --glow: rgba(56, 189, 248, 0.15);
  --gradient: linear-gradient(135deg, #38BDF8 0%, #818CF8 100%);

  /* shape */
  --radius-sm: 8px;
  --radius-md: 14px;
  --radius-lg: 20px;
  --radius-pill: 999px;

  /* shadow */
  --shadow-card: 0 8px 24px rgba(0, 0, 0, 0.25);
  --shadow-hover: 0 12px 32px rgba(56, 189, 248, 0.12);

  /* spacing scale (rem) */
  --space-1: 0.25rem;
  --space-2: 0.5rem;
  --space-3: 1rem;
  --space-4: 1.5rem;
  --space-5: 2.5rem;
  --space-6: 4rem;
  --space-7: 6rem;

  /* typography */
  --font-body: "Inter", system-ui, -apple-system, "Segoe UI", Roboto, Arial, sans-serif;
  --font-mono: "JetBrains Mono", ui-monospace, "Cascadia Code", Consolas, monospace;

  /* motion */
  --ease: cubic-bezier(0.2, 0.7, 0.2, 1);
  --t-fast: 150ms;
  --t-base: 300ms;
}
```

Set `<html lang="en" data-bs-theme="dark">` so Bootstrap's own components also use dark colours. Override Bootstrap variables (`--bs-body-bg`, `--bs-body-color`, `--bs-border-color`) with the values above.

---

## 4. Typography

| Item | Spec |
|---|---|
| Body font | **Inter** (weights 400, 500, 600, 700) |
| Mono font | **JetBrains Mono** (weight 500) for small labels, tech tags, dates |
| Loading | Google Fonts `<link>` with `display=swap` and `preconnect`. Always keep the fallback stack from `--font-body`. |
| Base size | 16px, line-height 1.7 |

| Element | Size (desktop / mobile) | Weight | Notes |
|---|---|---|---|
| Hero name (`h1`) | 3.5rem / 2.25rem | 700 | Name or part of it uses `--gradient` text fill |
| Hero designation | 1.25rem / 1.05rem | 500 | `--accent` colour |
| Hero tagline | 1.1rem / 1rem | 400 | `--text-muted` |
| Section title (`h2`) | 2.25rem / 1.75rem | 700 | With accent underline (Section 8) |
| Card title (`h3`) | 1.25rem | 600 | |
| Body | 1rem | 400 | Max line length about 70 characters |
| Small / meta | 0.875rem | 500 | Mono font for dates and tags |

Letter-spacing: `-0.02em` on `h1`, `h2`. Normal elsewhere.

---

## 5. Layout and Spacing

- Container: Bootstrap `.container`, max width **1140px**, side padding 1.25rem on mobile.
- Section vertical padding: `--space-7` (6rem) desktop, `--space-6` (4rem) mobile.
- Sections alternate background: Hero `--bg`, About `--bg-alt`, Education `--bg`, Skills `--bg-alt`, and so on. Contact uses `--bg-alt`.
- Grid gap between cards: 1.5rem.
- Anchor offset: `scroll-margin-top: 80px` on every section (sticky navbar height).
- Breakpoints (Bootstrap): `<576`, `576`, `768`, `992`, `1200`.

---

## 6. Components

### 6.1 Buttons
| Type | Look |
|---|---|
| Primary | Background `--gradient`, text `--on-accent`, weight 600, radius `--radius-pill`, padding `0.7rem 1.6rem`. Hover: lift `translateY(-2px)` and `--shadow-hover`. |
| Outline | Transparent, 1px border `--border`, text `--text`, radius pill. Hover: border and text `--accent`, background `--glow`. |
| Icon button | 44x44px circle, border `--border`, icon `--text-muted`. Hover: icon `--accent`, border `--accent`. |

Minimum tap target **44x44px**. Visible focus: `outline: 2px solid var(--accent); outline-offset: 3px;`.

### 6.2 Cards
- Background `--surface`, border 1px `--border`, radius `--radius-md`, padding 1.5rem, shadow `--shadow-card`.
- Hover (cards that are interactive): background `--surface-hover`, border `rgba(56,189,248,0.4)`, `translateY(-4px)`, `--shadow-hover`. Transition `--t-base var(--ease)`.

### 6.3 Badges / tech chips
- Background `rgba(129, 140, 248, 0.12)`, text `#C7D2FE`, 1px border `rgba(129, 140, 248, 0.3)`, radius pill, padding `0.25rem 0.75rem`, font mono 0.8rem.

### 6.4 Section heading
- `h2` in `--text`, below it a **48px x 4px** bar filled with `--gradient`, radius pill, margin-top 0.75rem.
- Centred on all sections.

### 6.5 Icons
- **Bootstrap Icons** only. Size 1.25rem in cards, colour `--accent`.

---

## 7. Section-by-Section Design

### 7.1 Navbar
- Sticky top, height about 72px, `backdrop-filter: blur(12px)`.
- At page top: transparent background. After scrolling 20px: background `rgba(11, 16, 32, 0.85)`, bottom border 1px `--border` (toggle a class in `site.js`).
- Left: brand = full name, weight 700, letter-spacing -0.01em. Small gradient dot before it is allowed.
- Right: nine links, font 0.9rem, weight 500, colour `--text-muted`. Hover: `--text`. **Active link** (scroll spy): colour `--accent` with a 2px gradient underline.
- Because there are nine links, **collapse into the hamburger below the `xl` breakpoint (1200px)**. The mobile menu is a full-width panel with `--surface` background and generous link spacing.

### 7.2 Hero (`#home`)
- Full viewport height (`min-height: 100vh`, minus navbar), content vertically centred.
- Subtle background: two blurred radial glows (`--accent` at 12% opacity top-right, `--accent-2` at 10% opacity bottom-left). CSS only.
- **Desktop (two columns, 7/5):**
  - Left: name (`h1`), designation (accent), tagline (muted), then buttons: **Download CV** (primary), **GitHub** (outline, with icon), **LinkedIn** (outline, with icon).
  - Right: profile photo, circle 320px, 4px gradient ring with 8px gap, soft glow shadow `0 0 60px var(--glow)`.
- **Mobile:** single column, photo first (200px circle), then text, all centred, buttons stacked or wrapped.
- Photo fallback: if the photo file is missing, show a circle with initials "RH" on `--surface` with the gradient ring.
- Optional: a small scroll-down chevron at the bottom centre (decorative, `aria-hidden`).

### 7.3 About (`#about`)
- Narrow centred column (max 760px). Bio as a paragraph in `--text-muted` at 1.1rem, first words not styled.
- Below the bio, a row of 3 small info items with icons: **Location** (`bi-geo-alt`), **Email** (`bi-envelope`), **Phone** (`bi-telephone`). Values from `CONTENT.md`. Email is a `mailto:` link.

### 7.4 Education (`#education`)
- One centred card (max 640px) with a graduation-cap icon (`bi-mortarboard`) in a 48px rounded square (`--glow` background).
- Degree as card title, institution as muted text. Show period or CGPA **only if not null**.

### 7.5 Skills (`#skills`)
- Responsive grid: 3 columns desktop, 2 tablet, 1 mobile. One **card per category**.
- Each card: icon in a rounded square, category title, then skill **chips** (6.3) wrapping inside.
- Category icons: Backend `bi-code-slash`, Architecture `bi-diagram-3`, Database `bi-database`, Frontend & Mobile `bi-phone`, Tools `bi-tools`, Problem Solving `bi-puzzle`.

### 7.6 Projects (`#projects`)
- Grid: 2 columns desktop, 1 mobile. Large cards with hover lift.
- Card top: a gradient strip (4px) or a folder icon. No stock images.
- Content: title, description (muted), **Tech stack** as chips, **Architecture** as a muted mono line (hide if null).
- Footer of card: **Live Demo** (primary small) and **GitHub** (outline small), each shown **only if URL is not null**. If both are null, show no footer.

### 7.7 Certifications (`#certifications`)
- Vertical list or 2-column grid of cards. Each: certificate icon (`bi-patch-check`), course name, platform (muted mono), and **View Certificate** link (accent, with `bi-box-arrow-up-right`) only if URL is not null.

### 7.8 Experience (`#experience`)
- **Vertical timeline**, centred on desktop (alternating left/right is NOT needed; keep all cards on the right of a left line for simplicity and readability).
- Left line 2px `--border`; each entry has a 14px dot filled with `--gradient` and a glow ring.
- Entry card: role (title), organisation (accent), date range (mono, muted), bullet points (muted, custom accent bullets).
- Mobile: line and dots stay on the left, cards fill the width.

### 7.9 Achievements (`#achievements`)
- Grid 3 columns desktop, 1 mobile. Small cards with a trophy icon (`bi-trophy`), title, and the **year as a large mono number** in `--accent`.

### 7.10 Contact (`#contact`)
- Centred card (max 720px) on `--bg-alt`, large radius `--radius-lg`, subtle gradient border.
- Content from `CONTENT.md`: a short section title, then a big primary button **Send an Email** (`mailto:`), phone and location lines with icons, and a row of icon buttons for GitHub and LinkedIn.
- No form (PRD Section 6).

### 7.11 Footer
- Background `--surface`, top border `--border`, padding 2rem.
- Footer text centred in `--text-muted`, small. Social icon buttons above it.
- Back-to-top button: fixed bottom-right 44px circle, `--gradient` background, `bi-arrow-up`, hidden until the user scrolls 400px, fade in/out.

---

## 8. Decorative Elements (allowed)

- Section number via CSS counter (`01`, `02`, ...) shown above each section title in `--font-mono`, `--accent`, small. This is purely decorative (`::before`), not added to HTML content.
- Gradient underline bar under each section title (6.4).
- Soft background glows in the hero only.
- Thin divider lines in `--border`.

---

## 9. Motion

- Navbar background change: `--t-base`.
- Cards and buttons: hover lift (Sections 6.1, 6.2).
- **Reveal on scroll:** sections and cards fade in and slide up 16px once, using `IntersectionObserver` in `site.js` (adds class `is-visible`). Duration 500ms, `--ease`. Only vanilla JS.
- Smooth scroll via `scroll-behavior: smooth` plus JS for the active link.
- **Respect `prefers-reduced-motion: reduce`:** disable reveal, lift and smooth scroll.
- No parallax, no auto-playing carousels, no heavy animation.

---

## 10. Responsive Behaviour

| Width | Behaviour |
|---|---|
| `< 576px` | Single column everywhere. Hero photo above text, centred. Buttons wrap or stack. Section padding 4rem. |
| `576 - 991px` | Skills 2 columns, Projects 1 column. Hamburger menu. |
| `992 - 1199px` | Skills 3 columns, Projects 2 columns. Hamburger menu (nine nav links need the extra space). |
| `>= 1200px` | Full layout, inline nav links. |

No horizontal scrolling from 320px upwards. Test at 320, 768, 1024, 1440.

---

## 11. Accessibility

- Contrast AA on all text. Do not use `--text-muted` below 14px.
- Visible `:focus-visible` outline on every interactive element.
- Icon-only buttons have `aria-label`. Decorative icons have `aria-hidden="true"`.
- Navigation uses a `<nav>` landmark with `aria-label="Main"`. Sections use `<section>` with `aria-labelledby` pointing to the section title.
- Profile image alt text from `CONTENT.md` (`PhotoAlt`).
- Mobile menu and back-to-top work with keyboard only.

---

## 12. Do Not

- Do not use white or light backgrounds for sections.
- Do not use more than two accent colours or add random colours per card.
- Do not use emojis in the UI.
- Do not use stock photos, placeholder images or fake logos.
- Do not add AOS, GSAP, Swiper, jQuery plugins or any animation/UI library.
- Do not add a dark/light toggle, language switcher, or a contact form.
- Do not leave default Bootstrap look unstyled (default blue buttons, default navbar, default card).
- Do not show empty UI for `null` values (no empty buttons, no "N/A" text). Hide the element.

---

## 13. Design Acceptance Checklist (verify at the end of Phase 3)

- [ ] Dark palette applied only through CSS variables from Section 3.
- [ ] Inter and JetBrains Mono load with fallbacks.
- [ ] Navbar is transparent at top, solid after scroll, collapses below 1200px, active link highlighted.
- [ ] Hero has photo with gradient ring, name, designation, tagline, and 3 buttons.
- [ ] Every section has the number, title and gradient underline.
- [ ] Cards share the same radius, border, hover behaviour.
- [ ] Projects hide Live Demo / GitHub buttons when the URL is null.
- [ ] Reveal-on-scroll works and is disabled for reduced motion.
- [ ] No horizontal scroll at 320 / 768 / 1024 / 1440 px.
- [ ] Back-to-top button appears after scrolling.

---

## 14. Admin Panel Design (Phase 7 onwards)

The admin panel is a tool, not a showcase. It must look clearly related to the public site (same dark palette, fonts, radii) but be calmer, denser and more functional.

### 14.1 General Rules
- Reuse the CSS variables from Section 3. Admin-only styles go in `wwwroot/css/admin.css`, loaded only by `_AdminLayout.cshtml`. Do not load `site.css` animations (reveal on scroll, hero glows) in admin.
- Same fonts: Inter for text, JetBrains Mono for ids, dates and technical values.
- Same components as Section 6 (buttons, cards, chips), with the additions below.
- Every admin page has `<meta name="robots" content="noindex, nofollow">`.
- No decorative section numbers, no gradients except on the primary button and the active sidebar marker.

### 14.2 Extra Semantic Colours (admin only)

| Role | Hex | Use |
|---|---|---|
| Success | `#34D399` | "Published" badge, success flash |
| Danger | `#F87171` | Delete button, error text, error flash |
| Warning | `#FBBF24` | Draft/unpublished hint, caution |

Backgrounds for badges and alerts use the same colour at 12% opacity with a 1px border at 30% opacity. Add them as `--success`, `--danger`, `--warning` in `admin.css`. These colours are **only** for status and feedback, never for decoration.

### 14.3 Layout
- **Desktop (>= 992px):** fixed left sidebar 250px wide (`--surface`, right border `--border`) and a content area (`--bg`) with a topbar (64px) and a page container (max width 1100px, padding 2rem).
- **Below 992px:** sidebar becomes an offcanvas menu opened from a hamburger in the topbar.
- **Sidebar:** brand (name) at top. Links with icons: Dashboard `bi-speedometer2`, Skills `bi-lightning-charge`, Projects `bi-folder2-open`, Experience `bi-briefcase`, then a divider, then **View site** `bi-box-arrow-up-right` (new tab) and **Logout** `bi-box-arrow-right` (a POST form styled as a link).
- Active link: text `--text`, background `--glow`, 3px left marker with `--gradient`.
- **Topbar:** page title on the left, admin email (muted, mono) on the right.

### 14.4 Login Page
- No sidebar. Centred card (max width 400px, `--radius-lg`, subtle gradient border) on `--bg` with the soft glow of the hero.
- Content: brand name, title "Admin Login", email field, password field, primary full-width button "Sign in".
- Error alert (danger style) above the form: "Invalid email or password." Lockout message: "Too many attempts. Try again later."
- No "Register", no "Forgot password", no social login.
- Password field has autocomplete attributes (`username`, `current-password`).

### 14.5 Dashboard
- Three stat cards in a row (1 column on mobile): Skill categories, Projects, Experience. Large mono number in `--accent`, label in muted text, icon in a rounded square, whole card links to the section.
- Below: a short "Quick actions" card with buttons "Add skill category", "Add project", "Add experience".

### 14.6 List Pages (Skills, Projects, Experience)
- Page header: title on the left, primary button **Add new** on the right (with `bi-plus-lg`).
- Table inside a card (`--surface`, radius `--radius-md`). Header row: `--bg-alt`, muted uppercase 0.75rem text. Row border `--border`, row hover `--surface-hover`. Cell padding 0.9rem 1rem.
- Actions column, right aligned: icon buttons **Edit** (`bi-pencil`) and **Delete** (`bi-trash`, danger on hover). Each has `aria-label` and a tooltip `title`.
- Status badge for Projects and Experience: **Published** (success) or **Draft** (muted/warning). Clicking the badge button toggles it (POST).
- Optional up/down buttons (`bi-arrow-up`, `bi-arrow-down`) beside the order number.
- Below 768px the table scrolls horizontally inside its card. The page body must not scroll sideways.
- **Empty state:** centred icon, text "No items yet.", and an **Add new** button.

### 14.7 Forms (Create and Edit)
- One card (max width 760px). Labels above inputs, weight 500. Helper text in muted 0.85rem under the field.
- Inputs and textareas: background `--bg-alt`, 1px `--border`, radius `--radius-sm`, padding 0.65rem 0.9rem, text `--text`. Focus: border `--accent` and a 3px `--glow` ring.
- Short fields (display order, dates, icon) sit two per row on desktop; long fields and textareas are full width.
- Textareas for lists (skills, points) show helper text "One per line."; tech stack shows "Separate with commas."
- Icon field is a `<select>` with the allowed icon names; each option label shows the icon name (for example `bi-database`).
- Month fields use `type="month"`. When "Current role" is checked, the end month input is disabled and cleared (small JS in `admin.js`).
- Validation errors: field border `--danger`, message below the field in `--danger` 0.85rem. A summary alert at the top when there are errors.
- Buttons at the bottom: **Save** (primary) and **Cancel** (outline, links back to the list).

### 14.8 Delete Confirmation Modal
- Bootstrap modal in dark style (`--surface`, `--border`, `--radius-md`). Title "Delete item?". Body: "Delete **{name}**? This cannot be undone." Buttons: **Cancel** (outline) and **Delete** (danger fill) which submits a POST form with the anti-forgery token.
- The modal text names the item being deleted. Deleting a skill category also says "Its skills will be deleted too."

### 14.9 Flash Messages
- Alerts at the top of the content area, using success or danger style with an icon (`bi-check-circle`, `bi-exclamation-triangle`) and a close button.
- Success messages auto-dismiss after 4 seconds. Error messages stay until closed.
- Text examples: "Project saved.", "Skill category deleted.", "Could not save. Please fix the errors below."

### 14.10 Admin UI Text (allowed to write)
Keep labels short and neutral: Add new, Edit, Delete, Save, Cancel, Sign in, Sign out, View site, Published, Draft, No items yet. Page titles: Dashboard, Skills, Projects, Experience, New skill category, Edit skill category, New project, Edit project, New experience, Edit experience.

### 14.11 Responsive and Accessibility
- Usable from 360px up. Tap targets at least 44x44px, including icon buttons.
- Visible `:focus-visible` outline on every control. Colour contrast AA.
- Modal traps focus and closes with Escape. Flash alerts use `role="alert"` for errors and `role="status"` for success.
- Forms: every input has a `<label for>`; errors are linked with `aria-describedby`.
- Sidebar is a `<nav aria-label="Admin">`.

### 14.12 Do Not (admin)
- Do not use the public site's hero, glows, reveal animations or section numbers in admin.
- Do not use default unstyled Bootstrap tables, forms or alerts. Style them with the variables above.
- Do not use more than the three semantic colours for status.
- Do not add charts, rich-text editors, drag-and-drop libraries or any new UI library.
- Do not show admin links, buttons or text on the public site.

---

## 15. Admin Design Acceptance Checklist (verify at the end of Phase 7 and Phase 8)

- [ ] `admin.css` loads only in the admin layout; public pages are unchanged.
- [ ] Login page is centred, with generic error messages and no register/forgot links.
- [ ] Sidebar works on desktop and as offcanvas on mobile; active link is marked.
- [ ] Dashboard shows three stat cards with correct numbers and links.
- [ ] List tables use the dark card style; actions have aria-labels; empty states are shown.
- [ ] Forms show errors next to fields and keep the entered values.
- [ ] Delete uses the confirmation modal and a POST form.
- [ ] Flash messages appear and success messages auto-dismiss.
- [ ] Status badges (Published/Draft) use the semantic colours only.
- [ ] No horizontal page scroll at 360 / 768 / 1024 / 1440 px.
- [ ] Every admin page has `noindex, nofollow`.

