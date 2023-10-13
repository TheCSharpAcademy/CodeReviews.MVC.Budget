using MVC.Budget.JsPeanut.Data;

namespace MVC.Budget.JsPeanut.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public List<Models.Category> GetAllCategories()
        {
            var categories = _context.Categories.ToList();

            return categories;
        }
        public void AddCategory(Models.Category category)
        {
            _context.Categories.Add(category);

            _context.SaveChanges();
        }
        public void DeleteCategory(Models.Category category)
        {
            _context.Categories.Remove(category);

            _context.SaveChanges();
        }
        public void UpdateCategory(Models.Category category)
        {
            _context.Categories.Update(category);

            _context.SaveChanges();
        }
    }
}
