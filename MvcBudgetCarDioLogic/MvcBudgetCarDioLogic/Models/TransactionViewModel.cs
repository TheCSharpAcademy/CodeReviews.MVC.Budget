using MvcBudgetCarDioLogic.Data;
using System.ComponentModel.DataAnnotations;

namespace MvcBudgetCarDioLogic.Models
{
    public class TransactionViewModel
    {
        private readonly MvcBudgetCarDioLogicContext _context;

        public TransactionViewModel(MvcBudgetCarDioLogicContext context)
        {
            _context = context;
        }

        public IEnumerable<Transaction> Transactions { get; set; } //= new List<Transaction>();
        public IEnumerable<Category> Categories { get; set; } //= new List<Category>();
        public Transaction Transaction { get; set; }
        public FilterProperties FilterProperties { get; set; }
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance
        {
            get
            {
                var incomeTotal = _context.Transactions.Where(t => t.TransactionType == TransactionTypes.Income)
                                             .Sum(t => t.Ammount);

                var expenseTotal = _context.Transactions.Where(t => t.TransactionType == TransactionTypes.Expense)
                                              .Sum(t => t.Ammount);

                // Calculate the difference
                return (decimal)(incomeTotal - expenseTotal);
            }
        }
    }
}
