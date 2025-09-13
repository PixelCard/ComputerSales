using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSalesProject_MVC.DependencyInjetionServices;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddControllersWithViews();


builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddScoped<IUnitOfWorkApplication, UnitOfWork_Infa>();


builder.Services.AddUseCaseMVC(); //Add Dependency Injection cho các UseCase

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
