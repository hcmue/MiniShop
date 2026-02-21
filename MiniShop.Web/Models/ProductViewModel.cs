namespace MiniShop.Web.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public string CurrentCategory { get; set; } = string.Empty;
    }
}
