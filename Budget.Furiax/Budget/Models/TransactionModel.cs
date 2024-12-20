using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models
{
    public class TransactionModel
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string TransactionSource { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TransactionAmount { get; set; }

        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}
