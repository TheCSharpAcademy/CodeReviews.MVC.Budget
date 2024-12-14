using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget.hasona23.Models;
using Budget.hasona23.Services;

namespace Budget.hasona23.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllCategoriesAsync());
        }

       /* // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryModel = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryModel == null)
            {
                return NotFound();
            }

            return PartialView(categoryModel);
        }
        */
        // GET: Category/Create
        public IActionResult Create()
        {
            return PartialView(new CategoryDto(""));
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto categoryModel)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddCategoryAsync(new CategoryDto(categoryModel.Name));
                return Json(new { success = true });
            }
           
            return PartialView(categoryModel);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryModel = await _categoryService.GetCategoryByIdAsync((int)id);
            if (categoryModel == null)
            {
                return NotFound();
            }
            return PartialView(new CategoryDto(categoryModel.Name));
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDto categoryModel)
        {
            if (!CategoryModelExists(id))
            {
                Console.WriteLine("Category id is invalid");
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(id,new CategoryDto(categoryModel.Name));
                    return Json(new { success = true });;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"DbUpdateConcurrencyException occured: {ex}");
                }
               
            }
            return PartialView(categoryModel);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryModel = await _categoryService.GetCategoryByIdAsync((int)id);
            if (categoryModel == null)
            {
                return NotFound();
            }

            return PartialView(categoryModel);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryModel = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryModel != null)
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            return Json(new { success = true });
        }

        private bool CategoryModelExists(int id)
        {
            return _categoryService.GetCategoryById(id) != null;
        }
    }
}
