namespace MVC.Budget.JsPeanut.Services
{
    public interface ICategoryService
    {
        public List<Models.Category> GetAllCategories();
        public void AddCategory(Models.Category category);
        public void DeleteCategory(Models.Category category);
        public void UpdateCategory(Models.Category category);
    }
}
