namespace Budget.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TransactionSource { get; set; }
        public decimal TransactionAmount { get; set; }

        public CategoryModel Category { get; set; }
    }
}
