using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBudgetCarDioLogic.Data;
using MvcBudgetCarDioLogic.Models;
using System.Diagnostics.Metrics;

namespace MvcBudgetCarDioLogic.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly MvcBudgetCarDioLogicContext _context;

        public TransactionsController(MvcBudgetCarDioLogicContext context)
        {
            _context = context;
        }


        public IActionResult Index(FilterProperties? filterProperties)
        {
            List<Transaction> transactions = _context.Transactions.ToList();

            if (filterProperties != null && filterProperties.FilterId != 0)
            {
                if (filterProperties.StartDate != null)
                {
                    if (filterProperties.EndDate != null)
                    {
                        transactions = transactions
                            .Where(t => t.CategoryId == filterProperties.FilterId &&
                                        t.Date >= filterProperties.StartDate &&
                                        t.Date <= filterProperties.EndDate)
                            .ToList();
                    }
                    else
                    {
                        transactions = transactions
                            .Where(t => t.CategoryId == filterProperties.FilterId &&
                                        t.Date >= filterProperties.StartDate)
                            .ToList();
                    }
                }
                else
                {
                    transactions = transactions
                        .Where(t => t.CategoryId == filterProperties.FilterId)
                        .ToList();
                }
            }
            else if (filterProperties != null && filterProperties.StartDate != null)
            {
                if (filterProperties.EndDate != null)
                {
                    transactions = transactions
                        .Where(t => t.Date >= filterProperties.StartDate &&
                                    t.Date <= filterProperties.EndDate)
                        .ToList();
                }
                else
                {
                    transactions = transactions
                        .Where(t => t.Date >= filterProperties.StartDate)
                        .ToList();
                }
                return View(transactions);
            }


        

        TransactionViewModel transactionViewModel = new TransactionViewModel(_context)
            {
                Transactions = transactions.OrderByDescending(t => t.Date),
                Categories = _context.Categories.ToList()
            };

            return View(transactionViewModel);
        }


        [HttpPost]
        public IActionResult Create(Transaction transaction)
        {

            Category category = _context.Categories.Where(c => c.Id == transaction.CategoryId).FirstOrDefault();

            Transaction transactionToAdd = new Transaction()
            {
                TransactionName = transaction.TransactionName,
                CategoryId = category.Id,
                CategoryName = category.CategoryName,
                Ammount = transaction.Ammount,
                TransactionType = transaction.TransactionType,
                Date = transaction.Date,
                Category = category,
            };

            _context.Add(transactionToAdd);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public  IActionResult Edit(Transaction transaction)
        {

            Category category = _context.Categories.Where(c => c.Id == transaction.CategoryId).FirstOrDefault();

            Transaction transactionToEdit = new Transaction()
            {
                Id = transaction.Id,
                TransactionName = transaction.TransactionName,
                CategoryId = category.Id,
                CategoryName = category.CategoryName,
                TransactionType = transaction.TransactionType,
                Ammount = transaction.Ammount,
                Date = transaction.Date,
                Category = category,
            };

            _context.Update(transactionToEdit);
            _context.SaveChanges();
                
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteTransiction(int Id)
        {
            var transactionToDel = _context.Transactions.Find(Id);
            if (Id != null)
            {
                _context.Transactions.Remove(transactionToDel);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
