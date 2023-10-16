namespace MVCBudget.Forser.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _entities = appDbContext.Set<T>();
        }

        public async Task CreateAsync(T entity) => await _context.AddAsync(entity);

        public async Task DeleteAsync(int id)
        {
            T existing = await _entities.FindAsync(id);
            _entities.Remove(existing);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _entities.FindAsync(id);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}