using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models
{
    public class TransactionModel
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public string? TransactionSource { get; set; }
        [Required]
        public decimal TransactionAmount { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        public CategoryModel Category { get; set; } = null!;
    }
}
