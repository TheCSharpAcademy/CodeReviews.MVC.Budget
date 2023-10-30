using Microsoft.AspNetCore.Mvc;
using MvcBudgetCarDioLogic.Data;
using MvcBudgetCarDioLogic.Models;

namespace MvcBudgetCarDioLogic.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MvcBudgetCarDioLogicContext _context;

        public CategoriesController(MvcBudgetCarDioLogicContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel()
            { 
                Categories = _context.Categories.ToList() 
            }; 

            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public  IActionResult DeleteCategory(Category category)//This category that the method receives only gets the category.Id from the view, the rest is null
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'MvcBudgetCarDioLogicContext.Category'  is null.");
            } 

            var categoryToDel = _context.Categories.Find(category.Id);
            if (category != null)
            {
                _context.Categories.Remove(categoryToDel);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
