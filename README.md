# read-Portfolio-Website

Personal developer portfolio website for **Md Read Hasan** (ASP.NET Core MVC, .NET 10, Bootstrap 5).

## Solution layout

```text
PortfolioWebsite.sln
src/
  PortfolioWebsite.Domain/          # entities (later phases)
  PortfolioWebsite.Application/     # CQRS use cases (later phases)
  PortfolioWebsite.Infrastructure/  # persistence (later phases)
  PortfolioWebsite.Web/             # MVC site, views, static assets
tests/                              # test projects (later phases)
docs/                               # PRD, content, design, tasks and setup
```

## Run locally

```bash
dotnet restore
dotnet build PortfolioWebsite.sln
dotnet run --project src/PortfolioWebsite.Web
```

The development site runs at <http://localhost:5114>.

## Documentation

- `docs/PRD.md` - requirements and architecture
- `docs/CONTENT.md` - all site content
- `docs/DESIGN.md` - colours, typography and layout
- `docs/TASKS.md` - phased implementation checklist
- `docs/SETUP.md` - local setup, configuration and secrets
- `AGENTS.md` - instructions for AI coding agents
