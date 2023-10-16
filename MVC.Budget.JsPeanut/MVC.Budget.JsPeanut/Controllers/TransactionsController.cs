using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models;
using MVC.Budget.JsPeanut.Models.ViewModel;
using MVC.Budget.JsPeanut.Services;
using System.Text.Json;

namespace MVC.Budget.JsPeanut.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly DataContext _context;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;
        private readonly JsonFileCurrencyService _jsonFileCurrencyService;
        private readonly CurrencyConverterService _currencyConverterService;
        public TransactionsController(DataContext context, CategoryService categoryService, TransactionService transactionService, JsonFileCurrencyService jsonFileCurrencyService, CurrencyConverterService currencyConverterService)
        {
            _context = context;
            _categoryService = categoryService;
            _transactionService = transactionService;
            _jsonFileCurrencyService = jsonFileCurrencyService;
            _currencyConverterService = currencyConverterService;
        }

        public IActionResult Index(int id = -1, string name = "", string imageurl = "")
        {
            List<Transaction> transactions = _transactionService.GetAllTransactions();
            List<Transaction> transactionsToShow = new();
            var categories = _context.Categories.ToList();
            var currencies = _jsonFileCurrencyService.GetCurrencyList();
            var categoryselectlist_ = new List<SelectListItem>();
            var currencyselectlist_ = new List<SelectListItem>();
            ViewBag.ImageUrl = imageurl;
            ViewBag.Category = name;

            foreach (var category in categories)
            {
                categoryselectlist_.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            foreach (var currency in currencies)
            {
                currencyselectlist_.Add(new SelectListItem
                {
                    Text = $"{currency.Name} ({currency.CurrencyCode})",
                    Value = /*currency.NativeSymbol */JsonSerializer.Serialize(currency)
                });
            }
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
                Categories = _categoryService.GetAllCategories(),
                CategorySelectList = categoryselectlist_,
                CurrencySelectList = currencyselectlist_
            };
            return View(transactionViewModel);
        }

        public IActionResult UpdateTransaction(Models.Transaction transaction, CategoryViewModel cvm)
        {
            var existingTransaction = _context.Transactions.Find(transaction.Id);
            if (existingTransaction != null)
            {
                existingTransaction.Date = transaction.Date;
                existingTransaction.Name = transaction.Name;
                existingTransaction.CategoryId = transaction.CategoryId;
                existingTransaction.Value = transaction.Value;

                var categories = _categoryService.GetAllCategories();
                var transactionCategory = categories.Where(c => c.Id == transaction.CategoryId).First();
                transactionCategory.TotalValue += transaction.Value;

                var selectedCurrencyOption = JsonSerializer.Deserialize<Currency>(cvm.CurrencyObjectJson);
                existingTransaction.CurrencyCode = selectedCurrencyOption.CurrencyCode;
                existingTransaction.CurrencyNativeSymbol = selectedCurrencyOption.NativeSymbol;

                _context.SaveChanges();
            }

            return Redirect("https://localhost:7229");
        }

        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            var transaction = _transactionService.GetAllTransactions().Find(x => x.Id == id);

            _transactionService.DeleteTransaction(transaction);

            //return Redirect("https://localhost:7229");

            return RedirectToAction("Index", "Categories");
        }
    }
}
