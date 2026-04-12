using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartWarehouse.Data;

namespace SmartWarehouse.Services
{
    public class InventoryMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<string> _notifiedProducts = new HashSet<string>();

        public InventoryMonitorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    
                    var products = await dbContext.Products.ToListAsync(stoppingToken);
                    var lowStockItems = products.Where(p => p.Quantity <= p.MinThreshold).ToList();

                    foreach (var item in lowStockItems)
                    {
                        if (!_notifiedProducts.Contains(item.Name ?? "Unknown"))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n[AVTO-OGOHLANTIRISH]: {item.Name} tugamoqda! (Qoldi: {item.Quantity})");
                            Console.ResetColor();
                            
                            // Try-Catch yordamida xatolarni ushlaymiz
                            try 
                            {
                                await emailService.SendEmailAsync(
                                    "lochinbekmajidov3737@gmail.com", 
                                    "Omborxona Ogohlantiruvi!", 
                                    $"Diqqat: {item.Name} mahsuloti tugamoqda! Qoldi: {item.Quantity} dona.");
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Email yuborishda xatolik: {ex.Message}");
                                Console.ResetColor();
                            }

                            _notifiedProducts.Add(item.Name ?? "Unknown");
                        }
                    }

                    var recovered = _notifiedProducts.Where(name => !lowStockItems.Any(p => p.Name == name)).ToList();
                    foreach (var name in recovered) _notifiedProducts.Remove(name);
                }

                await Task.Delay(10000, stoppingToken); // Har 10 soniyada tekshiradi
            }
        }
    }
}