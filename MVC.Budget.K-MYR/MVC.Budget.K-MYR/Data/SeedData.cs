using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Data;
public static class SeedData
{
    public static readonly Random Random = new();
    public static readonly DateTime Now = DateTime.UtcNow;
    public static void InitializeDatabase(DatabaseContext context)
    {
        context.Database.EnsureCreated();  
        
        if(context.FiscalPlans.Any())
        {
            return;
        }

        List<IncomeCategory> incomeCategories =
        [
            new()
            {
                Name = "Salary",
                Budget = 1600,
                Transactions = GenerateTransactions(1600, 1600, 1, ["Salary"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 1600
                    }
                ]
            },
            new()
            {
                Name = "Gifts",
                Budget = 50,
                Transactions = GenerateTransactions(10, 30, 2, ["Gift From Grandma", "Gift From Grandpa", "Gift From Friends","Gift From Parents"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 50
                    }
                ]
            },
            new()
            {
                Name = "Side Hustle",
                Budget = 500,
                Transactions = GenerateTransactions(200, 600, 1, ["Day Trading"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(0, 12)),
                        Budget = 500
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(12, 24)),
                        Budget = 400
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(24, 35)),
                        Budget = 300
                    },
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 200
                    }
                ]
            }
        ];

        List<ExpenseCategory> expenseCategories =
        [
           new()
            {
                Name = "Rent",
                Budget = 1000,
                Transactions = GenerateTransactions(1000, 1000, 1, ["Rent"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 1000
                    }
                ]
            },
            new()
            {
                Name = "Heating & Electricity",
                Budget = 100,
                Transactions = GenerateTransactions(70, 120, 1, ["Heating & Electricity"]), 
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(0, 12)),
                        Budget = 100
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(12, 36)),
                        Budget = 90
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(24, 35)),
                        Budget = 80
                    },
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 70
                    }
                ]
            },
            new()
            {
                Name = "Groceries",
                Budget = 300,
                Transactions = GenerateTransactions(20, 40, 12, ["Billa", "Spar", "Aldi", "Costco", "METRO", "Walmart"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(0, 12)),
                        Budget = 300
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(12, 36)),
                        Budget = 290
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(24, 35)),
                        Budget = 280
                    },
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 270
                    }
                ]
            },
            new()
            {
                Name = "Insurance",
                Budget = 300,
                Transactions = GenerateTransactions(250, 350, 1, ["Car Insurance"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(0, 12)),
                        Budget = 300
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(12, 36)),
                        Budget = 250
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(24, 35)),
                        Budget = 220
                    },
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 200
                    }
                ]
            },
            new()
            {
                Name = "Leisure",
                Budget = 300,
                Transactions = GenerateTransactions(5, 30, 10, ["Pub", "Cinema", "Barbecue", "Shopping"]),
                PreviousBudgets =
                [
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(0, 12)),
                        Budget = 300
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(12, 36)),
                        Budget = 250
                    },
                    new()
                    {
                        Month = Now.AddMonths(-Random.Next(24, 35)),
                        Budget = 220
                    },
                    new()
                    {
                        Month = Now.AddMonths(-36),
                        Budget = 200
                    }
                ]
            },
        ];

        FiscalPlan fiscalPlan = new()
        {
            Name = "My Daily Budget",
            ExpenseCategories = expenseCategories,
            IncomeCategories = incomeCategories,
        };

        context.FiscalPlans.Add(fiscalPlan);
        context.SaveChanges();
    }

    public static List<Transaction> GenerateTransactions(int minValue, int maxValue, int countPerMonth, string[] titles)
    {
        List<Transaction> transactions = [];

        for (int i = 0; i < 36; i++)
        {
            for (int j = 0; j < countPerMonth; j++)
            {
                string title = titles[Random.Next(0, titles.Length)];
                bool isEvaluated = Random.Next(0, 3 + i / 6) >= 1;

                transactions.Add(new Transaction
                {
                    Title = title,
                    Amount = Random.Next(minValue, maxValue),
                    DateTime = Now.AddMonths(-i).AddDays(-j),
                    IsHappy = Random.Next(0, 2) == 1,
                    IsNecessary = Random.Next(0, 2) == 1,
                    Evaluated = isEvaluated,
                    PreviousIsHappy = isEvaluated && Random.Next(0, 2) == 1,
                    PreviousIsNecessary = isEvaluated && Random.Next(0, 2) == 1
                });
            }
        }

        return transactions;
    }
}
