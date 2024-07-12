using Budget.TransactionsModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Budget.ViewModels
{
    public class TransactionsListViewModel
    {
        public List<Transaction> Transactions { get; set; } = new();
        public string? SearchText { get; set; }
        public int? SearchCategoryId { get; set; }
        public SelectList CategoriesList { get; set; }

        [Range(1, int.MaxValue)]
        public int? PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int? PageSize { get; set; } = 25;

        public int Total { get; set; }
        
        public int TotalPages { get; set; }
        
        public SelectList PageNumbersList { get; set; }
    }
}
