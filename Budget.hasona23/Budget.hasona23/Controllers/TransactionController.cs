using Budget.hasona23.Models;
using Budget.hasona23.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget.hasona23.Controllers
{
    public class TransactionController(ITransactionService transactionService) : Controller
    {
        
        // GET: TransactionController
        public ActionResult Index()
        {
            var transactions = transactionService.GetAllTransactions();
            return View(transactions);
        }

        

        // POST: TransactionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Date,Price,Details,CategoryId")]TransactionDto transaction)
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

        // POST: TransactionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Date,Price,Details,CategoryId")]TransactionDto transaction)
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
        
        // POST: TransactionController/Delete/5
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
