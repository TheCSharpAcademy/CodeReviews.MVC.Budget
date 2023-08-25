using System.ComponentModel.DataAnnotations;

namespace kmakai.Budget.Models;

public class TransactionType
{
    public int Id { get; set; }

    public TypeName Name { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = null!;
}
public enum TypeName
{
    [Display(Name = "Income")]
    Income,
    [Display(Name = "Expense")]
    Expense
}