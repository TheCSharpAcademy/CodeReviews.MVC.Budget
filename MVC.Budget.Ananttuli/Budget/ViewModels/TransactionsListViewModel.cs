using Budget.TransactionsModule.Models;

namespace Budget.ViewModels
{
    public class TransactionsListViewModel
    {
        public List<Transaction> Transactions { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
