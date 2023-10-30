using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MvcBudgetCarDioLogic.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string CategoryName { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
