using Microsoft.EntityFrameworkCore;
using SmartWarehouse.Data;
using SmartWarehouse.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlite("Data Source=SmartWarehouse.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<EmailService>(); 
builder.Services.AddHostedService<InventoryMonitorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
