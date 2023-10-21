using MVCBudget.Forser.Helpers;

namespace MVCBudget.Forser.Data
{
    public class SeedData
    {
        public static async Task Initalize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Transactions.Any() || context.Categories.Any() || context.Wallets.Any()) 
                {
                    return;
                }

                var userwallet = new UserWallet()
                {
                    Id = 1,
                    Name = "User Wallet",
                    Amount = decimal.Parse("10000")
                };

                var categories = new Category[]
                {
                    new Category { Id = 1, Name = "Groceries" },
                    new Category { Id = 2, Name = "Restaurants" },
                    new Category { Id = 3, Name = "Gasoline" },
                    new Category { Id = 4, Name = "Utilities" },
                    new Category { Id = 5, Name = "Rent/Mortgage" },
                    new Category { Id = 6, Name = "Entertainment" },
                    new Category { Id = 7, Name = "Healthcare" },
                    new Category { Id = 8, Name = "Transportation" },
                    new Category { Id = 9, Name = "Clothing" },
                    new Category { Id = 10, Name = "Electronics" },
                    new Category { Id = 11, Name = "Home Improvement" },
                    new Category { Id = 12, Name = "Gifts" },
                    new Category { Id = 13, Name = "Travel" },
                    new Category { Id = 14, Name = "Insurance" },
                    new Category { Id = 15, Name = "Education" },
                    new Category { Id = 16, Name = "Savings" },
                    new Category { Id = 17, Name = "Investments" },
                    new Category { Id = 18, Name = "Charity" },
                    new Category { Id = 19, Name = "Pets" },
                    new Category { Id = 20, Name = "Miscellaneous" }
                };

                var transactions = new Transaction[]
                {
                    new Transaction { Id = 1, Name = "Grocery Shopping", Description = "Weekly grocery expenses", TransferredAmount = decimal.Parse("-50,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-05-15"), CategoryId = 1, UserWalletId = 1 },
                    new Transaction { Id = 2, Name = "Dinner at Pizzeria", Description = "Dining out with friends", TransferredAmount = decimal.Parse("-30,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-08-10"), CategoryId = 2, UserWalletId = 1 },
                    new Transaction { Id = 3, Name = "Gas Refill", Description = "Car fuel refill", TransferredAmount = decimal.Parse("-40,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-03-22"), CategoryId = 3, UserWalletId = 1 },
                    new Transaction { Id = 4, Name = "Electric Bill", Description = "Monthly electricity bill", TransferredAmount = decimal.Parse("-70,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-04-05"), CategoryId = 4, UserWalletId = 1 },
                    new Transaction { Id = 5, Name = "Rent Payment", Description = "Monthly rent for apartment", TransferredAmount = decimal.Parse("-1200,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-09-01"), CategoryId = 5, UserWalletId = 1 },
                    new Transaction { Id = 6, Name = "Movie Tickets", Description = "Entertainment expenses", TransferredAmount = decimal.Parse("-25,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-07-12"), CategoryId = 6, UserWalletId = 1 },
                    new Transaction { Id = 7, Name = "Doctor's Visit", Description = "Medical check-up copay", TransferredAmount = decimal.Parse("-40,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-12-18"), CategoryId = 7, UserWalletId = 1 },
                    new Transaction { Id = 8, Name = "Bus Fare", Description = "Public transportation cost", TransferredAmount = decimal.Parse("-10,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-02-08"), CategoryId = 8, UserWalletId = 1 },
                    new Transaction { Id = 9, Name = "New Shoes", Description = "Footwear purchase", TransferredAmount = decimal.Parse("-60,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-06-30"), CategoryId = 9, UserWalletId = 1 },
                    new Transaction { Id = 10, Name = "Laptop Purchase", Description = "New laptop for work", TransferredAmount = decimal.Parse("-800,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-11-25"), CategoryId = 10, UserWalletId = 1 },
                    new Transaction { Id = 11, Name = "Home Repairs", Description = "Fixing plumbing issues", TransferredAmount = decimal.Parse("-150,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-02-12"), CategoryId = 11, UserWalletId = 1 },
                    new Transaction { Id = 12, Name = "Birthday Gift", Description = "Gift for a friend's birthday", TransferredAmount = decimal.Parse("-20,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-04-03"), CategoryId = 12, UserWalletId = 1 },
                    new Transaction { Id = 13, Name = "Weekend Getaway", Description = "Short trip with family", TransferredAmount = decimal.Parse("-300,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-07-08"), CategoryId = 13, UserWalletId = 1 },
                    new Transaction { Id = 14, Name = "Health Insurance", Description = "Monthly insurance premium", TransferredAmount = decimal.Parse("-80,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-10-15"), CategoryId = 14, UserWalletId = 1 },
                    new Transaction { Id = 15, Name = "Textbooks", Description = "Purchase of college textbooks", TransferredAmount = decimal.Parse("-150,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-09-05"), CategoryId = 15, UserWalletId = 1 },
                    new Transaction { Id = 16, Name = "Savings Deposit", Description = "Adding to savings account", TransferredAmount = decimal.Parse("200,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-10-10"), CategoryId = 16, UserWalletId = 1 },
                    new Transaction { Id = 17, Name = "Investment Fund", Description = "Investing in mutual funds", TransferredAmount = decimal.Parse("500,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-05-20"), CategoryId = 17, UserWalletId = 1 },
                    new Transaction { Id = 18, Name = "Charity Donation", Description = "Contribution to a local charity", TransferredAmount = decimal.Parse("-30,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-07-14"), CategoryId = 18, UserWalletId = 1 },
                    new Transaction { Id = 19, Name = "Pet Supplies", Description = "Buying food and toys for pets", TransferredAmount = decimal.Parse("-45,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-01-29"), CategoryId = 19, UserWalletId = 1 },
                    new Transaction { Id = 20, Name = "Miscellaneous", Description = "Random miscellaneous expense", TransferredAmount = decimal.Parse("-15,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-08-05"), CategoryId = 20, UserWalletId = 1 },
                    new Transaction { Id = 21, Name = "Coffee Shop", Description = "Morning coffee and pastries", TransferredAmount = decimal.Parse("-8,50", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-01-10"), CategoryId = 2, UserWalletId = 1 },
                    new Transaction { Id = 22, Name = "Grocery Shopping", Description = "Weekly grocery expenses", TransferredAmount = decimal.Parse("-60,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-11-20"), CategoryId = 1, UserWalletId = 1 },
                    new Transaction { Id = 23, Name = "Internet Bill", Description = "Monthly internet subscription", TransferredAmount = decimal.Parse("-55,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-06-05"), CategoryId = 4, UserWalletId = 1 },
                    new Transaction { Id = 24, Name = "Movie Tickets", Description = "Entertainment expenses", TransferredAmount = decimal.Parse("-20,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-03-15"), CategoryId = 6, UserWalletId = 1 },
                    new Transaction { Id = 25, Name = "Dentist Appointment", Description = "Dental check-up copay", TransferredAmount = decimal.Parse("-60,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-10-30"), CategoryId = 7, UserWalletId = 1 },
                    new Transaction { Id = 26, Name = "Subway Fare", Description = "Public transportation cost", TransferredAmount = decimal.Parse("-5,50", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-07-08"), CategoryId = 8, UserWalletId = 1 },
                    new Transaction { Id = 27, Name = "New Jacket", Description = "Purchase of winter jacket", TransferredAmount = decimal.Parse("-70,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-02-25"), CategoryId = 9, UserWalletId = 1 },
                    new Transaction { Id = 28, Name = "Smartphone Upgrade", Description = "Upgrading to the latest model", TransferredAmount = decimal.Parse("-700,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-04-10"), CategoryId = 10, UserWalletId = 1 },
                    new Transaction { Id = 29, Name = "Bathroom Renovation", Description = "Home improvement project", TransferredAmount = decimal.Parse("-200,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-05-12"), CategoryId = 11, UserWalletId = 1 },
                    new Transaction { Id = 30, Name = "Anniversary Gift", Description = "Gift for spouse's anniversary", TransferredAmount = decimal.Parse("-50,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-09-21"), CategoryId = 12, UserWalletId = 1 },
                    new Transaction { Id = 31, Name = "Beach Vacation", Description = "Summer vacation with family", TransferredAmount = decimal.Parse("-800,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-08-05"), CategoryId = 13, UserWalletId = 1 },
                    new Transaction { Id = 32, Name = "Car Insurance", Description = "Auto insurance premium", TransferredAmount = decimal.Parse("-90,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-06-28"), CategoryId = 14, UserWalletId = 1 },
                    new Transaction { Id = 33, Name = "School Supplies", Description = "Back-to-school shopping", TransferredAmount = decimal.Parse("-40,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-03-03"), CategoryId = 15, UserWalletId = 1 },
                    new Transaction { Id = 34, Name = "Investment Portfolio", Description = "Adding to investment portfolio", TransferredAmount = decimal.Parse("300,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-10-10"), CategoryId = 17, UserWalletId = 1 },
                    new Transaction { Id = 35, Name = "Home Decor", Description = "Purchase of home decor items", TransferredAmount = decimal.Parse("-35,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-12-07"), CategoryId = 20, UserWalletId = 1 },
                    new Transaction { Id = 36, Name = "Phone Bill", Description = "Monthly phone plan bill", TransferredAmount = decimal.Parse("-40,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-09-16"), CategoryId = 4, UserWalletId = 1 },
                    new Transaction { Id = 37, Name = "Taxi Ride", Description = "Ride to the airport", TransferredAmount = decimal.Parse("-25,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2021-11-14"), CategoryId = 8, UserWalletId = 1 },
                    new Transaction { Id = 38, Name = "Fitness Membership", Description = "Monthly gym membership", TransferredAmount = decimal.Parse("-60,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2023-07-29"), CategoryId = 15, UserWalletId = 1 },
                    new Transaction { Id = 39, Name = "Bookstore Purchase", Description = "New books and magazines", TransferredAmount = decimal.Parse("-45,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-10-19"), CategoryId = 6, UserWalletId = 1 },
                    new Transaction { Id = 40, Name = "Car Maintenance", Description = "Routine car maintenance", TransferredAmount = decimal.Parse("-80,00", NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint), TransactionDate = DateTime.Parse("2022-06-14"), CategoryId = 11, UserWalletId = 1 }
                };

                context.Wallets.Add(userwallet);
                await context.SaveChangesWithIdentityInsertAsync<UserWallet>();

                context.Categories.AddRange(categories);
                await context.SaveChangesWithIdentityInsertAsync<Category>();

                context.Transactions.AddRange(transactions);
                await context.SaveChangesWithIdentityInsertAsync<Transaction>();
            }

        }
    }
}