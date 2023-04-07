using Microsoft.EntityFrameworkCore;

namespace Budget.Models;

public class Wallet
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    [Precision(10, 2)]
    public decimal Income { get; set; }

    [Precision(10, 2)]
    public decimal? Expenses { get; set; } = 0M;
    
    [Precision(10, 2)]
    public decimal? Balance { get; set; } = 0M;

    public IEnumerable<Transaction>? Transactions { get; set; }
}