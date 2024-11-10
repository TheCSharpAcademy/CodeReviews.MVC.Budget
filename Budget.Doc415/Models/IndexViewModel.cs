using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Doc415.Models;

public class IndexViewModel
{
    public List<TransactionViewModel>? Transactions { get; set; }
    public List<Category>? Categories { get; set; }
    public TransactionViewModel? NewTransaction { get; set; }
    public Category? NewCategory { get; set; }
    public string? SearchName { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public SelectList? CategoriesSelect { get; set; }
    public string? FilterCategory { get; set; }
}
