namespace MVCBudget.Forser.ViewComponents
{
    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly ILogger<CategoriesListViewComponent> _logger;
        private ICategoryRepository _categoryRepository;

        public CategoriesListViewComponent(ILogger<CategoriesListViewComponent> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int CategoryId)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var categoriesSelectList = categories.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            if (CategoryId != null)
            {
                foreach (var category in categoriesSelectList) 
                {
                    if (category.Value == CategoryId.ToString())
                    {
                        category.Selected = true;
                        break;
                    }
                }
            }

            return View(categoriesSelectList);
        }
    }
}