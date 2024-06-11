using Budget.Doc415.Models;
using Budget.Doc415.Services;

namespace Budget.Doc415.Data
{
    public class Seeder
    {
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;

        public Seeder(CategoryService categoryService, TransactionService transactionService)
        {
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        public async void SeedDb()
        {
            if (!_transactionService.IsSeeded())
            {
                string[] Categories = ["Education", "Food", "Car", "Bills"];
                for (int i = 0; i < Categories.Length; i++)
                {
                    var nametext = Categories[i];
                    _categoryService.InsertCategory(new Category { Name = Categories[i] });
                }


                string[] Names = ["Book", "Lunch", "Car repair", "Water bill", "Kids school payment", "Plane ticket", "Gift"];
                Random random = new Random();
                List<Transaction> transactions = new();
                for (int i = 0; i < 10; i++)
                {
                    var transaction = new TransactionViewModel()
                    {
                        Date = DateTime.Now - TimeSpan.FromDays(random.Next(50)),
                        Name = Names[random.Next(0, Names.Length)],
                        Amount = random.Next(0, 3222),
                        Category = Categories[random.Next(0, Categories.Length)]
                    };
                    _transactionService.InsertTransaction(transaction);
                }
            }
        }
    }
}
