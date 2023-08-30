namespace kmakai.Budget.Models.ViewModels;

public class HomeViewModel
{
    public BalanceViewModel Balance { get; set; } = new BalanceViewModel();
    public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public IEnumerable<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
    public FilterParamsViewModel FilterParams { get; set; } = new FilterParamsViewModel();

    public AddCategoryViewModel AddCategory { get; set; } = new AddCategoryViewModel();
    public AddTransactionViewModel AddTransaction { get; set; } = new AddTransactionViewModel();

}

public class BalanceViewModel
{
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Balance => Income - Expense;
    
}