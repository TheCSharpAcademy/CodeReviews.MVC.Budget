using Data.Models;

namespace Budget.Models;

public class BudgetVM
{
    public List<Category> Categories { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public TransactionsVM TransactionViewModel { get; set; } = new();
    public CategoriesVM CategoryViewModel { get; set; } = new();
}