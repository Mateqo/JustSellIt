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
        private readonly IStatusService _statusService;

        public ProductController(IProductService productService, IStatusService statusService)
        {
            _productService = productService;
            _statusService = statusService;
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

        [HttpGet]
        public IActionResult AddProduct()
        {
            NewOrEditProductVm newProduct = new NewOrEditProductVm() 
            { 
                ProductStatusId = _statusService.GetIdBeforeNew() 
            };

            return View(newProduct);
        }

        [HttpPost]
        public IActionResult AddProduct(NewOrEditProductVm model)
        {
            var id = _productService.AddProduct(model);

            return RedirectToAction("Index");
        }
    }
}
