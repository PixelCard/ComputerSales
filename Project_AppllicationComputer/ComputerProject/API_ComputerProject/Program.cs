using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Domain.Entity;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DependencyInjection

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);


/********************Product**************************/
builder.Services.AddScoped<CreateProduct_UC>();
builder.Services.AddScoped<GetProduct_UC>();
builder.Services.AddScoped<UpdateProduct_UC>();
builder.Services.AddScoped<DeleteProduct_UC>();
/*****************************************************/

/********************Product Over View**************************/
builder.Services.AddScoped<CreateProductOverView_UC>();
builder.Services.AddScoped<DeleteProductOverView_UC>();
builder.Services.AddScoped<GetByIdProductOverView_UC>();
builder.Services.AddScoped<UpdateProductOverView_UC>();
/*****************************************************/



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
