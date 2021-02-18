using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
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
            var model = _productService.GetLatesProducts();

            return View(model);
        }

        [HttpGet]
        public IActionResult SearchProducts(string searchString, string searchLocation, int? searchCategory, int? actualPage, bool isNewSearch)
        {
            int pageSize = SystemConfiguration.DefaultPageSize;
            var model = _productService.GetAllProduct(searchString, searchLocation, searchCategory, actualPage, isNewSearch, pageSize);

            return View(model);
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            var model = _productService.GetProductDetails(id);

            return View("ProductDetails", model);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            NewOrEditProductVm newProduct = new NewOrEditProductVm()
            {
                ProductStatusId = _statusService.GetIdBeforeNew(),
                Categories = _productService.GetAllCategory(),
                Action = "AddProduct"
            };

            return View("AddOrEditProduct", newProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(NewOrEditProductVm model, string businessAction)
        {
            if (ModelState.IsValid)
            {
                model.ProductStatusId = businessAction == "publish" ? _statusService.GetIdForVeryfication() : _statusService.GetIdDraft();
                model.CreatedOn = DateTime.Now;
                _productService.AddProduct(model);

                if (businessAction == "publish")
                    SetMessage("Ogłoszenie w trakcie weryfikacji", MessageType.Success);
                else
                    SetMessage("Ogłoszenie zapisano w wersji roboczej", MessageType.Success);

                return RedirectToAction("MyProducts", new { id = model.OwnerId });
            }
            else
            {
                model.Categories = _productService.GetAllCategory();
                SetMessage("Uzupełnij wymagane pola", MessageType.Error);
                return View("AddOrEditProduct", model);
            }
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetProductForEdit(id);
            product.CategoryImage = $"/images/categories/{_productService.GetImageCategoryById(product.CategoryId)}";
            product.CategoryName = _productService.GetNameCategoryById(product.CategoryId);
            product.Categories = _productService.GetAllCategory();
            product.Action = "EditProduct";
            return View("AddOrEditProduct", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(NewOrEditProductVm model, string businessAction)
        {
            if (ModelState.IsValid)
            {
                model.CreatedOn = DateTime.Now;
                model.ProductStatusId = businessAction == "publish" ? _statusService.GetIdForVeryfication() : _statusService.GetIdDraft();
                _productService.UpdateProduct(model);

                if (businessAction == "publish")
                    SetMessage("Ogłoszenie w trakcie weryfikacji", MessageType.Success);
                else
                    SetMessage("Ogłoszenie zapisano w wersji roboczej", MessageType.Success);

                return RedirectToAction("MyProducts", new { id = model.OwnerId });
            }
            else
            {
                model.Categories = _productService.GetAllCategory();
                SetMessage("Uzupełnij wymagane pola", MessageType.Error);
                return View("AddOrEditProduct", model);
            }
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            SetMessage("Ogłoszenie zostało usunięte", MessageType.Error);
            return RedirectToAction("MyProducts", new { id=_productService.GetOwnerIdByProductId(id)});
        }

        [HttpGet]
        public IActionResult AutoCompleteString(string text)
        {
            List<string> autoComplete = _productService.AutoCompleteString(text);
            var result = autoComplete.Select(x => new { id = x, label = x, value = x }).ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult AutoCompleteLocation(string text)
        {
            List<string> autoComplete = _productService.AutoCompleteLocation(text);
            var result = autoComplete.Select(x => new { id = x, label = x, value = x }).ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetContactInfo(int id)
        {
            var contact = _productService.GetContactById(id);

            return Json(new { data = contact });
        }

        [HttpGet]
        public IActionResult GetOwnerProducts(int id, int? actualPage)
        {
            int pageSize = SystemConfiguration.DefaultPageSize;
            var model = _productService.GetOwnerProducts(id, actualPage, pageSize);
            model.Action = "GetOwnerProducts";

            return View(model);
        }

        [HttpGet]
        public IActionResult MyProducts(int id, int? actualPage)
        {
            int pageSize = SystemConfiguration.DefaultPageSize;
            var model = _productService.GetMyProducts(id, actualPage, pageSize);
            model.Action = "MyProducts";

            return View("GetOwnerProducts", model);
        }
    }
}
