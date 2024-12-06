using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Budget.hasona23.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BudgetDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetDB") ?? throw new InvalidOperationException("Connection string 'BudgetDB' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<BudgetDB>().Database.Migrate();
    var seed = new DataSeeder(scope.ServiceProvider);
    seed.SeedData();
}
// Configure the HTTP request pipeline.
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
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();