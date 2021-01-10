﻿using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using JustSellIt.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustSellIt.Web.Controllers
{
    public class ProductController : BaseController
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
                ProductStatusId = _statusService.GetIdBeforeNew(),
                Categories = _productService.GetAllCategory()
            };

            return View("AddOrEditProduct",newProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(NewOrEditProductVm model)
        {
            if (ModelState.IsValid)
            {
                model.ProductStatusId = _statusService.GetIdForVeryfication();
                _productService.AddProduct(model);
                SetMessage("Ogłoszenie dodane", MessageType.Success);
                return RedirectToAction("Index");
            }
            else
            {
                model.Categories = _productService.GetAllCategory();
                SetMessage("Uzupełnij wymagane pola",MessageType.Error);
                return View("AddOrEditProduct", model);
            }
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetProductForEdit(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(NewOrEditProductVm model)
        {
            _productService.UpdateProduct(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
