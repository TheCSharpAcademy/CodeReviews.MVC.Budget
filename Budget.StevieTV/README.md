# Project - Budget Tracker - MVC Relational Database

The goal of this project was to create a MVC app to track personal finances with categories and transactions in two seperate tables. An additional major requirement on this project is to do all adding and editing via modals instead of separate views.

The project is part of TheCSharpAcademy: https://thecsharpacademy.com/project/27

### Requirements

*   This is an application where you should record personal finance transactions.
*   You should have two linked tables: Transaction and Category.
*   You need to use Entity Framework, raw SQL isn't allowed.
*   Each transaction MUST have a category and if you delete a category all it's transactions should be deleted.
*   You should use SQL Server, not SQLite.
*   You should have a search functionality where I can search transactions by name
*   You should have a filter functionality, so I can show transactions per category and per date.
*   You need to use modals to insert, delete and update transactions and categories. These operations shouldn't be done in a different page.

### Features of the project

*   Interface has two pages for CRUD operations on transactions and categories 
*   Uses Bootstrap for nicer styling
*   Seeds database with sample data when no data exists
*   Edit Modals are populated via js (get details via fetch and populate form with returned json)