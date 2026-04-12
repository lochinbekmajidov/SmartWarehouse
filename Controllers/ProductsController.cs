using Microsoft.AspNetCore.Mvc;
using SmartWarehouse.Services;
using SmartWarehouse.Models;

namespace SmartWarehouse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    // Konstruktor
    public ProductsController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // GET: Barcha mahsulotlarni olish
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Endi bu qator xato bermaydi, chunki InventoryService ichida metod bor
        var products = await _inventoryService.GetAllProductsAsync();
        return Ok(products);
    }
    // POST: Qo'shish
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        await _inventoryService.AddOrUpdateProductAsync(product);
        return Ok("Mahsulot qo'shildi!");
    }
    // 3. POST: Mahsulot sotish (bazadan ayirish)
    [HttpPost("sell/{id}")]
    public async Task<IActionResult> SellProduct(int id, [FromBody] int amount)
    {
        // Service orqali bazadan ayiramiz
        var success = await _inventoryService.SellProductAsync(id, amount);
        
        if (!success) 
            return BadRequest("Mahsulot topilmadi yoki miqdori yetarli emas!");
            
        return Ok("Sotildi!");
    }
}