namespace MVCBudget.Forser.Models
{
    public class UserWallet
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } = Guid.NewGuid();
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter your name")]
        [RegularExpression(@"^[a-zA-Z\s.\-']{2,}$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; } = string.Empty;
        [Required, Precision(10, 2)]
        [DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        public int? TransactionsId { get; set; }
        public IList<Transaction>? Transactions { get; set; } = null!;
    }
}