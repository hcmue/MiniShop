using Microsoft.AspNetCore.Mvc;
using MiniShop.Web.Models;
using MiniShop.Web.Models.Exceptions;
using MiniShop.Web.Services;
using System.Diagnostics;

namespace MiniShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index() // Task<IActionResult>
        {
            var products = await _productService.GetAllAsync();
            ViewData["Products"] = products;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product != null ? View(product) : NotFound();
        }

        //public IActionResult Index()
        //{
        //    var products = _productService.GetAll();
        //    ViewData["Products"] = products;
        //    return View();
        //}

        //public IActionResult Details(int id)
        //{
        //    try
        //    {
        //        var product = _productService.GetById(id);
        //        if (product == null) return NotFound();
        //        return View(product);
        //    }
        //    catch (ProductNotFoundException ex)
        //    {
        //        TempData["Error"] = ex.Message;
        //        return RedirectToAction("Error");
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
