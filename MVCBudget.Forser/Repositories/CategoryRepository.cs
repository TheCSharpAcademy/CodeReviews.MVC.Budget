namespace MVCBudget.Forser.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoryRepository(AppDbContext appDbContext, ILogger<CategoryRepository> logger) : base(appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(Category)} - Calling GetAllCategoriesAsync()");
                var categories = await _appDbContext.Categories.ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
    }
}