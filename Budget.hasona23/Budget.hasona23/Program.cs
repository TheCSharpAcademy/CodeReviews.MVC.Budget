using Microsoft.EntityFrameworkCore;
using Budget.hasona23.Data;
using Budget.hasona23.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BudgetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetDB") ?? throw new InvalidOperationException("Connection string 'BudgetDB' not found.")));
builder.Services.AddScoped<ITransactionService,TransactionService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<BudgetContext>().Database.Migrate();
    try
    {
        DataSeed.Seed(scope.ServiceProvider);
    }
    catch (Exception e)
    {
        scope.ServiceProvider.GetRequiredService<ILogger>().LogError(e.Message);
        throw;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Category}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
Console.ReadLine();