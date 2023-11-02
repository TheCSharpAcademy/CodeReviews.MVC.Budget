namespace MVCBudget.Forser.Models
{
    public class UserWallet
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter your name")]
        [RegularExpression(@"^[a-zA-Z\s.\-']{2,}$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; } = string.Empty;
        public int? TransactionsId { get; set; }
        public IList<Transaction>? Transactions { get; set; } = null!;
    }
}