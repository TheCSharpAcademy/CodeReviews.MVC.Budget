using Budget.CategoriesModule.Models;

namespace Budget.TransactionsModule.Models;

public class TransactionsViewModel
{
    public List<Transaction> Transactions { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public TransactionsViewModelActiveTab ActiveTab { get; set; } = TransactionsViewModelActiveTab.Transactions;
}

public enum TransactionsViewModelActiveTab
{
    Transactions,
    Categories
}