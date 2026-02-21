using Microsoft.AspNetCore.Mvc;
using MiniShop.Web.Models;
using MiniShop.Web.Services;
using System.Text;

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
        // 1️. ViewResult - Trả Razor View (.cshtml)
        public IActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        // 2️. RedirectResult - Redirect 302
        public IActionResult Search(string keyword)
        {
            TempData["Success"] = $"Có sản phẩm '{keyword}'!";
            return Redirect("/Products"); // Hoặc Redirect("Index")
        }

        // 3️. RedirectToActionResult - Redirect với route values
        public IActionResult Success()
        {
            return RedirectToAction("Index", "Home", new { highlight = "new" });
        }

        // 4️. RedirectToRouteResult - Redirect theo route name
        public IActionResult GoHome()
        {
            return RedirectToRoute("default", new { controller = "Home", action = "Index" });
        }

        // 5️. NotFoundResult - 404
        public IActionResult Details(int id)
        {
            var product = _productService.GetById(id);
            return product == null ? NotFound() : View(product);
        }

        // 6️. BadRequestResult/ObjectResult - 400 + JSON
        public IActionResult Validate(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
                return BadRequest("Tên sản phẩm không được để trống");

            return Ok(new { message = "Valid", product });
        }

        // 7️. ContentResult - Plain text/HTML
        public ContentResult DownloadCSV()
        {
            var csv = "Id,Name,Price\n1,iPhone 16,25000000";
            return Content(csv, "text/csv", Encoding.UTF8);
        }

        // 8️. FileResult - Download file
        public IActionResult DownloadImage()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/product.png");
            if (!System.IO.File.Exists(path))
                return NotFound("File không tồn tại");

            return PhysicalFile(path, "image/png", "sample-product.jpg");
        }

        // 9️. JsonResult - JSON API response
        [Produces("application/json")]
        public IActionResult ApiProducts()
        {
            var products = _productService.GetAll();
            return Json(products);
        }

        // 10️. StatusCodeResult - Custom HTTP status
        public IActionResult Maintenance()
        {
            Response.StatusCode = 503;
            return Content("Server bảo trì, vui lòng quay lại sau!");
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

        public IActionResult TestResults()
        {
            return View();
        }
    }
}
