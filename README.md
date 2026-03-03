# ICT2112-P2 — Developer Onboarding Guide

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
8. [Demo Day Checklist](#8-demo-day-checklist)
9. [FAQ & Troubleshooting](#9-faq--troubleshooting)

---

## 1. Project Overview

This is a **C# ASP.NET Core** application backed by **PostgreSQL 17**.

There are **multiple teams** working on this project. Each team:
- Shares the **same base data layer and database schema**
- Implements **one feature module** unique to their team
- Runs the app **independently on their own machine** for demo day

The base repo is maintained by the Leaders. Your team works on a **fork** of it.

### Architecture at a Glance

```
[Presentation Layer]   → Controllers + Razor Views (.cshtml)
    ↓
[Domain Layer]         → Domain models, business logic & services
    ↓
[Data Source Layer]    → EF Core DbContext + Entity classes + Repositories
    ↓
[PostgreSQL 17]        → Your local database (schema defined in schema.sql)
```

> **Rule:** Business logic belongs in the Service/Domain layer — never in Controllers or Views.

---

## 2. Prerequisites

Install these **before** running the project. Everyone must use the **same versions** to avoid issues.

| Tool | Version | Download |
|------|---------|----------|
| .NET SDK | 10.0 | https://dotnet.microsoft.com/download |
| PostgreSQL | 17 | https://www.postgresql.org/download/ (install everything except Stack Builder) |
| pgAdmin | Latest | https://www.pgadmin.org/download/ |
| EF Core CLI | Latest | `dotnet tool install --global dotnet-ef` |
| Git | Latest | https://git-scm.com/downloads |
| VS Code | Latest | https://code.visualstudio.com/ |

> ✅ Verify .NET: `dotnet --version` → should print `10.x.x`  
> ✅ Verify PostgreSQL: `psql --version` → should print `17.x`  
> ✅ Verify EF CLI: `dotnet ef --version`

---

## 3. First-Time Setup

Follow these steps **in order**, once when you first join the project.

### Step 1 — Clone the Team Repo

```bash
git clone <your-team-repo-url>
cd <repo-folder>
```

### Step 2 — Add the Base Repo as Upstream

```bash
git remote add upstream <base-repo-url>
git remote -v   # should show both origin and upstream
```

### Step 3 — Create Your Local Database

Open **pgAdmin** → Query Tool, or run `psql -U postgres`, then execute:

```sql
CREATE DATABASE pro_rental;
CREATE USER devuser WITH PASSWORD 'devpassword';
GRANT ALL PRIVILEGES ON DATABASE pro_rental TO devuser;
```

> 💡 You can use any name/password — just keep it consistent with Step 4.

### Step 4 — Configure Your Local Settings

```bash
cp appsettings.Development.json.example appsettings.Development.json
```

Edit `appsettings.Development.json` with your credentials:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=pro_rental;Username=devuser;Password=devpassword"
  }
}
```

> ⚠️ `appsettings.Development.json` is **gitignored** and will never be committed. Never put credentials in `appsettings.json`.

### Step 5 — Create the Database Tables

The full schema is defined in `schema.sql`. Run it against your empty database to create all tables:

**Option A — pgAdmin:**
1. In the left panel, right-click `pro_rental` → **Query Tool**
2. Open `schema.sql` (File → Open) and click **Run (▶)**

**Option B — psql terminal:**
```bash
psql -U postgres -d pro_rental -f schema.sql
```

You should see a series of `CREATE TYPE` and `CREATE TABLE` statements with no errors.

### Step 6 — Apply the EF Core Baseline Migration

This registers the existing tables with EF Core's migration history tracker without touching the database:

```bash
dotnet ef database update --project src/BaseApp.Data
```

You should see output ending in `Done.` — your local environment is fully set up.

---

## 4. Daily Development Workflow

Do this **at the start of every work session**:

```bash
# 1. Get your teammates' latest changes
git pull origin main

# 2. Apply any new migrations (safe to run even if nothing changed)
dotnet ef database update --project src/BaseApp.Data

# 3. Restore packages if needed
dotnet restore
```

### Pulling Updates from the Base Repo

If the project lead announces an update:

```bash
git fetch upstream
git merge upstream/main
dotnet restore
dotnet ef database update --project src/BaseApp.Data
```

---

## 5. Project Structure

```
repo-root/
├── src/
│   └── BaseApp.Data/              ← Data Source Layer (EF Core class library)
│       ├── AppDbContext.cs         ← EF Core DbContext — all DbSet<T> properties
│       ├── DesignTimeDbContextFactory.cs  ← Lets dotnet ef commands work without a web host
│       ├── Entities/               ← Auto-generated entity classes (one per DB table)
│       ├── Migrations/             ← EF Core migration history
│       └── Repositories/          ← Data access classes (add yours here)
│
├── schema.sql                      ← Authoritative DB schema — all teams share this
├── appsettings.json                ← Committed — connection string placeholder only
├── appsettings.Development.json    ← GITIGNORED — your local credentials
└── appsettings.Development.json.example  ← Committed — copy this to get started
```

> The `Entities/` folder is **auto-generated** by `dotnet ef dbcontext scaffold` — do not hand-edit those files. If you need to extend an entity, create a separate partial class file.

---

## 6. Database & Schema Rules

### The Schema is Frozen

The database schema (`schema.sql`) was designed upfront and agreed upon by all teams:

- ✅ Write code that reads from and writes to the existing tables
- ✅ Run `dotnet ef database update` once during setup, then after every pull
- ❌ Do **not** create new migrations unless approved
- ❌ Do **not** modify `schema.sql` without approval from the project lead

### If You Think You Need a Schema Change

1. Raise it with your **team lead**
2. If approved, the project lead updates `schema.sql` in the base repo
3. All teams pull the update and re-run: `psql -U postgres -d pro_rental -f schema.sql` and `dotnet ef database update --project src/BaseApp.Data`

### Migration Owner Rule

If a migration must be created:
- Only the **designated migration owner** runs `dotnet ef migrations add`
- Announce it in the team channel **before** running it
- Never two people adding migrations simultaneously
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
- ✅ Open a **Pull Request** when ready; at least one teammate must review before merging
- ✅ Delete your branch after merging
- ✅ Announce in the team channel before merging anything into `main`

### Typical Branch Workflow

```bash
git checkout main
git pull origin main
git checkout -b feature/my-feature

# develop, then commit regularly
git add .
git commit -m "feat: add order processing repository"

git push origin feature/my-feature
# → open a Pull Request on GitHub/GitLab
```

### Commit Message Convention

| Prefix | Use for |
|--------|---------|
| `feat:` | New feature |
| `fix:` | Bug fix |
| `refactor:` | Code restructure, no behaviour change |
| `docs:` | README or comment updates |
| `chore:` | Config, setup, dependency changes |

---

## 8. Demo Day Checklist

```
☐ git pull origin main
☐ dotnet restore
☐ dotnet ef database update --project src/BaseApp.Data
☐ Database has all expected tables (verify in pgAdmin)
☐ appsettings.Development.json is configured with your credentials
☐ Your team's feature works end-to-end
☐ No unhandled exceptions or error pages visible
```

---

## 9. FAQ & Troubleshooting

**Q: I get "relation does not exist" errors.**  
A: You skipped Step 5. Run `schema.sql` against your database first, then `dotnet ef database update --project src/BaseApp.Data`.

**Q: `dotnet ef database update` says "No migrations were applied."**  
A: That is normal — it means the baseline is already recorded and the database is up to date.

**Q: `dotnet ef` is not recognised as a command.**  
A: Install the EF Core CLI: `dotnet tool install --global dotnet-ef`

**Q: The build fails with "connection refused" or a config error.**  
A: PostgreSQL is not running, or your `appsettings.Development.json` credentials are wrong. Check with pgAdmin that the `pro_rental` database exists and the service is running.

**Q: I pulled from main and now the project won't build.**  
A: Run `dotnet restore` first — a teammate likely added a new NuGet package.

**Q: `appsettings.Development.json` keeps getting overwritten.**  
A: It should be listed in `.gitignore`. If it isn't, add it. Never commit this file.

**Q: I accidentally committed to main.**  
A: Do not push. Run `git reset HEAD~1` to undo, then tell your team lead.

**Q: Two people edited the same file and there's a merge conflict.**  
A: Open the file, look for `<<<<<<<` markers, resolve manually, then `git add . && git commit`.

---

*Last updated: March 2026*  
