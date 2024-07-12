# InBudget Spending Tracker

Spending tracker is an single page web app that helps
users log their transactions and link them to spend categories
for better visibility.

## Features

### Transactions

- Create transactions
- View paginated transactions
- Filter Transactions
  - By description text
  - By category
  - By date range (start date, end date)
- Update transactions
- Delete transactions
- Link transactions to spend categories

### Spend Categories

- Create categories
- View categories
- Update categories
- Delete categories
  - Deleting a category will delete all associated transactions

## Running locally

- **Pre-requisites/knowledge**
  - Ensure SQL server local DB is running before proceeding
  - DB connection details configurable in `Budget/appsettings.json`, defaults should work
  - Database should be automatically created if it doesn't exist on the app's first run
  - See `Seed data` section below
- `cd Budget`
- `dotnet run`

## Seed data

If there is no data, random data will be inserted into the database
for demonstration purposes. To disable this, please comment this line
in `Budget/Program.cs`:

```csharp
19 SeedService.Seed(budgetDb);
```

## Tech stack

- Backend
  - C#
  - EF Core
  - ASP.NET MVC
  - SQL Server
- Frontend
  - Pure javascript (single page app)
  - Bootstrap
  - Jquery (for asp.net mvc built-in form validation, mostly)

## Future feature plans

Planning to visualize user spend per category and allow the user to
set monthly targets for a category. This will help the user
visually see their remaining runway for the month.
