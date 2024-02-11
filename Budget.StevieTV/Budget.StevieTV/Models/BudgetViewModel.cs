namespace Budget.StevieTV.Models;

public class BudgetViewModel
{
    public List<Category> Categories { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}