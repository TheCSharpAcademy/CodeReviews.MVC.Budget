using Microsoft.EntityFrameworkCore;
using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Repositories;
using MVC.Budget.K_MYR.Services;
using Newtonsoft.Json;
using System.Globalization;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IGroupsRepository, GroupsRepository>();
builder.Services.AddTransient<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddTransient<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddTransient<ICategoryStatisticsRepository, CategoryStatisticsRepository>();
builder.Services.AddTransient<ICategoriesService, CategoriesService>();
builder.Services.AddTransient<ITransactionsService, TransactionsService>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSQL")));
builder.Services.AddControllersWithViews()
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
     });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    if (builder.Configuration.GetValue<bool>("Auto-Migrate"))
        db.Database.Migrate();
    if (builder.Configuration.GetValue<bool>("SeedData"))
        SeedData.InitializeDatabase(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
