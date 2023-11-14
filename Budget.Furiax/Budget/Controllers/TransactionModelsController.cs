using Budget.DataAccess;
using Budget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionModelsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public TransactionModelsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/TransactionModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactions()
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/TransactionModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionModel>> GetTransactionModel(int id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var transactionModel = await _context.Transactions.FindAsync(id);

            if (transactionModel == null)
            {
                return NotFound();
            }

            return transactionModel;
        }

        // PUT: api/TransactionModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionModel(int id, TransactionModel transactionModel)
        {
            if (id != transactionModel.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transactionModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TransactionModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionModel>> PostTransactionModel(TransactionModel transactionModel)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'BudgetContext.Transactions'  is null.");
            }
            _context.Transactions.Add(transactionModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionModel", new { id = transactionModel.TransactionId }, transactionModel);
        }

        // DELETE: api/TransactionModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionModel(int id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var transactionModel = await _context.Transactions.FindAsync(id);
            if (transactionModel == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transactionModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionModelExists(int id)
        {
            return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
