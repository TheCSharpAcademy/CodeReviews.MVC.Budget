namespace MVC.Budget.K_MYR.Models;

public class Group
{
    public int Id { get; set; }
    public ICollection<Category> Categories { get; set; }
}
