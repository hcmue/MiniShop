namespace MiniShop.Web.Models.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public int ProductId { get; }

        public ProductNotFoundException(int productId)
            : base($"Sản phẩm ID {productId} không tồn tại")
        {
            ProductId = productId;
        }
    }
}
