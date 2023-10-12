using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models;
using System.Transactions;

namespace MVC.Budget.JsPeanut.Services
{
    public class TransactionService
    {
        private readonly DataContext _context;
        public TransactionService(DataContext context) 
        {
            _context = context;
        }
        public List<Models.Transaction> GetAll()
        {
            var transactions = _context.Transactions.ToList();

            return transactions;
        }
        public void Add()
        {

        }
    }
}
