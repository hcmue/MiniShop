using MiniShop.Web.Models;

namespace MiniShop.Web.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetById(int id);
        Task<List<Product>> GetAllAsync();        
        Task<Product?> GetByIdAsync(int id);
    }
}
