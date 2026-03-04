# ICT2112 – Pro Rental Order Processing System

## Module 1 – Order Processing System

This repository contains the implementation for **Module 1: Order Processing System** of the ICT2112 team project.

The system allows customers and staff to:

- Place rental orders
- Track order status
- Process checkout and delivery
- Handle rental returns
- Manage deposit refunds

**Technology Stack:**

- ASP.NET Core MVC
- PostgreSQL
- Entity Framework Core

---

## Project Structure

```
Repository
│
├── Module1/                  # Main ASP.NET MVC project
│   ├── Controllers/          # MVC controllers
│   ├── Models/               # Domain models
│   ├── Views/                # Razor views
│   ├── wwwroot/              # Static files
│   ├── Properties/
│   ├── Program.cs            # Application entry point
│   ├── Module1.csproj
│   └── appsettings.json
│
├── src/                      # Base repository files
│
├── schema.sql                # Database schema
├── appsettings.json
├── appsettings.Development.json
├── ICT2112-P2.sln            # Base repo solution file (not used for Module1)
└── README.md
```

> [!WARNING]
> The runnable MVC project is located in `Module1/`. Do **NOT** run the base solution (`ICT2112-P2.sln`).

---

## Prerequisites

Ensure the following tools are installed:

### .NET SDK

```bash
dotnet --version
# Expected: 10.0.101
```

### PostgreSQL

```bash
psql --version
# Expected: psql (PostgreSQL) 17.9
```

### Entity Framework CLI

```bash
dotnet ef --version
# Expected: Entity Framework Core .NET Command-line Tools 10.0.3
```

If not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

## First-Time Setup

### 1. Clone Repository

```bash
git clone <repo-url>
cd ICT2112-P2-MOD1
```

### 2. Create Database

Open the PostgreSQL terminal:

```bash
psql -U postgres
```

Run the following commands:

```sql
CREATE DATABASE pro_rental;
CREATE USER devuser WITH PASSWORD 'devpassword';
GRANT ALL PRIVILEGES ON DATABASE pro_rental TO devuser;
```

Connect to the database:

```sql
\c pro_rental
```

Grant schema permissions:

```sql
GRANT ALL ON SCHEMA public TO devuser;
ALTER SCHEMA public OWNER TO devuser;
```

Exit:

```sql
\q
```

---

## Running the Project

Navigate to the MVC project:

```bash
cd Module1
```

Run the application:

```bash
dotnet run
```

The application will start at:

```
http://localhost:xxxx
```

---

## Git Branch Workflow

> [!WARNING]
> Do **NOT** push directly to `main`. All development must be done using feature branches.

### Create a New Branch

Before starting work:

```bash
git checkout main
git pull origin main
git checkout -b feature/<feature-name>
```

Example:

```bash
git checkout -b feature/cart-page
```

### Push Your Branch

```bash
git push origin feature/<feature-name>
```

Then open a **Pull Request**.

### Daily Workflow

Before starting work each day:

```bash
# Pull latest changes
git checkout main
git pull origin main

# Switch to your feature branch
git checkout feature/<your-feature>

# Merge latest main
git merge main

# Commit your changes
git add .
git commit -m "feat: implemented checkout page"
git push
```

---

## EF Core Migrations

The baseline migration has already been applied. You do **not** need to run migrations during setup.

Only run this if the database schema changes:

```bash
dotnet ef database update
```

---

## Team Development Rules

Follow these rules to avoid Git conflicts.

### 1. Never Push Directly to `main`

Always create a feature branch:

```
feature/<feature-name>
```

Examples: `feature/cart`, `feature/checkout`, `feature/order-status`

### 2. Always Pull Latest `main` Before Starting Work

```bash
git checkout main
git pull origin main
```

### 3. Work on One Feature Per Branch

**Good:**
```
feature/cart
feature/checkout
```

**Bad:**
```
feature/cart-and-checkout
```

### 4. Do Not Edit the Same Files Simultaneously

Communicate with teammates before modifying shared files such as:

- Controllers
- Database models
- `Program.cs`
- `appsettings.json`

### 5. Use Clear Commit Messages

```
feat: implemented checkout page
fix: corrected order status bug
refactor: cleaned cart service logic
```

---

## Common Troubleshooting

### Database Connection Error

**Error:**
```
Npgsql: Failed to connect to database
```

**Check:**
- PostgreSQL is running
- Database `pro_rental` exists
- User `devuser` exists with the correct password

**Verify connection:**
```bash
psql -U devuser -d pro_rental
```

---

### EF Command Not Found

**Error:**
```
dotnet ef command not found
```

**Fix:**
```bash
dotnet tool install --global dotnet-ef
dotnet ef --version
```

---

### Port Already in Use

**Error:**
```
Address already in use
```

**Fix:** Stop the existing process or restart the application.

---

### PostgreSQL Not Recognized

**Error:**
```
psql is not recognized
```

**Fix:** Add the PostgreSQL `bin` folder to your system `PATH`.

Example path:
```
C:\Program Files\PostgreSQL\17\bin
```

---

## Maintainer Notes

Initial setup already completed:

- [x] PostgreSQL database configuration
- [x] EF Core baseline migration
- [x] Database schema initialization

Team members only need to:

1. Clone the repository
2. Create a local database
3. Run the `Module1` project
4. Develop features using feature branches

---

## Contributors

### Team 4

| Name | Role |
|------|------|
| Min Wei | Walk-in Process |
| Yu Sheng | Order Tracking |
| Raffael | Shipping Agent Management |
| Ye Kai | Staff Dashboard |
| YeZi | Customer Dashboard |
| Eugene | Internal Notification System |

### Team 6

| Name | Role |
|------|------|
| Daffa | Order CRUD & Order Management |
| Valerie | Authentication & Session Management |
| Yu Chen | Order Pricing & Deposit Calculation |
| Tharshini | Online Catalogue Browsing |
| Belle | Cart & Checkout Management |
| Beatrice | Payment Processing & Gateway Integration |