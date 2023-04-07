using Microsoft.EntityFrameworkCore;

namespace Budget.Models;

public class Transaction
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Month? Month { get; set; }
    public DateTime? Date { get; set; }

    [Precision(10, 2)]
    public decimal? Cost { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int WalletId { get; set; }
    public Wallet? Wallet { get; set; }
}