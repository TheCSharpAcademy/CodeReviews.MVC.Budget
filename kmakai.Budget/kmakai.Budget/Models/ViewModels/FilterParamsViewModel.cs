using System.ComponentModel.DataAnnotations;

namespace kmakai.Budget.Models.ViewModels;

public class FilterParamsViewModel
{
    [Display(Name = "Category")]
    public int? CategoryId { get; set; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }
}
