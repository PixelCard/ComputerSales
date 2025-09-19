using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Repositories.RefreshToken_Respo;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSales.Infrastructure.Sercurity.JWT.Extensions;
using ComputerSalesProject_MVC.DependencyInjetionServices;
using ComputerSalesProject_MVC.Extensions;
using ComputerSalesProject_MVC.MiddleWareCustome;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

// ====================================================
// Add services to the container (DI)
// ====================================================
builder.Services.AddControllersWithViews();


builder.Services.AddJwtAuth(builder.Configuration, requireHttps: false);


// gọi Infrastructure + ApplicationUseCase
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddApplicationUseCase();


// AutoMapper
builder.Services.ConfigureAutoMapper();


// Add UseCase for MVC
builder.Services.AddUseCaseMVC();



// ====================================================
// Middleware pipeline
// ====================================================
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseMiddleware<AutoRefreshAccessMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


// map route mặc định (không có area)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();


// Thêm dòng này:
public partial class Program { }