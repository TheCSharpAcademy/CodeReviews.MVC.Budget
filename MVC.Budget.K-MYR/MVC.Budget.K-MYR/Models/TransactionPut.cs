using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace MVC.Budget.K_MYR.Models;
[BindRequired]
public class TransactionPut
{
    [JsonRequired]
    public int Id { get; set; }
    [JsonRequired]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "'Title' must be between 1 and 50 characters.")]
    [Required]
    public string Title { get; set; } = string.Empty;
    [StringLength(200, MinimumLength = 0, ErrorMessage = "'Description' must be between 1 and 200 characters.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "'Description' cannot consist of only whitespaces.")]
    public string? Description { get; set; }
    [Display(Name = "Date & Time")]
    [DataType(DataType.DateTime)]
    [JsonRequired]
    public DateTime DateTime { get; set; }
    [DataType(DataType.Currency)]
    [Range(0.0, 100000000000000, ErrorMessage = $"'Amount' must be between 0 and 100000000000000.")] 
    [JsonRequired]
    public decimal Amount { get; set; }
    [JsonRequired]
    public bool IsHappy { get; set; }
    [JsonRequired]
    public bool IsNecessary { get; set; }
    [JsonRequired]
    public bool IsEvaluated { get; set; }
    [JsonRequired]
    public bool PreviousIsHappy { get; set; }
    [JsonRequired]
    public bool PreviousIsNecessary { get; set; }
}
