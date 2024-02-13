using System.ComponentModel.DataAnnotations;
using Budget.StevieTV.Enums;
using Microsoft.EntityFrameworkCore;

namespace Budget.StevieTV.Models;

public class BudgetViewModel
{
    public List<Category> Categories { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public TransactionViewModel TransactionViewModel { get; set; } = new TransactionViewModel();

    public CategoryViewModel CategoryViewModel { get; set; } = new CategoryViewModel();

    [DataType(DataType.Currency)]
    [Precision(18,2)]
    public decimal RunningTotal
    {
        get
        {
            var incomeSum = Transactions.Where(t => t.TransactionType == TransactionType.Income).Sum(t => t.Amount);
            var expensesSum = Transactions.Where(t => t.TransactionType == TransactionType.Expense).Sum(t => t.Amount);
            return incomeSum - expensesSum;
        }
    }
}