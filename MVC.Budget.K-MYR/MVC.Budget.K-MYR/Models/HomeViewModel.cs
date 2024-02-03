namespace MVC.Budget.K_MYR.Models;

public class HomeViewModel
{
    public List<Category> Income { get; set; }
    public List<Category> Expenses { get; set; }
    public List<Category> Savings { get; set; }
    public PostCategory Category { get; set; }
}
