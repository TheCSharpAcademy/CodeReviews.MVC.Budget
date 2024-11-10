
using Budget.Doc415.Data;
using Budget.Doc415.Repositories;
using Budget.Doc415.Services;
using Microsoft.EntityFrameworkCore;

namespace Budget.Doc415;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContextFactory<BudgetDb>(
          options => options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetDb") ?? throw new InvalidOperationException("Connection string 'BudgetDb' not found.")));


        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<TransactionService>();
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddMvcCore().AddRazorViewEngine();
        builder.Services.AddControllers();
        builder.Services.AddScoped<Seeder>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.


        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseStaticFiles();

        app.UseRouting();


        app.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Transaction}/{action=Index}/{id?}");

        app.Run();
    }
}
