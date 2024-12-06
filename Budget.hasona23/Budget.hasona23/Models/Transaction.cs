namespace Budget.hasona23.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float Price { get; set; }
    public bool IsProfit { get; set; }
    public DateTime Date { get; set; }
}