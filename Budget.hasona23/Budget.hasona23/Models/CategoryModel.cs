using System.ComponentModel.DataAnnotations;

namespace Budget.hasona23.Models;

public class CategoryModel
{
    public int Id { get; set; }
    [StringLength(32,MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    //Navigation
    public ICollection<TransactionModel> Transactions { get; set; }
}
public record CategoryDto(string Name);