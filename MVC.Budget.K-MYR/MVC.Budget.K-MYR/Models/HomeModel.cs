using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Budget.K_MYR.Models;

public class HomeModel
{
    public List<Category> Income { get; set; }
    public List<Category> Expenses { get; set; }
    public List<Category> Savings { get; set; }
    public SelectList Categories { get; set; }
    public Category Category { get; set; }
    public Transaction Transaction { get; set; }    
    public SearchModel Search { get; set; }
}
