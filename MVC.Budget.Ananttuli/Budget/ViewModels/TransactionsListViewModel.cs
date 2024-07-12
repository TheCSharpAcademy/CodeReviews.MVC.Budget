using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Budget.TransactionsModule.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.ViewModels;

public class TransactionsListViewModel
{
    public List<Transaction> Transactions { get; set; } = new();
    public string? SearchText { get; set; }
    public int? SearchCategoryId { get; set; }
    public SelectList CategoriesList { get; set; }

    [Range(1, int.MaxValue)]
    public int? PageNumber { get; set; }

    public int Total { get; set; }

    public int TotalPages { get; set; }

    public SelectList PageNumbersList { get; set; }

    [Range(1, 100)]
    public int? PageSize { get; set; }

    [DataType(DataType.Date)]
    [DisplayName("Date From")]
    public DateOnly? StartDateRange { get; set; }

    [DataType(DataType.Date)]
    [DisplayName("Date To")]
    public DateOnly? EndDateRange { get; set; }
}