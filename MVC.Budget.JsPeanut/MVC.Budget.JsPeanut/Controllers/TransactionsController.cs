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

        public Category chosenCategory { get; set; }

        //public IActionResult GetAllTransactions()
        //{
        //    _service.GetAllTransactions();

        //    return Redirect("https://localhost:7229/");
        //}
        public IActionResult Index(int id, string name, string imageurl)
        {
            var transactions = _transactionService.GetAllTransactions().Where(x => x.CategoryId == id).ToList();
            var transactionViewModel = new TransactionViewModel
            {
                Transactions = transactions
            };

            ViewBag.ImageUrl = imageurl;
            ViewBag.Message = name;

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
