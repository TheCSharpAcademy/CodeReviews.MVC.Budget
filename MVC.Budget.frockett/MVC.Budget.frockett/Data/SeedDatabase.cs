using Microsoft.EntityFrameworkCore;

namespace MVC.Budget.frockett.Data;

public class SeedDatabase
{

    public static void Seed(BudgetContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Categories.Any() && !context.Transactions.Any())
        {
            string rawSql = @"
                INSERT INTO Categories (Name) VALUES ('Groceries');
                INSERT INTO Categories (Name) VALUES ('Utilities');
                INSERT INTO Categories (Name) VALUES ('Entertainment');
                INSERT INTO Categories (Name) VALUES ('Travel');
                INSERT INTO Categories (Name) VALUES ('Health');
                INSERT INTO Categories (Name) VALUES ('Education');

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Grocery Shopping', 120.50, '2024-04-05', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Electricity Bill', 60.75, '2024-04-10', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Movie Night', 45.00, '2024-04-12', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Weekend Getaway', 250.00, '2024-04-20', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Doctor Visit', 90.00, '2024-04-22', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Course Materials', 75.00, '2024-04-25', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Weekly Groceries', 110.00, '2024-04-27', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Water Bill', 30.20, '2024-04-30', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Concert Tickets', 100.00, '2024-05-01', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Train Tickets', 150.00, '2024-05-03', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Dental Checkup', 200.00, '2024-05-05', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Online Course', 90.00, '2024-05-07', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Grocery Haul', 85.30, '2024-05-09', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Gas Bill', 45.00, '2024-05-10', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Theme Park', 180.00, '2024-05-12', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Flight Tickets', 400.00, '2024-05-15', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Physical Therapy', 75.00, '2024-05-17', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Workshop Fee', 50.00, '2024-05-20', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Monthly Groceries', 200.00, '2024-05-23', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Internet Bill', 65.00, '2024-05-25', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Game Subscription', 15.00, '2024-05-27', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Road Trip', 300.00, '2024-05-29', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Medication', 45.00, '2024-06-01', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('College Books', 120.00, '2024-06-03', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Fresh Produce', 55.50, '2024-06-05', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Heating Bill', 70.00, '2024-06-07', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Museum Visit', 25.00, '2024-06-10', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Bus Pass', 50.00, '2024-06-12', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Gym Membership', 40.00, '2024-06-15', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Seminar Registration', 30.00, '2024-06-17', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Household Items', 80.00, '2024-06-19', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Phone Bill', 45.00, '2024-06-21', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Theater Tickets', 60.00, '2024-06-23', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Hotel Booking', 350.00, '2024-06-25', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Optician Appointment', 80.00, '2024-06-27', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Course Fee', 100.00, '2024-06-29', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Weekly Groceries', 110.00, '2024-04-15', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Water Bill', 30.20, '2024-04-18', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Concert Tickets', 100.00, '2024-05-03', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Train Tickets', 150.00, '2024-05-06', 4);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Dental Checkup', 200.00, '2024-05-08', 5);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Online Course', 90.00, '2024-05-13', 6);

                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Grocery Haul', 85.30, '2024-05-16', 1);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Gas Bill', 45.00, '2024-05-22', 2);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Theme Park', 180.00, '2024-05-26', 3);
                INSERT INTO Transactions (Title, Amount, DateTime, CategoryId) VALUES ('Flight Tickets', 400.00, '2024-05-30', 4);";

            context.Database.ExecuteSqlRaw(rawSql);
        }
    }
}
