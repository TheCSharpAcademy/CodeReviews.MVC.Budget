using Microsoft.AspNetCore.Mvc;
using MVC.Budget.JsPeanut.Data;
using MVC.Budget.JsPeanut.Models.ViewModel;

namespace MVC.Budget.JsPeanut.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
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

        //public void GetAllCategories()
        //{
        //}
    }
}
