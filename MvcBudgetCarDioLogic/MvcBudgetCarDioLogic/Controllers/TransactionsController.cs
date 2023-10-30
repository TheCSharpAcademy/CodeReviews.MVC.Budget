using Microsoft.AspNetCore.Mvc;
using MvcBudgetCarDioLogic.Data;
using MvcBudgetCarDioLogic.Models;

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

            if (filterProperties.StartDate != null || filterProperties.EndDate != null || filterProperties.FilterCategoryId != 0)
            {
                if (filterProperties.StartDate != null && filterProperties.EndDate == null)
                {
                    if (filterProperties.FilterCategoryId != 0)
                    {
                        transactions = transactions
                        .Where(t => t.CategoryId == filterProperties.FilterCategoryId &&
                                    t.Date >= filterProperties.StartDate).ToList();
                    }
                    else
                    {
                        transactions = transactions
                        .Where(t => t.Date >= filterProperties.StartDate).ToList();
                    }
                }
                if (filterProperties.StartDate == null && filterProperties.EndDate != null)
                {
                    if (filterProperties.FilterCategoryId != 0)
                    {
                        transactions = transactions
                        .Where(t => t.CategoryId == filterProperties.FilterCategoryId &&
                                    t.Date <= filterProperties.EndDate).ToList();
                    }
                    else
                    {
                        transactions = transactions
                        .Where(t => t.Date <= filterProperties.EndDate).ToList();
                    }
                }
                if (filterProperties.StartDate != null && filterProperties.EndDate != null)
                {
                    if (filterProperties.FilterCategoryId != 0)
                    {
                        transactions = transactions
                        .Where(t => t.CategoryId == filterProperties.FilterCategoryId &&
                                t.Date >= filterProperties.StartDate &&
                                t.Date <= filterProperties.EndDate).ToList();
                    }
                    else
                    {
                        transactions = transactions
                        .Where(t => t.Date >= filterProperties.StartDate &&
                            t.Date <= filterProperties.EndDate).ToList();
                    }
                }
                else
                {
                    transactions = transactions
                    .Where(t => t.CategoryId == filterProperties.FilterCategoryId).ToList();
                }
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
        public IActionResult DeleteTransiction(Transaction transaction)
        {
            var transactionToDel = _context.Transactions.Find(transaction.Id);
            if (transaction.Id != null)
            {
                _context.Transactions.Remove(transactionToDel);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
