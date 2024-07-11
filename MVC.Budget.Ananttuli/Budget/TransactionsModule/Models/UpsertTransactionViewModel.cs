using Budget.TransactionsModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.CategoriesModule.Models;

public class UpsertTransactionViewModel
{
    public Transaction Transaction { get; set; }

    public SelectList AllCategories { get; set; }
}