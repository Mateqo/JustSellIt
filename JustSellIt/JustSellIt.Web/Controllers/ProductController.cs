using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using JustSellIt.Application.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace JustSellIt.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IStatusService _statusService;
        private readonly IImageService _imageService;

        public ProductController(IProductService productService, IStatusService statusService, IImageService imageService)
        {
            _productService = productService;
            _statusService = statusService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _productService.GetLatesProducts();

            return View(model);
        }

        [HttpGet]
        public IActionResult SearchProducts(string searchString, string searchLocation, int? searchCategory, int? searchMinPrice, int? searchMaxPrice, string searchCondition, string sorting, bool isNewSearch, int? actualPage)
        {
            int pageSize = SystemConfiguration.DefaultPageSize;
            var model = _productService.GetAllProduct(searchString, searchLocation, searchCategory, searchMinPrice,
                searchMaxPrice, searchCondition, sorting, isNewSearch, pageSize, actualPage);


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
                try
                {
                    model.ProductStatusId = businessAction == "publish" ? _statusService.GetIdForVeryfication() : _statusService.GetIdDraft();
                    model.CreatedOn = DateTime.Now;

                    List<ImageProductVm> images = new List<ImageProductVm>();

                    var imageName1 = _imageService.UploadToAzure(model.Image1) ?? null;
                    var imageName2 = _imageService.UploadToAzure(model.Image2) ?? null;
                    var imageName3 = _imageService.UploadToAzure(model.Image3) ?? null;
                    var imageName4 = _imageService.UploadToAzure(model.Image4) ?? null;

                    if (!string.IsNullOrEmpty(imageName1)) images.Add(new ImageProductVm(imageName1));
                    if (!string.IsNullOrEmpty(imageName2)) images.Add(new ImageProductVm(imageName2));
                    if (!string.IsNullOrEmpty(imageName3)) images.Add(new ImageProductVm(imageName3));
                    if (!string.IsNullOrEmpty(imageName4)) images.Add(new ImageProductVm(imageName4));

                    if (images.Count > 0)
                    {
                        var mainImage = images.FirstOrDefault();
                        mainImage.IsMain = true;
                        model.MainImageName = mainImage.Name;
                        model.Id = _productService.AddProduct(model);
                        images.ForEach(x => x.ProductId = model.Id);
                        _imageService.AddImages(images);
                    }

                    if (businessAction == "publish")
                        SetMessage("Ogłoszenie w trakcie weryfikacji", MessageType.Success);
                    else
                        SetMessage("Ogłoszenie zapisano w wersji roboczej", MessageType.Success);

                    return RedirectToAction("MyProducts", new { id = model.OwnerId });
                }
                catch (Exception ex)
                {
                    throw new Exception("Add/Edit Product Failed " + ex);
                }
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
            return RedirectToAction("MyProducts", new { id = _productService.GetOwnerIdByProductId(id) });
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

        [HttpGet]
        public IActionResult MyFavourites(int? actualPage)
        {
            int pageSize = SystemConfiguration.DefaultPageSize;
            var favouriteIds = !string.IsNullOrEmpty(HttpContext.Session.GetString("favourite")) ?
                HttpContext.Session.GetString("favourite").Split(',') :
                null;

            if (favouriteIds != null)
            {
                var products = _productService.GetMyFavourites(favouriteIds, actualPage, pageSize);
                return View("MyFavourites", products);
            }
            else
            {
                return View("MyFavourites", new ListFavouritProducts() { Count = 0 });
            }
        }

        [HttpPost]
        public IActionResult AddFavourite(int productId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("favourite")))
            {
                HttpContext.Session.SetString("favourite", productId.ToString());
            }
            else
            {
                var favouriteIds = HttpContext.Session.GetString("favourite").Split(',').ToList();
                favouriteIds.Add(productId.ToString());
                HttpContext.Session.SetString("favourite", string.Join(",", favouriteIds));
            }

            HttpContext.Session.SetInt32($"favourite_{productId}", productId);
            return Json(new { Success = true });
        }

        [HttpPost]
        public IActionResult RemoveFavourite(int productId)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("favourite")))
            {
                var favouriteIds = HttpContext.Session.GetString("favourite").Split(',').ToList();
                var newFavouriteIds = favouriteIds.Where(x => x != productId.ToString());
                HttpContext.Session.SetString("favourite", string.Join(",", newFavouriteIds));
            }

            HttpContext.Session.Remove($"favourite_{productId}");
            return Json(new { IsAdd = true });
        }
    }
}
