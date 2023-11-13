using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }
        [ForeignKey("CategoryId")]
        public ICollection<TransactionModel> Transactions { get; } = new List<TransactionModel>();
    }
}
