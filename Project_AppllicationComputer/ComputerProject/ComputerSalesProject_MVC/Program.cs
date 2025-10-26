using ComputerSales.Application.Payment.VNPAY.Entity;
using ComputerSales.Application.Sercurity.JWT.Extensions;
using ComputerSales.Infrastructure;
using ComputerSalesProject_MVC.Extensions;
using ComputerSalesProject_MVC.MiddleWareCustome;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

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


//CORS Test
builder.Services.AddCors(o => o.AddPolicy("GamePieceLabsPolicy", builder =>
{
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(configuration["AllowedOrigins"])
        .AllowCredentials();
}));

// ====================================================
// Middleware pipeline
// ====================================================
var app = builder.Build();
app.UseStatusCodePages();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseCors("GamePieceLabsPolicy");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AutoRefreshAccessMiddleware>();

//dùng cho logo
app.UseStaticFiles();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


// map route mặc định (không có area)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllers();


app.Run();


// Thêm dòng này:
public partial class Program { }