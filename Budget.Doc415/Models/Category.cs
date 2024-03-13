using System.ComponentModel.DataAnnotations;

namespace Budget.Doc415.Models;

public class Category
{
    public int Id { get; set; }

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please enter a category name")]
    public required string Name { get; set; }

}
