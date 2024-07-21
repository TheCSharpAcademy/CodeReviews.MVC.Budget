using System.ComponentModel.DataAnnotations;

namespace Budget.CategoriesModule.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }
}