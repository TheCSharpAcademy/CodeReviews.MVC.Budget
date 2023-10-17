using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcBudgetCarDioLogic.Models;

namespace MvcBudgetCarDioLogic.Data
{
    public class MvcBudgetCarDioLogicContext : DbContext
    {
        public MvcBudgetCarDioLogicContext (DbContextOptions<MvcBudgetCarDioLogicContext> options)
            : base(options)
        {
        }

        public DbSet<MvcBudgetCarDioLogic.Models.Category> Categories { get; set; } //= default!;

        public DbSet<MvcBudgetCarDioLogic.Models.Transaction> Transactions { get; set; } //= default!;
    }
}
