using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustSellIt.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            SearchProductVm searchProduct = new SearchProductVm();
            var model = _productService.GetAllProduct(searchProduct);

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SearchProductVm searchProduct)
        {          
            var model = _productService.GetAllProduct(searchProduct);

            return View(model);
        }

        [HttpGet]
        public IActionResult ViewProduct(int productId)
        {
            var model = _productService.GetProductDetails(productId);

            return View(model);
        }
    }
}
