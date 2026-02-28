# 🚀 [Project Name] — Developer Onboarding Guide

> **Read this fully before writing a single line of code.**  
> This guide covers everything you need to get your local environment running and understand how the team works together.

---

## 📋 Table of Contents

1. [Project Overview](#1-project-overview)
2. [Prerequisites](#2-prerequisites)
3. [First-Time Setup](#3-first-time-setup)
4. [Daily Development Workflow](#4-daily-development-workflow)
5. [Project Structure](#5-project-structure)
6. [Database & Schema Rules](#6-database--schema-rules)
7. [Git & Branching Strategy](#7-git--branching-strategy)
8. [Team Coordination](#8-team-coordination)
9. [Demo Day Checklist](#9-demo-day-checklist)
10. [FAQ & Troubleshooting](#10-faq--troubleshooting)

---

## 1. Project Overview

This is a **C# ASP.NET Core MVC** web application backed by **PostgreSQL**.

There are **3 teams** working on this project. Each team:
- Shares the **same base application and database schema**
- Implements **one feature module** unique to their team
- Runs the app **independently on their own machine** for demo day

The base repo is maintained by the Leaders. Your team works on a **fork** of it.

### Architecture at a Glance

```
[Browser]
    ↓
[Presentation Layer]   → Controllers + Razor Views (.cshtml)
    ↓
[Service Layer]        → Business logic, use case coordination
    ↓
[Domain Layer]         → Domain models, business rules
    ↓
[Data Source Layer]    → EF Core DbContext + Repositories
    ↓
[PostgreSQL 16]        → Your local database
```

> **Rule:** Business logic belongs in the Service/Domain layer — never in Controllers or Views.

---

## 2. Prerequisites

Install these **before** running the project. Everyone must use the **same versions** to avoid issues.

| Tool | Version | Download |
|------|---------|----------|
| .NET SDK | 10.0 | https://dotnet.microsoft.com/download |
| PostgreSQL | 16 | https://www.postgresql.org/download/ |
| pgAdmin (optional GUI) | Latest | https://www.pgadmin.org/download/ |
| Git | Latest | https://git-scm.com/downloads |
| VS Code | Latest | https://code.visualstudio.com/ |

> ✅ After installing .NET, verify with: `dotnet --version` (should print `10.x.x`)  
> ✅ After installing PostgreSQL, verify with: `psql --version` (should print `16.x`)

---

## 3. First-Time Setup

Follow these steps **in order**, once when you first join the project.

### Step 1 — Clone the Team Repo

```bash
git clone <your-team-repo-url>
cd <repo-folder>
```

### Step 2 — Add the Base Repo as Upstream

This lets you pull base updates when needed.

```bash
git remote add upstream <base-repo-url>
git remote -v   # verify: you should see both origin and upstream
```

### Step 3 — Create Your Local Database

Open **pgAdmin** or run these commands in your terminal:

```sql
-- In psql or pgAdmin Query Tool:
CREATE DATABASE team_dev;
CREATE USER devuser WITH PASSWORD 'devpassword';
GRANT ALL PRIVILEGES ON DATABASE team_dev TO devuser;
```

> 💡 You can use any database name, username, and password — just make sure it matches what you put in Step 4.

### Step 4 — Configure Your Local Settings

Copy the example config file and fill in your own credentials:

```bash
cp appsettings.Development.json.example appsettings.Development.json
```

Then open `appsettings.Development.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=team_dev;Username=devuser;Password=devpassword"
  }
}
```

> ⚠️ `appsettings.Development.json` is **gitignored** — it will never be committed. Never put your credentials in `appsettings.json`.

### Step 5 — Apply the Database Schema

This runs the project's migrations and creates all the tables in your local database:

```bash
dotnet ef database update --project src/BaseApp.Data
```

You should see output ending in `Done.` — your database is now set up.

### Step 6 — Run the Application

```bash
dotnet run --project src/BaseApp.Web
```

Open your browser at `http://localhost:5000`. If you see the home page, you're all set! 🎉

---

## 4. Daily Development Workflow

Do this **at the start of every work session**:

```bash
# 1. Get your teammates' latest changes
git pull origin main

# 2. Apply any new migrations (usually a no-op since schema is locked)
dotnet ef database update --project src/BaseApp.Data

# 3. Start the app
dotnet run --project src/BaseApp.Web
```

### Pulling Updates from the Professor's Base Repo

If the professor announces an update to the base application:

```bash
git fetch upstream
git merge upstream/main
dotnet ef database update --project src/BaseApp.Data
dotnet run --project src/BaseApp.Web
```

---

## 5. Project Structure

```
repo-root/
├── src/
│   ├── BaseApp.Web/          ← Presentation Layer
│   │   ├── Controllers/            ← Handle HTTP requests (keep these thin!)
│   │   ├── Views/                  ← Razor .cshtml pages
│   │   └── Program.cs              ← App entry point & dependency injection
│   │
│   ├── BaseApp.Domain/       ← Domain & Service Layer
│   │   ├── Models/                 ← Domain model classes
│   │   └── Services/               ← Business logic & use case coordination
│   │
│   └── BaseApp.Data/         ← Data Source Layer
│       ├── AppDbContext.cs          ← EF Core DbContext (Unit of Work)
│       ├── Migrations/              ← Database migrations (schema history)
│       └── Repositories/           ← Data access classes
│
├── appsettings.json                 ← Committed — structure only, no secrets
├── appsettings.Development.json     ← GITIGNORED — your local credentials
├── appsettings.Development.json.example  ← Committed — template to copy
└── .gitignore
```

### Where Does My Feature Code Go?

Your team's feature lives in:
- **Controllers** → `src/BaseApp.Web/Controllers/`
- **Views** → `src/BaseApp.Web/Views/[FeatureName]/`
- **Services** → `src/BaseApp.Domain/Services/`
- **Models** → `src/BaseApp.Domain/Models/`

> ❌ Do not modify files inherited from the base repo unless discussed with your team lead.

---

## 6. Database & Schema Rules

### The Schema is Frozen

The database schema was **designed upfront and agreed upon by all teams**. This means:

- ✅ You write code that uses the existing tables
- ✅ You run `dotnet ef database update` once during setup
- ❌ You do **not** create new migrations during development
- ❌ You do **not** modify existing tables without approval

### If You Think You Need a Schema Change

1. Raise it with your **team lead** first
2. If approved, the Leader updates the base repo
3. All teams then pull the update via `git fetch upstream && git merge upstream/main`

> This process exists to protect all 3 teams — a schema change that only works for one team breaks everyone else.

### Migration Owner Rule

If a migration ever does need to be created within your team:
- Only the **designated migration owner** for your sub-team runs `dotnet ef migrations add`
- Coordinate in your team channel before doing this — never two people adding migrations at the same time
- The migration must be reviewed and merged before anyone else pulls

---

## 7. Git & Branching Strategy

### Branch Structure

```
main                            ← Always stable, always demo-ready
├── feature/your-feature-name   ← Your work goes here
├── feature/another-feature     ← Another sub-team's work
└── fix/bug-description         ← Bug fixes
```

### Rules

- ❌ **Never commit directly to `main`**
- ✅ Always create a feature branch: `git checkout -b feature/your-feature-name`
- ✅ Open a **Pull Request** when your feature is ready
- ✅ At least **one teammate reviews** your PR before it is merged
- ✅ Delete your branch after merging

### Typical Branch Workflow

```bash
# Start a new feature
git checkout main
git pull origin main
git checkout -b feature/my-feature

# Work, commit regularly
git add .
git commit -m "feat: add order processing service"

# Push and open a Pull Request
git push origin feature/my-feature
# → Go to GitHub/GitLab and open a PR against main
```

### Commit Message Convention

Use clear, consistent commit messages:

| Prefix | Use for |
|--------|---------|
| `feat:` | New feature |
| `fix:` | Bug fix |
| `refactor:` | Code restructure, no behaviour change |
| `docs:` | README or comment updates |
| `chore:` | Config, setup, dependency changes |

Example: `feat: add carbon footprint calculator service`

---

## 8. Team Coordination


### Communication Rules

- Announce in the team channel **before** you merge anything into `main`
- Announce **before** creating a migration (should be rare)

---

## 9. Demo Day Checklist

Run through this checklist on demo day **before** presenting:

```
☐ git pull origin main
☐ dotnet ef database update --project src/BaseApp.Data
☐ dotnet run --project src/BaseApp.Web
☐ App loads at http://localhost:5000
☐ Shared base features work correctly
☐ Your team's feature module works end-to-end
☐ appsettings.Development.json is configured correctly
☐ No debug output / error pages visible
```

---

## 10. FAQ & Troubleshooting

**Q: The app won't start and says "connection refused" for the database.**  
A: PostgreSQL is not running. Open pgAdmin or run `pg_ctl start` in your terminal. Also double-check your connection string in `appsettings.Development.json`.

**Q: I get "relation does not exist" errors.**  
A: You haven't applied migrations. Run `dotnet ef database update --project src/BaseApp.Data`.

**Q: I pulled from main and now the app won't build.**  
A: Run `dotnet restore` first, then try again. A teammate may have added a new NuGet package.

**Q: My `appsettings.Development.json` keeps getting overwritten.**  
A: Check that `.gitignore` includes `appsettings.Development.json`. If not, add it.

**Q: I accidentally committed to main.**  
A: Tell your team lead immediately. Do not push. Run `git reset HEAD~1` to undo the commit locally.

**Q: Two people edited the same file and now there's a merge conflict.**  
A: Don't panic. Open the conflicting file, look for `<<<<<<` markers, resolve manually, then `git add .` and `git commit`. Ask a teammate if unsure.

**Q: Do I need to re-run migrations every time I pull?**  
A: It's a safe habit — run `dotnet ef database update` after every pull. Since the schema is locked, it will usually print `No migrations were applied` and exit.

---

*Last updated: February 2026*  
