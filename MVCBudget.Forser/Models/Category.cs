namespace MVCBudget.Forser.Models
{
    public class Category
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a category name")]
        [RegularExpression(@"^[a-zA-Z\s.\-']{2,}$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; }
        public int? TransactionsId { get; set; }
        public IList<Transaction>? Transactions { get; set; }
    }
}