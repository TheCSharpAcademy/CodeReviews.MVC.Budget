namespace MVC.Budget.frockett.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}
