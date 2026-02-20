using Microsoft.AspNetCore.Mvc;
using MiniShop.Web.Services;

namespace MiniShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Products/Index hoặc /san-pham
        [Route("san-pham")]
        [Route("Products")]
        [Route("Products/Index")]
        public IActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        // GET: /Products/Details/1 hoặc /san-pham/iphone-16-pro
        [Route("Products/Details/{id}")]
        [Route("san-pham/{slug?}")]
        public IActionResult Details(string? slug = null, int id = 0)
        {
            // Convert slug → ID hoặc tìm theo tên
            if (slug != null)
            {
                id = slug switch
                {
                    "iphone-16-pro" => 1,
                    "macbook-pro-m4" => 2,
                    "ipad-pro" => 3,
                    _ => 0
                };
            }
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
