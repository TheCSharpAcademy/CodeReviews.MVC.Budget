using Microsoft.AspNetCore.Mvc;
using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models;
using MVC.Budget.JsPeanut.Models.ViewModel;
using MVC.Budget.JsPeanut.Services;

namespace MVC.Budget.JsPeanut.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly DataContext _context;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;
        public TransactionsController(DataContext context, CategoryService categoryService, TransactionService transactionService) 
        {
            _context = context;
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        public IActionResult Index(int id = -1, string name = "", string imageurl = "")
        {
            List<Transaction> transactions = _transactionService.GetAllTransactions();
            List<Transaction> transactionsToShow = new();
            ViewBag.ImageUrl = imageurl;
            ViewBag.Category = name;

            if (id == -1)
            {
                transactionsToShow = transactions;
                ViewBag.ImageUrl = null;
                ViewBag.Category = null;
            }
            else
            {
                transactionsToShow = _transactionService.GetAllTransactions().Where(x => x.CategoryId == id).ToList();
            }

            var transactionViewModel = new TransactionViewModel
            {
                Transactions = transactionsToShow,
                Categories = _categoryService.GetAllCategories()
            };
            return View(transactionViewModel);
        }

        public IActionResult AddTransaction(Models.Transaction transaction)
        {
            _context.Add(transaction);

            var category = _categoryService.GetAllCategories().Where(c => c.Id == transaction.CategoryId).First();

            category.TotalValue += transaction.Value;

            _context.SaveChanges();

            return Redirect("https://localhost:7229/");
        }

        public IActionResult ManageTransactions()
        {
            return View("~/Views/Transactions/Index.cshtml");
        }
    }
}
