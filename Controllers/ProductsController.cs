using Microsoft.AspNetCore.Mvc;
using SmartWarehouse.Services;
using SmartWarehouse.Models;

namespace SmartWarehouse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public ProductsController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _inventoryService.GetAllProductsAsync();
        return Ok(products);
    }
 
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        await _inventoryService.AddOrUpdateProductAsync(product);
        return Ok("Mahsulot qo'shildi!");
    }
    
    [HttpPost("sell/{id}")]
    public async Task<IActionResult> SellProduct(int id, [FromBody] int amount)
    {
       
        var success = await _inventoryService.SellProductAsync(id, amount);
        
        if (!success) 
            return BadRequest("Mahsulot topilmadi yoki miqdori yetarli emas!");
            
        return Ok("Sotildi!");
    }
}
