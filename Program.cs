using Microsoft.EntityFrameworkCore;
using SmartWarehouse.Data;
using SmartWarehouse.Services;

var builder = WebApplication.CreateBuilder(args);

// --- HAMMA SERVILARNI SHU YERGA YOZING ---
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlite("Data Source=SmartWarehouse.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bular BUILD() dan oldin bo'lishi shart!
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<EmailService>(); 
builder.Services.AddHostedService<InventoryMonitorService>();
// ------------------------------------------

// FAQAT SHU QATORDAN KEYIN BUILD QILING
var app = builder.Build();

// Endi faqat Middleware (app.Use...) qismlari keladi
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();