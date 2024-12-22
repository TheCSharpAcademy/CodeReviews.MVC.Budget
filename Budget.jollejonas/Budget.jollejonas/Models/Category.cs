using System.ComponentModel.DataAnnotations;

namespace Budget.jollejonas.Models;

public class Category
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
