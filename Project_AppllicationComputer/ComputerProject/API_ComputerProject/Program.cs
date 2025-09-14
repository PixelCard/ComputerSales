using API_ComputerProject.Extensions;
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
