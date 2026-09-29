using Microsoft.EntityFrameworkCore;
using SmartWarehouse.Data;
using SmartWarehouse.Models;

namespace SmartWarehouse.Services;

public class InventoryService
{
    private readonly WarehouseDbContext _context;

    public InventoryService(WarehouseDbContext context)
    {
        _context = context;
    }


    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    
    public async Task AddOrUpdateProductAsync(Product newProduct)
    {
        var existing = await _context.Products
            .FirstOrDefaultAsync(p => p.Name != null && p.Name.ToLower() == (newProduct.Name ?? "").ToLower());

        if (existing != null)
        {
            existing.Quantity += newProduct.Quantity;
        }
        else
        {
            _context.Products.Add(newProduct);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<bool> SellProductAsync(int id, int amount)
    {
        var product = await _context.Products.FindAsync(id);

        if (product != null && product.Quantity >= amount)
        {
            product.Quantity -= amount;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
