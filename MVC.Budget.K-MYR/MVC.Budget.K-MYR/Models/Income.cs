namespace MVC.Budget.K_MYR.Models;

public class Income
{
    public int Id { get; set; }
    public ICollection<Category> Categories { get; set; }
}
