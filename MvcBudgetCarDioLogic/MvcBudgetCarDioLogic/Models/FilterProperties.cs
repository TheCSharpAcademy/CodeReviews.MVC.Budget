using System.ComponentModel.DataAnnotations;

namespace MvcBudgetCarDioLogic.Models
{
    public class FilterProperties
    {
        public int FilterId { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}
