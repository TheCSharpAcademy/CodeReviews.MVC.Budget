namespace MVCBudget.Forser.Models
{
    public class TransactionDTO
    {
        public int WalletId { get; set; }
        public string? WalletName { get; set; }
        public string? CategoryName { get; set; }
        public List<string>? TransactionName { get; set; }
        public List<decimal>? TransactionAmount { get; set; }
        public List<DateTime>? TransactionDate { get; set; }
    }
}