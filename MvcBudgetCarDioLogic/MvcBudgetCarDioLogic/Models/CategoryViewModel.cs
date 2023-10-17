namespace MvcBudgetCarDioLogic.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; } //= new List<Category>();
        public Category Category { get; set; } //= new Category();
    }
}
