using System.Text.Json;
using SmartWarehouse.Models;

namespace SmartWarehouse.Data
{
    public static class DataManager
    {
        // Directory.GetCurrentDirectory() faylni loyihaning asosiy papkasida yaratishni ta'minlaydi
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "products.json");

        public static void SaveData(List<Product> products)
        {
            string json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static List<Product> LoadData()
        {
            if (!File.Exists(filePath)) return new List<Product>();
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
    }
}