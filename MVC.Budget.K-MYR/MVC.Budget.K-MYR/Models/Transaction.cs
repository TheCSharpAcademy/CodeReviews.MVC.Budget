using System.ComponentModel.DataAnnotations;

namespace MVC.Budget.K_MYR.Models;
public class Transaction
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "'Title' must be between 1 and 50 characters.")]
    public string Title { get; set; } = string.Empty;
    [StringLength(200, MinimumLength = 0, ErrorMessage = "'Description' must be between 1 and 200 characters.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "'Description' cannot consist of only whitespaces.")]
    public string? Description { get; set; }
    [Display(Name = "Date & Time")]
    [DataType(DataType.DateTime)]
    public DateTime DateTime { get; set; }
    [DataType(DataType.Currency)]
    [Range(0.0, 79_228_162_514_264_337_593_543_950_335.0, MinimumIsExclusive = true, ErrorMessage = $"'Amount' must be between 0 and 79,228,162,514,264,337,593,543,950,335.")]
    public decimal Amount { get; set; }
    public bool IsHappy { get; set; }
    public bool IsNecessary { get; set; }
    public bool IsEvaluated { get; set; }
    public bool PreviousIsHappy { get; set; }
    public bool PreviousIsNecessary { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
