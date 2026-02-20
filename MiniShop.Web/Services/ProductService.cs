using MiniShop.Web.Models;
using MiniShop.Web.Models.Exceptions;

namespace MiniShop.Web.Services
{
    public class FakeProductService : IProductService
    {
        public List<Product> GetAll()
        {
            return new List<Product>
            {
                new() { Id = 1, Name = "iPhone 16 Pro", Price = 29990000, Category = "Điện thoại" },
                new() { Id = 2, Name = "MacBook Pro M4", Price = 59990000, Category = "Laptop" },
                new() { Id = 3, Name = "iPad Pro", Price = 24990000, Category = "Tablet" }
            };
        }

        public Product? GetById(int id)
        {
            try
            {
                var product = GetAll().FirstOrDefault(p => p.Id == id);
                if (product == null)
                    throw new ProductNotFoundException(id);
                return product;
            }
            catch (Exception ex)
            {
                // Log lỗi business
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine($"Service error: {ex.Message}");
                throw; // Re-throw để controller xử lý
            }
        }

        // FakeProductService.cs
        public async Task<List<Product>> GetAllAsync()
        {
            await Task.Delay(500); // Giả lập DB call
            return GetAll();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            await Task.Delay(200);
            return GetAll().FirstOrDefault(p => p.Id == id);
        }
    }
}
