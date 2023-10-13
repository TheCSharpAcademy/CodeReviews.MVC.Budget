using Microsoft.AspNetCore.Mvc;
using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models;
using System.Transactions;

namespace MVC.Budget.JsPeanut.Services
{
    public class TransactionService : ITransactionSerivce
    {
        private readonly DataContext _context;
        public TransactionService(DataContext context) 
        {
            _context = context;
        }
        public List<Models.Transaction> GetAllTransactions()
        {
            var transactions = _context.Transactions.ToList();

            return transactions;
        }


        public void AddTransaction(Models.Transaction transaction)
        {
            _context.Transactions.Add(transaction);

            _context.SaveChanges();
        }

        public void DeleteTransaction(Models.Transaction transaction)
        {
            _context.Transactions.Remove(transaction);

            _context.SaveChanges();
        }

        public void UpdateTransaction(Models.Transaction transaction)
        {
            _context.Transactions.Update(transaction);

            _context.SaveChanges();
        }
    }
}
