using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Budget.JsPeanut.Models.ViewModel
{
    public class TransactionViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public List<Category> Categories { get; set; }
        public List<SelectListItem> CategorySelectList { get; set; }
        public List<SelectListItem> CurrencySelectList { get; set; }
        public string CurrencyObjectJson { get; set; }
        public string? SearchStringForName { get; set; }
        public string? FilterByCategoryString { get; set; }
        public string? FilterByDateString { get; set; }
        public Transaction Transaction { get; set; }
    }
}
