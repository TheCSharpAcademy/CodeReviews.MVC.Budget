using Budget.jollejonas.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BudgetjollejonasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetjollejonasContext") ?? throw new InvalidOperationException("Connection string 'BudgetjollejonasContext' not found.")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transactions}/{action=Index}/{id?}");


app.Run();

