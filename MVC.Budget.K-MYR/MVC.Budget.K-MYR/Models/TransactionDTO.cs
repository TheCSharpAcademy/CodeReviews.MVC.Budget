
namespace MVC.Budget.K_MYR.Models;

public sealed class TransactionDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get;set; }
    public DateTime DateTime { get; set; }
    public string? Category { get; internal set; }
}
