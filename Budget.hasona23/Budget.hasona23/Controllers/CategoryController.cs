using Budget.hasona23.Models;
using Budget.hasona23.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget.hasona23.Controllers
{
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        // GET: CategoryController
        public ActionResult Index()
        {
            var categories = categoryService.GetAllCategories();
            return View(categories);
        }
        
        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name")]CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    return Json(new
                    {
                        success = true
                    });
                }
                catch(Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        errors = (string[])[ex.Message],
                    });
                }
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v=>v.Errors).Select(e=>e.ErrorMessage) });

        }
        

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Name")]CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    return Json(new
                    {
                        success = true
                    });
                }
                catch(Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        errors = (string[])[ex.Message],
                    });
                }
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v=>v.Errors).Select(e=>e.ErrorMessage) });

        }
        

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                return Json(new {
                    success=true});
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
