using Budget.CategoriesModule.Models;

namespace Budget.ViewModels;

public class HomeViewModel
{
    public TransactionsListViewModel TransactionList { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public TransactionsViewModelActiveTab ActiveTab { get; set; } = TransactionsViewModelActiveTab.Transactions;
}

public enum TransactionsViewModelActiveTab
{
    Transactions,
    Categories
}