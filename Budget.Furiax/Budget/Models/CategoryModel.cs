using System.ComponentModel.DataAnnotations;

namespace Budget.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public ICollection<TransactionModel> Transactions { get; } = new List<TransactionModel>();
    }
}
