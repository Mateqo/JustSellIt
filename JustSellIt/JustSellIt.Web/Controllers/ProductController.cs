using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustSellIt.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerContactService _ownerContactService;
        private readonly IStatusService _statusService;
        private readonly IImageService _imageService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService,
            IStatusService statusService,
            IImageService imageService,
            UserManager<IdentityUser> userManager,
            IOwnerService ownerService,
            IOwnerContactService ownerContactService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _statusService = statusService;
            _imageService = imageService;
            _userManager = userManager;
            _ownerService = ownerService;
            _ownerContactService = ownerContactService;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var model = _productService.GetLatesProducts();

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult SearchProducts(string searchString, string searchLocation, int? searchCategory, int? searchMinPrice, int? searchMaxPrice, string searchCondition, string sorting, bool isNewSearch, int? actualPage)
        {
            try
            {
                int pageSize = SystemConfiguration.DefaultPageSize;
                var model = _productService.GetAllProduct(searchString, searchLocation, searchCategory, searchMinPrice,
                    searchMaxPrice, searchCondition, sorting, isNewSearch, pageSize, actualPage);


                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            try
            {
                var model = _productService.GetProductDetails(id);

                if (model != null && model.Status != "Published" && !HttpContext.User.Identity.IsAuthenticated && model.UserGuid != _userManager.GetUserId(HttpContext.User))
                    return View("Error");

                if (model == null)
                    return View("Error");

                model.AvatarUrl = model.AvatarUrl == null ? null : SystemConfiguration.OwnerImageUrl.Replace("{{name}}", model.AvatarUrl);
                model.Images = _imageService.GetImages(id)
                    .OrderBy(x => x.Position)
                    .Select(x => SystemConfiguration.ProductImageUrl.Replace("{{name}}", x.Name))
                    .ToArray();

                return View("ProductDetails", model);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddProduct()
        {
            try
            {
                var useGuid = _userManager.GetUserId(HttpContext.User);
                var owner = _ownerService.GetOwnerByGuid(useGuid);
                var ownerContact = _ownerContactService.GetOwnerContactByOwner(owner.Id);

                NewOrEditProductVm newProduct = new NewOrEditProductVm()
                {
                    ProductStatusId = _statusService.GetIdBeforeNew(),
                    Categories = _productService.GetAllCategory(),
                    Action = "AddProduct",
                    PriceField = "0,00",
                    AvatarUrl = owner.AvatarImage == null ? null : SystemConfiguration.OwnerImageUrl.Replace("{{name}}", owner.AvatarImage),
                    OwnerName = owner.Name,
                    Email = ownerContact.Email,
                    Location = owner.City,
                    OwnerId = owner.Id,
                    PhoneContact = ownerContact.PhoneNumber
                };

                return View("AddOrEditProduct", newProduct);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(NewOrEditProductVm model, string businessAction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ProductStatusId = businessAction == "publish" ? _statusService.GetIdForVeryfication() : _statusService.GetIdDraft();
                    model.CreateDate = DateTime.Now;
                    model.UserGuid = _userManager.GetUserId(HttpContext.User);
                    model.Location = StringHelper.CapitalizeFirstLetter(model.Location);

                    List<ImageProductVm> images = new List<ImageProductVm>();

                    var imageName1 = _imageService.UploadProductToAzure(model.Image1) ?? null;
                    var imageName2 = _imageService.UploadProductToAzure(model.Image2) ?? null;
                    var imageName3 = _imageService.UploadProductToAzure(model.Image3) ?? null;
                    var imageName4 = _imageService.UploadProductToAzure(model.Image4) ?? null;

                    if (!string.IsNullOrEmpty(imageName1)) images.Add(new ImageProductVm(imageName1, 1));
                    if (!string.IsNullOrEmpty(imageName2)) images.Add(new ImageProductVm(imageName2, 2));
                    if (!string.IsNullOrEmpty(imageName3)) images.Add(new ImageProductVm(imageName3, 3));
                    if (!string.IsNullOrEmpty(imageName4)) images.Add(new ImageProductVm(imageName4, 4));

                    if (images.Count > 0)
                    {
                        var mainImage = images.FirstOrDefault();
                        mainImage.IsMain = true;
                        model.MainImageName = mainImage.Name;
                    }

                    model.Id = _productService.AddProduct(model);
                    images.ForEach(x => x.ProductId = model.Id);
                    _imageService.AddImages(images);

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
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProduct(int id)
        {
            try
            {
                var product = _productService.GetProductForEdit(id);

                if (product.UserGuid != _userManager.GetUserId(HttpContext.User))
                    return View("Error");

                product.CategoryImage = $"/images/categories/{_productService.GetImageCategoryById(product.CategoryId)}";
                product.CategoryName = _productService.GetNameCategoryById(product.CategoryId);
                product.Categories = _productService.GetAllCategory();
                product.Action = "EditProduct";
                product.PriceField = product.Price.ToString();
                var images = _imageService.GetImages(id);

                var owner = _ownerService.GetOwnerByGuid(_userManager.GetUserId(HttpContext.User));
                var ownerContact = _ownerContactService.GetOwnerContactByOwner(owner.Id);

                product.AvatarUrl = owner.AvatarImage == null ? null : SystemConfiguration.OwnerImageUrl.Replace("{{name}}", owner.AvatarImage);
                product.OwnerName = owner.Name;
                product.Email = ownerContact.Email;
                product.OwnerId = owner.Id;

                product.ImageUrl1 = images.Any(x => x.Position == 1) ?
                    SystemConfiguration.ProductImageUrl.Replace("{{name}}", images.FirstOrDefault(x => x.Position == 1).Name) : null;
                product.ImageUrl2 = images.Any(x => x.Position == 2) ?
                 SystemConfiguration.ProductImageUrl.Replace("{{name}}", images.FirstOrDefault(x => x.Position == 2).Name) : null;
                product.ImageUrl3 = images.Any(x => x.Position == 3) ?
                 SystemConfiguration.ProductImageUrl.Replace("{{name}}", images.FirstOrDefault(x => x.Position == 3).Name) : null;
                product.ImageUrl4 = images.Any(x => x.Position == 4) ?
                 SystemConfiguration.ProductImageUrl.Replace("{{name}}", images.FirstOrDefault(x => x.Position == 4).Name) : null;

                return View("AddOrEditProduct", product);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(NewOrEditProductVm model, string businessAction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UserGuid != _userManager.GetUserId(HttpContext.User))
                        return View("Error");
                    if (model.ProductStatusId != _statusService.GetIdPublished())
                        model.CreateDate = DateTime.Now;
                    model.Location = StringHelper.CapitalizeFirstLetter(model.Location);
                    model.UserGuid = _userManager.GetUserId(HttpContext.User);
                    model.ProductStatusId = businessAction == "publish" ? _statusService.GetIdForVeryfication() : _statusService.GetIdDraft();

                    var imageName1 = _imageService.UploadProductToAzure(model.Image1) ?? "";
                    var imageName2 = _imageService.UploadProductToAzure(model.Image2) ?? "";
                    var imageName3 = _imageService.UploadProductToAzure(model.Image3) ?? "";
                    var imageName4 = _imageService.UploadProductToAzure(model.Image4) ?? "";

                    if (string.IsNullOrEmpty(model.ImageUrl1))
                    {
                        var imageToDelete = _imageService.GetImageByPosition(model.Id, 1);
                        if (imageToDelete != null)
                        {
                            _imageService.DeleteImage(imageToDelete.Id);
                            _imageService.DeleteImageProductFromAzure(imageToDelete.Name);
                        }
                    }
                    if (string.IsNullOrEmpty(model.ImageUrl2))
                    {
                        var imageToDelete = _imageService.GetImageByPosition(model.Id, 2);
                        if (imageToDelete != null)
                        {
                            _imageService.DeleteImage(imageToDelete.Id);
                            _imageService.DeleteImageProductFromAzure(imageToDelete.Name);
                        }
                    }
                    if (string.IsNullOrEmpty(model.ImageUrl3))
                    {
                        var imageToDelete = _imageService.GetImageByPosition(model.Id, 3);
                        if (imageToDelete != null)
                        {
                            _imageService.DeleteImage(imageToDelete.Id);
                            _imageService.DeleteImageProductFromAzure(imageToDelete.Name);
                        }
                    }
                    if (string.IsNullOrEmpty(model.ImageUrl4))
                    {
                        var imageToDelete = _imageService.GetImageByPosition(model.Id, 4);
                        if (imageToDelete != null)
                        {
                            _imageService.DeleteImage(imageToDelete.Id);
                            _imageService.DeleteImageProductFromAzure(imageToDelete.Name);
                        }
                    }

                    List<ImageProductVm> images = new List<ImageProductVm>();
                    if (model.Image1 != null) images.Add(new ImageProductVm(imageName1, 1));
                    if (model.Image2 != null) images.Add(new ImageProductVm(imageName2, 2));
                    if (model.Image3 != null) images.Add(new ImageProductVm(imageName3, 3));
                    if (model.Image4 != null) images.Add(new ImageProductVm(imageName4, 4));

                    images.ForEach(x => x.ProductId = model.Id);
                    _imageService.UpdateImages(images, model.Id);

                    model.MainImageName = _imageService.GetImages(model.Id)
                        .FirstOrDefault(x => x.IsMain == true)?.Name ?? null;

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
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                if (_productService.GetProductDetails(id).UserGuid != _userManager.GetUserId(HttpContext.User))
                    return View("Error");
                _productService.DeleteProduct(id);
                var images = _imageService.GetImages(id);
                _imageService.DeleteImages(id);

                foreach (var image in images)
                {
                    _imageService.DeleteImageProductFromAzure(image.Name);
                }

                SetMessage("Ogłoszenie zostało usunięte", MessageType.Error);
                return RedirectToAction("MyProducts", new { id = _productService.GetOwnerIdByProductId(id) });
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }

        }

        [HttpGet]
        public IActionResult AutoCompleteString(string text)
        {
            try
            {
                List<string> autoComplete = _productService.AutoCompleteString(text);
                var result = autoComplete.Select(x => new { id = x, label = x, value = x }).ToList();

                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult AutoCompleteLocation(string text)
        {
            try
            {
                List<string> autoComplete = _productService.AutoCompleteLocation(text);
                var result = autoComplete.Select(x => new { id = x, label = x, value = x }).ToList();

                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult GetContactInfo(int id)
        {
            try
            {
                var contact = _productService.GetContactById(id);

                return Json(new { data = contact });
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult GetOwnerProducts(int id, int? actualPage)
        {
            try
            {
                int pageSize = SystemConfiguration.DefaultPageSize;
                var model = _productService.GetOwnerProducts(id, actualPage, pageSize);
                if (model == null)
                    return View("Error");

                model.AvatarUrl = model.AvatarUrl == null ? null : SystemConfiguration.OwnerImageUrl.Replace("{{name}}", model.AvatarUrl);
                model.Action = "GetOwnerProducts";

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyProducts(int? actualPage)
        {
            try
            {
                var userGuid = _userManager.GetUserId(HttpContext.User);

                int pageSize = SystemConfiguration.DefaultPageSize;
                var model = _productService.GetMyProducts(userGuid, actualPage, pageSize);
                model.AvatarUrl = model.AvatarUrl == null ? null : SystemConfiguration.OwnerImageUrl.Replace("{{name}}", model.AvatarUrl);
                model.Action = "MyProducts";

                return View("GetOwnerProducts", model);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult MyFavourites(int? actualPage)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult AddFavourite(int productId)
        {
            try
            {
                var favouriteNumber = 0;
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("favourite")))
                {
                    HttpContext.Session.SetString("favourite", productId.ToString());
                    HttpContext.Session.SetInt32("favouriteNumber", 1);
                    favouriteNumber = 1;
                }
                else
                {
                    var favouriteIds = HttpContext.Session.GetString("favourite").Split(',').ToList();
                    favouriteIds.Add(productId.ToString());
                    HttpContext.Session.SetString("favourite", string.Join(",", favouriteIds));
                    HttpContext.Session.SetInt32("favouriteNumber", favouriteIds.Count());
                    favouriteNumber = favouriteIds.Count();
                }

                HttpContext.Session.SetInt32($"favourite_{productId}", productId);
                return Json(new { Success = true, FavouriteNumber = favouriteNumber });
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult RemoveFavourite(int productId)
        {
            try
            {
                var favouriteNumber = 0;

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("favourite")))
                {
                    var favouriteIds = HttpContext.Session.GetString("favourite").Split(',').ToList();
                    var newFavouriteIds = favouriteIds.Where(x => x != productId.ToString());
                    HttpContext.Session.SetString("favourite", string.Join(",", newFavouriteIds));
                    HttpContext.Session.SetInt32("favouriteNumber", newFavouriteIds.Count());
                    favouriteNumber = newFavouriteIds.Count();
                }

                HttpContext.Session.Remove($"favourite_{productId}");
                return Json(new { IsAdd = true, FavouriteNumber = favouriteNumber });
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }
    }
}
