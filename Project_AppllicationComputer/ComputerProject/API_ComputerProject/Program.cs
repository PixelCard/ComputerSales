using ComputerSales.Application.UseCase.Product.GetByID;
using ComputerSales.Application.UseCaseDTO.Product.GetByID;
using ComputerSales.Domain.Entity;
using ComputerSales.Infrastructure;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

//DependencyInjection

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<GetProductByIdUseCase>();

var app = builder.Build();


// MiddleWare
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Products.Any())
    {
        var p1 = Product.Create("Laptop A", "14\" i5, 16GB, 512GB");
        var p2 = Product.Create("Mouse X", "Wireless 2.4G");
        db.Products.AddRange(p1, p2);
        await db.SaveChangesAsync();

        app.Logger.LogInformation("Seeded products:");
        app.Logger.LogInformation(" - {Name} | Id = {Id}", p1.Name, p1.Id);
        app.Logger.LogInformation(" - {Name} | Id = {Id}", p2.Name, p2.Id);
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
