using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBudgetCarDioLogic.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string TransactionName { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Ammount { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please select a transaction type.")]
        [EnumDataType(typeof(TransactionTypes))]
        public TransactionTypes TransactionType { get; set; }
        public int CategoryId { get;set; }
        public string CategoryName { get; set; }
        public Category Category { get; set; }
    }

    public enum TransactionTypes
    {
        [Display(Name = "Income")]
        Income,
        [Display(Name = "Expense")]
        Expense
    }
}
