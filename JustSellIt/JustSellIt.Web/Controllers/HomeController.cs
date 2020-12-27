using JustSellIt.Application.Interfaces;
using JustSellIt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JustSellIt.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        List<Product> products = new List<Product>();

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
            products.Add(new Product() { Id = 1, Name = "Harry Poter", Category = "Książki", Owner = "Marek" });
            products.Add(new Product() { Id = 2, Name = "Opel", Category = "Motoryzacja", Owner = "Jan" });
            products.Add(new Product() { Id = 3, Name = "Samsung", Category = "Telewizory", Owner = "Magda" });
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Witamy w HomeController");
            return View();
        }

        public IActionResult AllProducts()
        {
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var model = products.FirstOrDefault(x => x.Id == id);

            if (model == null)
                return BadRequest();

            return View(model);
        }

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
