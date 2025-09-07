using ComputerSales.Application.UseCase.Product_UC;
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

builder.Services.AddScoped<CreateProduct_UC>();

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
