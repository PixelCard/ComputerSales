using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSales.Infrastructure.Sercurity.JWT.Extensions;
using ComputerSalesProject_MVC.DependencyInjetionServices;
using ComputerSalesProject_MVC.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddControllersWithViews();


builder.Services.AddJwtAuth(builder.Configuration, requireHttps: false);


builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddScoped<IUnitOfWorkApplication, UnitOfWork_Infa>();


builder.Services.ConfigureAutoMapper();


builder.Services.AddUseCaseMVC(); //Add Dependency Injection cho các UseCase

//MiddleWare
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
