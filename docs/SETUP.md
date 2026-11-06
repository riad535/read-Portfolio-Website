# SETUP.md

Owner-facing guide for running the portfolio locally: SQL Server, connection string, secrets, migrations, and production configuration.
(Paths assume the layout from Phase 4: `PortfolioWebsite.sln` at the repo root and the web project in `src/PortfolioWebsite.Web`.)

**Golden rule:** secrets (passwords, production connection strings) are **never** committed to Git. They go in `user-secrets` (local) or environment variables / an untracked file (production).

---

## 1. Prerequisites

| Tool | Check | Notes |
|---|---|---|
| .NET 10 SDK | `dotnet --version` shows `10.x` | Already installed |
| SQL Server (one option) | See Section 2 | Needed from Phase 6 |
| `dotnet-ef` tool | `dotnet ef --version` shows `10.x` | Install: `dotnet tool install --global dotnet-ef`. Update: `dotnet tool update --global dotnet-ef` |
| Visual Studio 2026 (or VS Code + C# Dev Kit) | | .NET 10 needs Visual Studio 2026 |
| Git | `git --version` | |

---

## 2. Pick a Local SQL Server

| Option | When to use | How to check |
|---|---|---|
| **LocalDB** (recommended) | Installed with Visual Studio (the "ASP.NET and web development" workload). No password, no service to manage. | `sqllocaldb info` lists instances (usually `MSSQLLocalDB`). Start it: `sqllocaldb start MSSQLLocalDB` |
| **SQL Server Express** | You already installed it, or want a normal server | Service `SQL Server (SQLEXPRESS)` is running in Windows Services |
| **SQL Server Developer / full** | You already have it | Connect with SSMS to confirm |

If none is installed: easiest is to add the **ASP.NET and web development** workload in the Visual Studio Installer (includes LocalDB).

---

## 3. Connection String

A connection string tells the app which server and database to use. The app reads it from the key:

```text
ConnectionStrings:DefaultConnection
```

### 3.1 Local development strings

| Server | Connection string |
|---|---|
| **LocalDB** | `Server=(localdb)\MSSQLLocalDB;Database=PortfolioDb;Trusted_Connection=True;TrustServerCertificate=True` |
| **SQL Express** | `Server=.\SQLEXPRESS;Database=PortfolioDb;Trusted_Connection=True;TrustServerCertificate=True` |
| **SQL Server with a login** | `Server=localhost;Database=PortfolioDb;User Id=sa;Password=<your-password>;TrustServerCertificate=True` |

- `Trusted_Connection=True` means Windows login, so **no password** appears in the string.
- `Database=PortfolioDb` does not need to exist yet. `dotnet ef database update` creates it.
- `TrustServerCertificate=True` is for local development only.

### 3.2 Where to put it

| Case | Where | Why |
|---|---|---|
| LocalDB or SQL Express with Windows login (**no password**) | `src/PortfolioWebsite.Web/appsettings.Development.json` | Safe to commit; contains no secret |
| Any string with a **password** | `dotnet user-secrets` (Section 4) | Never in a committed file |
| **Production** | Host environment variable or untracked `appsettings.Production.json` (Section 7) | Never committed |

**`appsettings.Development.json` example (LocalDB)**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PortfolioDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Backslash rule:** inside a JSON file a backslash is written twice (`\\`). In user-secrets and environment variables write it **once** (`\`).

**Do not** put a connection string with a password in `appsettings.json` or `appsettings.Development.json`.

### 3.3 Which value wins (highest wins)

1. Command-line arguments
2. Environment variables
3. User secrets (Development only)
4. `appsettings.{Environment}.json` (for example `appsettings.Development.json`)
5. `appsettings.json`

So a user-secret overrides the value in `appsettings.Development.json`, and an environment variable overrides both.

---

## 4. User Secrets (passwords stay off Git)

Run once from the repo root:

```bash
dotnet user-secrets init --project src/PortfolioWebsite.Web
```

Admin account (needed from Phase 7):

```bash
dotnet user-secrets set "Admin:Email" "your-email@example.com" --project src/PortfolioWebsite.Web
dotnet user-secrets set "Admin:Password" "YourStrongPassword1" --project src/PortfolioWebsite.Web
```

Optional, only if your local SQL Server uses a login and password:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=PortfolioDb;User Id=sa;Password=YourSqlPassword;TrustServerCertificate=True" --project src/PortfolioWebsite.Web
```

Useful commands:

```bash
dotnet user-secrets list   --project src/PortfolioWebsite.Web
dotnet user-secrets remove "Admin:Password" --project src/PortfolioWebsite.Web
dotnet user-secrets clear  --project src/PortfolioWebsite.Web
```

Notes:
- Admin password rules: at least 10 characters with an upper case letter, a lower case letter and a digit.
- In **PowerShell**, wrap values containing `$`, `!` or `;` in **single quotes** (`'...'`) so the shell does not change them.
- Secrets are stored outside the repo (in your Windows user profile). They exist only on your machine.
- `dotnet ef` uses the web project as its startup project, so it reads these secrets too.

---

## 5. Create the Database (Phase 6 onwards)

From the repo root:

```bash
# create or update the database from migrations
dotnet ef database update -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
```

- `-p` = project that contains the migrations (Infrastructure). `-s` = startup project that has the configuration (Web).
- `-- --environment Development` (at the end) is **required**. Without it `dotnet ef` uses the Production environment, does not read `appsettings.Development.json` or user-secrets, and fails with a missing connection string. PowerShell alternative: run `$env:ASPNETCORE_ENVIRONMENT = "Development"` once in the window, then run the commands (you can then omit the trailing part).
- In Development the app can also apply migrations and seed data when it starts.
- **See the database:** Visual Studio, View, **SQL Server Object Explorer**, `(localdb)\MSSQLLocalDB`, Databases, `PortfolioDb`. Or use SSMS / Azure Data Studio with server `(localdb)\MSSQLLocalDB`.
- **Reset the local database** (deletes all local data, including admin changes):

  ```bash
  dotnet ef database drop   -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
  dotnet ef database update -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
  ```

Add a new migration only when the schema changes (agents do this inside their tasks):

```bash
dotnet ef migrations add <Name> -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -- --environment Development
```

Never edit a migration that has already been applied. Never run `database update` against the **production** database from your machine.

---

## 6. Run the Site

```bash
dotnet run --project src/PortfolioWebsite.Web
```

- Public site: `https://localhost:<port>` (the port is printed in the console).
- Admin: `https://localhost:<port>/admin` (login with the email and password you stored in user-secrets).
- Visual Studio: open `PortfolioWebsite.sln`, choose the **https** profile, press **F5**.

---

## 7. Production Configuration

Your host (for example Somee, SmarterASP.NET, Azure) gives you a SQL Server connection string. It looks different from the local one and **contains a password**, so it must not be committed.

| Setting | Key | Where |
|---|---|---|
| Connection string | `ConnectionStrings:DefaultConnection` | Host environment variable `ConnectionStrings__DefaultConnection`, **or** an untracked `appsettings.Production.json` uploaded to the host |
| Admin email | `Admin:Email` | Environment variable `Admin__Email` |
| Admin password | `Admin:Password` | Environment variable `Admin__Password` (needed only for the first start) |

- In environment variables, `:` is written as `__` (double underscore).
- If your host has **no environment variable panel** (common on shared hosting), create `appsettings.Production.json` on your machine and upload it with the published files. It is already in `.gitignore`, so it never goes to GitHub.
- Use the connection string exactly as the host provides it. Follow the host's guidance on `Encrypt` and `TrustServerCertificate`.
- **Create the production tables:** generate a script, take a database backup first, then run the script in the host's SQL tool:

  ```bash
  dotnet ef migrations script --idempotent -p src/PortfolioWebsite.Infrastructure -s src/PortfolioWebsite.Web -o migrate.sql -- --environment Development
  ```

- The first production start seeds Skills, Projects, Experience and creates the admin account (from `Admin__Email` / `Admin__Password`). Afterwards change or remove the password variable.
- Production never applies migrations automatically.

---

## 8. Troubleshooting

| Error / symptom | Likely cause | Fix |
|---|---|---|
| `A network-related or instance-specific error` | SQL Server instance is not running or the server name is wrong | LocalDB: `sqllocaldb start MSSQLLocalDB`. Express: start the Windows service. Check `Server=` spelling. |
| `The certificate chain was issued by an authority that is not trusted` | Local server certificate | Add `TrustServerCertificate=True` (local only) |
| `Cannot open database "PortfolioDb"` | Database not created yet | Run `dotnet ef database update ...` |
| `Login failed for user` | Wrong SQL login or password | Recheck the user-secret value, or use `Trusted_Connection=True` |
| `The ConnectionString property has not been initialized` | `dotnet ef` ran without the Development environment, or the key name is wrong or the value is missing | Add `-- --environment Development` to the `dotnet ef` command; key must be exactly `ConnectionStrings:DefaultConnection`; run `dotnet user-secrets list` |
| `Unable to create a 'DbContext'` during `dotnet ef` | Wrong `-p` / `-s` or the project does not build | Build the solution first, then use the commands in Section 5 exactly |
| `dotnet ef` not found | Tool not installed | `dotnet tool install --global dotnet-ef` |
| Backslash looks doubled or missing in the server name | JSON vs secrets escaping | `\\` in JSON files, `\` in secrets and environment variables |
| Cannot log in to `/admin` | Admin not seeded or password too weak | Check `Admin:Email` / `Admin:Password` secrets; restart the app (the seeder creates the admin only if no user exists; reset the local database to start over) |

---

## 9. Checklist

**Before Phase 6**
- [ ] LocalDB or SQL Express available (Section 2)
- [ ] `dotnet ef --version` shows 10.x

**Before Phase 7**
- [ ] `dotnet user-secrets init` done
- [ ] `Admin:Email` and `Admin:Password` set in user-secrets

**Before Phase 11 (deployment)**
- [ ] Host with SQL Server chosen, production connection string available
- [ ] Production settings prepared (environment variables or untracked `appsettings.Production.json`)
- [ ] Backup taken before running `migrate.sql`
