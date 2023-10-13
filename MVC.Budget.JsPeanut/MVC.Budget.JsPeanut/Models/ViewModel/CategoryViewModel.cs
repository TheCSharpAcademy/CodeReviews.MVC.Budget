using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Budget.JsPeanut.Models.ViewModel
{
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<SelectListItem> CategorySelectList { get; set; }
        public Transaction Transaction { get; set; }
    }
}
