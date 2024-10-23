# :money_with_wings: CodeReviews.MVC.Budget  
4th ASP.NET Core MVC Project of the [C# Academy](https://www.thecsharpacademy.com/)  
**Goal**: Create a personal financial budgeting application

## Requirements

:heavy_check_mark: This application allows users to record personal finance transactions.  
:heavy_check_mark: There should be two linked tables: Transaction and Category.  
:heavy_check_mark: Entity Framework must be used; raw SQL is not allowed.  
:heavy_check_mark: Each transaction MUST have a category, and if a category is deleted, all its transactions should be deleted as well.  
:heavy_check_mark: SQL Server must be used (no SQLite).  
:heavy_check_mark: The application should include a search functionality to find transactions by name.  
:heavy_check_mark: A filter feature should be provided to display transactions by category and date.  
:heavy_check_mark: Modals should be used for inserting, deleting, and updating transactions and categories. These operations should not require navigating to a different page.

## Description

I decided to expand upon my final project for the ASP.NET Core MVC section. In addition to the default requirements, I aimed to create an application that helps users become more self-aware of their spending habits and encourages more mindful spending. Users can evaluate their recorded transactions twice: first, when recording the transaction, and second, after a certain period of time. Monthly and yearly statistics are presented via charts, allowing users to quickly assess how "happy" their transactions made them and how "necessary" those transactions were.

## Getting Started

### Required Installation Steps

* Restore NuGet packages.
* Insert your SQL Server connection string into `appsettings.json`.  
* There is no need to run `npm restore`, as all necessary packages are already bundled.
* If the 'SeedData' and 'Auto-Migrate' options in `appsettings.json` are set to 'true', the latest migration will automatically be applied to the database, and seed data will be inserted.

### Usage

I aimed to make the layout and usage of the application as self-explanatory as possible. The only thing I’d like to point out is how to open the action menu for a category:

#### Category Action Menu

Clicking on any income or expense category will open a menu where the user can directly add a transaction without needing to navigate to the details page. Other possible actions include deleting, editing, or navigating to the details page of the selected category. A rotating border indicates which category is currently selected.

![image](https://github.com/user-attachments/assets/ea3af5f4-5fc0-4e82-ad31-7ddff73f6737)

Otherwise, the general workflow is as follows:
1. Add a budget/fiscal plan (e.g., "My Budget," "Work")—you can create as many as you like!
2. Add categories (e.g., "Groceries," "Rent," "Salary").
3. Add transactions to the categories. When adding a transaction, the user can select smileys that represent their sentiment toward the transaction ("Am I happy with this transaction?") and its necessity ("Is this transaction necessary?"), where necessity refers to realistic needs rather than survival needs.

#### Sidebar

The sidebar contains five buttons: four navigate to different sections of the app, and the last returns the user to the fiscal plan selection.

1. Monthly overview of the selected month (changeable via the date input).  
2. Reevaluate transactions after a certain period. In the yearly statistics, users can see how often their evaluation of a transaction has changed.  
3. Yearly statistics (monthly overspending, total spending, etc.).  
4. Search for transactions.

## Disclaimer

This is a learning project as part of my journey to mastering C# and beyond.
