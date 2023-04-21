using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Models;

public class WalletCategoryTransactionViewModel
{
    public SelectList? Wallets { get; set; }
    public SelectList? CategoriesSelectList { get; set; }
    public IEnumerable<Transaction>? Transactions { get; set; }

    public Wallet? Wallet { get; set; }
    public int WalletId { get; set; }
    public Category? Category { get; set; }
    public Transaction? Transaction { get; set; }
}