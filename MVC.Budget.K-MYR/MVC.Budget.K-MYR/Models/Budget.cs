namespace MVC.Budget.K_MYR.Models;

public class Budget
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Group Income { get; set; }
    public Group Expense{ get; set; }
}
