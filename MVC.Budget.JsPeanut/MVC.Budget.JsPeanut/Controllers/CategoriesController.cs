using Microsoft.AspNetCore.Mvc;
using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models;
using MVC.Budget.JsPeanut.Models.ViewModel;
using MVC.Budget.JsPeanut.Services;

namespace MVC.Budget.JsPeanut.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;
        public Transaction TransactionArgument { get; set; }
        public CategoriesController(DataContext context, CategoryService categoryService, TransactionService transactionService)
        {
            _context = context;
            _categoryService = categoryService;
            _transactionService = transactionService;
        }
        
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var transactions = _context.Transactions.ToList();
            var categoriesviewmodel = new CategoryViewModel
            {
                Categories = categories,
                Transactions = transactions
            };
            return View(categoriesviewmodel);
        }

        public IActionResult AddTransaction(Models.Transaction transaction)
        {
            _transactionService.AddTransaction(transaction);

            var category = _categoryService.GetAllCategories().Where(c => c.Id == transaction.CategoryId).First();

            category.TotalValue += transaction.Value;

            _context.SaveChanges();

            return Redirect("https://localhost:7229");
        }

        public IActionResult ManageTransactions()
        {
            return View("~/Views/Transactions/Index.cshtml");
        }
        //public void GetAllCategories()
        //{
        //}
    }
}
