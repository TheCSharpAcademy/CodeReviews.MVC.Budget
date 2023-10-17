namespace Budget.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public ICollection<TransactionModel> Transactions { get; } = new List<TransactionModel>();
    }
}
