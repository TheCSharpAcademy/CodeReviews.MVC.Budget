using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Budget.K_MYR.Models;

public class HomeViewModel
{
    public IEnumerable<Category> Income { get; set; }
    public IEnumerable<Category> Expenses { get; set; }
    public IEnumerable<Category> Savings { get; set; }
    public CategoryPost Category { get; set; }
    public TransactionPost Transaction { get; set; }    
    public SelectList Cultures { get; set; }
}
