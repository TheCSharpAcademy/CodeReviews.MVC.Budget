using System.ComponentModel.DataAnnotations;

namespace Budget.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set;} = "";
}