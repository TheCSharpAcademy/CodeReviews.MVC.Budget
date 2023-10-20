namespace MVCBudget.Forser.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TransactionRepository> _logger;
        public TransactionRepository(AppDbContext appDbContext, ILogger<TransactionRepository> logger) : base(appDbContext)
        {
            _context = appDbContext;
            _logger = logger;
        }

        public bool DeleteTransaction(Transaction transaction)
        {
            try
            {
                _logger.LogInformation($"Removing Transaction with ID: {transaction.Id}");
                _context.Remove(transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(Transaction)} - Calling GetAllTransactionAsync()");
                var transactions = await _context.Transactions.ToListAsync();

                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            try
            {
                _logger.LogInformation($"{nameof(Transaction)} - Calling GetAllTransactions()");
                var transactions = _context.Transactions.ToList();

                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Transaction> GetTransactionById(int? id)
        {
            try
            {
                _logger.LogInformation($"Fetched the Transaction with ID: {id}");
                var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
