using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcBudget.Models;

public class CategoryViewModel
{
    public List<Transaction> Transactions { get; set; }
    public List<Category> Categories { get; set; }
    public int? SelectedCategoryId { get; set; }
    public string SearchString { get; set; }
    public DateTime? SelectedDate { get; set; }
}