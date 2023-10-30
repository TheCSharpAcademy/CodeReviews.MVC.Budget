using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcBudgetCarDioLogic.Data;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcBudgetCarDioLogicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBudgetCarDioLogicContext") ?? throw new InvalidOperationException("Connection string 'MvcBudgetCarDioLogicContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    MvcBudgetCarDioLogicContext context = scope.ServiceProvider.GetService<MvcBudgetCarDioLogicContext>();
    var services = scope.ServiceProvider;

    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transactions}/{action=Index}/{id?}");


app.UseRequestLocalization("en-US");
app.Run();
