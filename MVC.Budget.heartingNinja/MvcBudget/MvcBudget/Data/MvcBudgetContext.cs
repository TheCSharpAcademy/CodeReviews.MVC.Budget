using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcBudget.Models;

namespace MvcBudget.Data
{
    public class MvcBudgetContext : DbContext
    {
        public MvcBudgetContext (DbContextOptions<MvcBudgetContext> options)
            : base(options)
        {
        }

        public DbSet<MvcBudget.Models.Transaction> Transaction { get; set; } = default!;

        public DbSet<MvcBudget.Models.Category>? Category { get; set; }
    }
}
