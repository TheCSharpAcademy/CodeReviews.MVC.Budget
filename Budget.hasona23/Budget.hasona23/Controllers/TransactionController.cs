using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget.hasona23.Models;
using Budget.hasona23.Services;

namespace Budget.hasona23.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        public TransactionController(ITransactionService transactionService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
        }

        // GET: Transaction
        public async Task<IActionResult> Index(string? searchString, int? categoryId, DateTime? minDate,
            decimal? maxPrice, decimal? minPrice)
        {
            var getAllTransactions = _transactionService.GetAllTransactionsAsync();
            List<TransactionModel> transactions = await getAllTransactions;
            var sorted = transactions.OrderByDescending(t1 => t1.Date).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                ViewData["SearchString"] = searchString;
                sorted = sorted.Where(t1 => t1.Details.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (minPrice.HasValue)
            {
                ViewData["MinPrice"] = minPrice;
                sorted = sorted.Where(t1 => t1.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                ViewData["MaxPrice"] = maxPrice;
                sorted = sorted.Where(t1 => t1.Price <= maxPrice.Value).ToList();
            }

            if (minDate.HasValue)
            {
                ViewData["MinDate"] = minDate;
                sorted = sorted.Where(t1 => t1.Date >= DateOnly.FromDateTime(minDate.Value)).ToList();
            }

            if (categoryId.HasValue && categoryId != -1)
            {
                ViewData["CategoryId"] = categoryId;
                sorted = sorted.Where(t1 => t1.Category.Id == categoryId).ToList();
            }
            ViewData["Categories"] = await _categoryService.GetAllCategoriesAsync();
            return View(sorted);
        }


        // GET: Transaction/Create
        public async Task<IActionResult> Create()
        {
            List<CategoryModel> categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["Categories"] = categories;
            return PartialView(
                new TransactionDto(DateOnly.FromDateTime(DateTime.Today)
                    , 0, "", categories[0].Id));
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionDto transactionModel)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.AddTransactionAsync(transactionModel);
                return Json(new { success = true });
            }

            return PartialView(transactionModel);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionModel = await _transactionService.GetTransactionByIdAsync((int)id);
            if (transactionModel == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = await _categoryService.GetAllCategoriesAsync();
            return PartialView(
                new TransactionDto(transactionModel.Date, transactionModel.Price, transactionModel.Details,
                    transactionModel.Category.Id)
            );
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionDto transactionModel)
        {
            if (!TransactionModelExists(id))
            {
                return NotFound();
            }

            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(modelError.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _transactionService.UpdateTransactionAsync(id, transactionModel);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"DbUpdateConcurrencyException occured {ex}");
                }

                return Json(new { success = true });
            }

            return PartialView(transactionModel);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionModel = await _transactionService.GetTransactionByIdAsync((int)id);
            if (transactionModel == null)
            {
                return NotFound();
            }

            return PartialView(transactionModel);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionModel = await _transactionService.GetTransactionByIdAsync(id);
            if (transactionModel != null)
            {
                await _transactionService.DeleteTransactionAsync(id);
            }

            return Json(new { success = true });
        }

        private bool TransactionModelExists(int id)
        {
            return _transactionService.GetTransactionById(id) != null;
        }
    }
}