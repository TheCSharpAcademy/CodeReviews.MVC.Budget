using Budget.TransactionsModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.ViewModels
{
    public class TransactionsListViewModel
    {
        public List<Transaction> Transactions { get; set; } = new();
        public string? SearchText { get; set; }
        public int? SearchCategoryId { get; set; }
        public SelectList CategoriesList { get; set; }
    }
}
