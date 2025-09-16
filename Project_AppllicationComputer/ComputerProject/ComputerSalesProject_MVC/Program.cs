using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSales.Infrastructure.Sercurity.JWT.Extensions;
using ComputerSalesProject_MVC.DependencyInjetionServices;
using ComputerSalesProject_MVC.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ====================================================
// Add services to the container (DI)
// ====================================================
builder.Services.AddControllersWithViews();


builder.Services.AddJwtAuth(builder.Configuration, requireHttps: false);


// gọi Infrastructure + ApplicationUseCase
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationUseCase();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWorkApplication, UnitOfWork_Infa>();

// AutoMapper
builder.Services.ConfigureAutoMapper();

// Add UseCase for MVC
builder.Services.AddUseCaseMVC();

// ====================================================
// Build app
// ====================================================
var app = builder.Build();

// ====================================================
// Middleware pipeline
// ====================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

// map route cho Areas
app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

// map route mặc định (không có area)
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
