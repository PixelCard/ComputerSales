using API_ComputerProject.Extensions;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCase.Order_UC;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Application.UseCase.ProductProtection_UC;
using ComputerSales.Application.UseCase.Role_UC;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//DependencyInjection

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureAutoMapper();

builder.Services.AddApplicationUseCase();

var app = builder.Build();
// MiddleWare
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

// Tự apply migration khi khởi động (tiện dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
