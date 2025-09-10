using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Persistence;
using ComputerSales.Infrastructure.Repositories.Account_Respo;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddControllersWithViews();


builder.Services.AddInfrastructure(builder.Configuration);


//------------------------Account-------------------------------
builder.Services.AddScoped<CreateAccount_UC>();



//MiddleWare
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
