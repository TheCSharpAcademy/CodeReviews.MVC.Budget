using Budget.Doc415.Models;
using Budget.Doc415.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Doc415.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: CategoryController
        public ActionResult Index()
        {
            return RedirectToAction("Transaction", "Index");
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> Add()
        {

            return RedirectToAction("Index", "Transaction");
        }

        // POST: CategoryController/Create

        [HttpPost]
        public async Task<ActionResult> Create(Category NewCategory)
        {
            Console.WriteLine(NewCategory.Name);
            try
            {
                await _categoryService.InsertCategory(NewCategory);
                return RedirectToAction("Index", "Transaction");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            await _categoryService.UpdateCategory(category);
            return RedirectToAction("Index", "Transaction");
        }


        // POST: CategoryController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
