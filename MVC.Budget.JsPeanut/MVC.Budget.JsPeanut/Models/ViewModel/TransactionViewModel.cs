namespace MVC.Budget.JsPeanut.Models.ViewModel
{
    public class TransactionViewModel
    {
        public List<Transaction> Transactions { get; set; }

        public List<Category> Categories { get; set; }

        public Transaction Transaction { get; set; }
    }
}
