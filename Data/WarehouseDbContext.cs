using Microsoft.EntityFrameworkCore;
using SmartWarehouse.Models;

namespace SmartWarehouse.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}