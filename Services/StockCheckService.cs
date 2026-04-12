using Microsoft.Extensions.DependencyInjection;
using SmartWarehouse.Data;

public class StockCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<int> _notifiedIds = new HashSet<int>();

    public StockCheckService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
                var lowStock = dbContext.Products.Where(p => p.Quantity <= p.MinThreshold).ToList();

                foreach (var item in lowStock)
                {
                    if (!_notifiedIds.Contains(item.Id))
                    {
                        Console.WriteLine($"\n[!!!] DIQQAT: {item.Name} tugamoqda! (Qoldi: {item.Quantity})");
                        Console.WriteLine($"[TAVSIYA]: {item.Name} uchun buyurtma bering!");
                        _notifiedIds.Add(item.Id);
                    }
                }
                var recovered = _notifiedIds.Where(id => !lowStock.Any(p => p.Id == id)).ToList();
                foreach (var id in recovered) _notifiedIds.Remove(id);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}