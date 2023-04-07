namespace Budget.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IEnumerable<Transaction>? Transactions { get; set; }
}