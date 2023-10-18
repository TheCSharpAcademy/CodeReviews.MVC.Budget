namespace MVCBudget.Forser.Models
{
    public class CreateTransactionDTO
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a transaction name")]
        [RegularExpression(@"^[a-zA-Z0-9\s.\-']{2,}$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required, Precision(10, 2)]
        [DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal TransferredAmount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }
        public List<Category> Categories { get; set; }
    }
}