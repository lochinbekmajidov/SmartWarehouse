using SmartWarehouse.Models;

namespace SmartWarehouse.Helpers
{
    public static class UIHelper
    {
        public static void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine("======================================================================");
            Console.WriteLine(string.Format("| {0,-5} | {1,-20} | {2,-10} | {3,-10} | {4,-8} |", "ID", "Nomi", "Kategoriya", "Miqdor", "Narx"));
            Console.WriteLine("======================================================================");
        }

        public static void PrintRow(Product p)
        {
            if (p.Quantity <= p.MinThreshold) Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine(string.Format("| {0,-5} | {1,-20} | {2,-10} | {3,-10} | {4,-8:F2} |", 
                p.Id, p.Name, p.Category, p.Quantity, p.Price));
            
            Console.ResetColor();
        }
    }
}