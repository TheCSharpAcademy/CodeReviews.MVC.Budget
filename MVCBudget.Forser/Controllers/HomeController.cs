using Microsoft.AspNetCore.Mvc.Rendering;
using MVCBudget.Forser.Helpers;
using System.Diagnostics;

namespace MVCBudget.Forser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ILogger<HomeController> _logger;
        private IUserWalletRepository UserWalletRepository { get; }
        private ICategoryRepository CategoryRepository { get; }
        private ITransactionRepository TransactionRepository { get; }

        public HomeController(IDiagnosticContext diagnosticContext, ILogger<HomeController> logger,
            IUserWalletRepository userWalletRepository, ICategoryRepository categoryRepository, ITransactionRepository transactionRepository)
        {
            _diagnosticContext = diagnosticContext ??
                throw new ArgumentNullException(nameof(diagnosticContext));
            _logger = logger;
            _logger.LogInformation("HomeController called");
            UserWalletRepository = userWalletRepository;
            CategoryRepository = categoryRepository;
            TransactionRepository = transactionRepository;
        }

        public async Task<IActionResult> Index(FilterModel? filterModel, int? categoryName)
        {
            _diagnosticContext.Set("CatalogLoadTime", 1423);
            _logger.LogInformation($"{nameof(Index)} Starting.");

            _logger.LogInformation($"{nameof(UserWalletRepository)} is loaded");
            var wallets = await UserWalletRepository.GetUserWalletsAsync();
            _logger.LogInformation($"{nameof(CategoryRepository)}");
            var categories = await CategoryRepository.GetAllCategoriesAsync();
            _logger.LogInformation($"{nameof(TransactionRepository)}");
            var transactions = await TransactionRepository.GetAllTransactionsAsync();

            if (filterModel != null)
            {
                transactions = Filter(filterModel, categoryName);
            }

            var viewModel = new MainViewModel
            {
                UserWallets = wallets,
                Categories = categories,
                Transactions = transactions
            };

            _logger.LogInformation($"{wallets.Select(s => s.Name).FirstOrDefault()} wallet is loaded");
            return View(viewModel);
        }

        private List<Transaction> Filter(FilterModel? filterModel, int? categoryName)
        {
            var transactions = TransactionRepository.GetAllTransactions();

            if (categoryName == null && filterModel.StartDate == null && filterModel.EndDate == null)
            {
                return transactions;
            }

            if (categoryName != null && filterModel.StartDate == null)
            {
                transactions = transactions.Where(w => w.CategoryId == categoryName).ToList();
            }

            if (categoryName == null && filterModel.StartDate != null && filterModel.EndDate == null)
            {
                transactions = transactions.Where(w => w.TransactionDate >= filterModel.StartDate).ToList();
            }

            if (categoryName == null && filterModel.StartDate == null && filterModel.EndDate != null)
            {
                transactions = transactions.Where(w => w.TransactionDate >= filterModel.StartDate
                    && w.TransactionDate <= filterModel.EndDate).ToList();
            }

            if (categoryName != null && filterModel.StartDate != null)
            {
                transactions = transactions.Where(w => w.CategoryId == categoryName
                    && w.TransactionDate >= filterModel.StartDate
                    && w.TransactionDate <= filterModel.EndDate).ToList();
            }

            return transactions;
        }

        public async Task<IActionResult> CreateTransaction()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(Transaction transaction, int CategoryName)
        {
            if (transaction == null || CategoryName < 1)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.CategoryId = CategoryName;
                        transaction.UserWalletId = 1; // Currently, only one user can use this service hence hardcoded.

                        await TransactionRepository.CreateAsync(transaction);
                        await TransactionRepository.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't create the new Transaction: {transaction} with error {ex.Message}");
                    return View(transaction);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditTransaction(int? id)
        {
            var transaction = await TransactionRepository.GetTransactionById(id);
            var categories = await CategoryRepository.GetAllCategoriesAsync();
            var selectList = categories.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            foreach (var item in selectList)
            {
                if (item.Value == transaction.CategoryId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

            var editTransaction = new EditTransactionDTO
            {
                Id = transaction.Id,
                Name = transaction.Name,
                Description = transaction.Description,
                TransferredAmount = transaction.TransferredAmount,
                TransactionDate = transaction.TransactionDate,
                CategoryId = transaction.CategoryId,
                Categories = selectList
            };

            if (id == null || transaction == null)
            {
                _logger.LogInformation($"Transaction with ID: {id} wasn't found.");
                return NotFound();
            }

            return Json(new { html = Helper.RenderViewToString(this, "_EditTransactionPartial", editTransaction) });
        }

        [HttpPost]
        [ActionName("EditTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(EditTransactionDTO transaction, int categoryName)
        {
            var exisitingTransaction = await TransactionRepository.GetTransactionById(transaction.Id);

            if (exisitingTransaction == null || transaction == null)
            {
                _logger.LogInformation($"Couldn't find the Transaction Requested : {transaction.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation($"Updated the Transaction with ID: {transaction.Id}");
                    exisitingTransaction.CategoryId = categoryName;
                    exisitingTransaction.TransferredAmount = transaction.TransferredAmount;
                    exisitingTransaction.TransactionDate = transaction.TransactionDate;
                    exisitingTransaction.Name = transaction.Name;
                    exisitingTransaction.Description = transaction.Description;

                    await TransactionRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Databsae issue : {ex.Message} while trying to update Transaction {transaction.Id}");
                    throw;
                }
                return Json(new { isValid = true, html = Helper.RenderViewToString(this, "_EditTransactionPartial", TransactionRepository.GetAllTransactionsAsync()) });
            }

            return Json(new { isValid = false, html = Helper.RenderViewToString(this, "_EditTransactionPartial", transaction) });
        }

        [HttpPost, ActionName("DeleteTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var transaction = await TransactionRepository.GetTransactionById(id);

                    if (transaction == null)
                    {
                        _logger.LogError($"Transaction with ID: {id} couldn't be found.", id);
                        return Problem("Couldn't find the Transaction");
                    }

                    if (transaction != null)
                    {
                        _logger.LogInformation($"Deleting Transaction: {transaction.Name}", transaction.Name);
                        await TransactionRepository.DeleteAsync(transaction.Id);
                        await TransactionRepository.SaveChangesAsync();
                    }

                    var wallets = await UserWalletRepository.GetUserWalletsAsync();
                    var categories = await CategoryRepository.GetAllCategoriesAsync();

                    var viewModel = new MainViewModel
                    {
                        UserWallets = wallets,
                        Categories = categories
                    };

                    return Json(new { html = Helper.RenderViewToString(this, "Index", viewModel) });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't delete transcation with ID {id} due to error {ex.Message}", id, ex.Message);
                }
            }

            return Json(new { html = Helper.RenderViewToString(this, "Index", null) });
        }

        [HttpPost]
        [ActionName("SaveCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category newCategory = new Category();
                    newCategory.Name = category.Name;

                    _logger.LogInformation($"Saving a new category {newCategory.Name}");
                    await CategoryRepository.CreateAsync(newCategory);
                    await CategoryRepository.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't Save Model: {category} with error {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await CategoryRepository.GetCategoryByIdAsync(id);

                    if (category == null)
                    {
                        _logger.LogError($"Category with ID: {id} couldn't be found.");
                        return Problem("Couldn't find Category");
                    }
                    
                    if (category != null)
                    {
                        _logger.LogInformation($"Deleting category: {category.Name}");
                        await CategoryRepository.DeleteAsync(category.Id);
                        await CategoryRepository.SaveChangesAsync();
                    }

                    var wallets = await UserWalletRepository.GetUserWalletsAsync();
                    var categories = await CategoryRepository.GetAllCategoriesAsync();

                    var viewModel = new MainViewModel
                    {
                        UserWallets = wallets,
                        Categories = categories
                    };

                    return Json(new { html = Helper.RenderViewToString(this, "_ListCategoriesPartial", viewModel) });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldn't delete Category with ID: {id} due to error : {ex.Message} ");
                }
            }

            return Json(new { html = Helper.RenderViewToString(this, "_ListCategoriesPartial", null) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditCategory(int id)
        {
            if (id == 0)
            {
                return View(new Category());
            }
            else
            {
                var category = await CategoryRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogInformation($"Category with ID : {id} was not found.", id);
                    return NotFound();
                }
                return Json(new { html = Helper.RenderViewToString(this, "_EditCategoryPartial", category) });
            }
        }

        [HttpPost]
        [ActionName("UpdateCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(int id, [Bind("Id", "Name")] Category category)
        {
            var existingCategory = await CategoryRepository.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                _logger.LogError($"Failed to find Category with ID: {category.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingCategory.Name = category.Name;

                    await CategoryRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Couldn't update Category, Error : {ex.Message}");
                    throw;
                }

                return Json(new { isValid = true, html = Helper.RenderViewToString(this, "_ListCategoriesPartial", CategoryRepository.GetAllCategoriesAsync()) });
            }
            return Json(new { isValid = false, html = Helper.RenderViewToString(this, "_EditCategoryPartial", category) });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError($"RequestedID : {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}