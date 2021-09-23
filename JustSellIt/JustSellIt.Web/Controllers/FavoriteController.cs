using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JustSellIt.Web.Controllers
{
    public class FavoriteController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IStatusService _statusService;

        public FavoriteController(IProductService productService, IStatusService statusService)
        {
            _productService = productService;
            _statusService = statusService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult AddFavorite(int productId)
        //{
        //    ProductForListVm product = new ProductForListVm();
        //    if (SessionHelper.GetObjectFromJson<List<ProductForListVm>>(HttpContext.Session, "favorites") == null)
        //    {
        //        List<ProductForListVm> favorites = new List<ProductForListVm>();
        //        favorites.Add();
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "favorites", favorites);
        //    }
        //    else
        //    {
        //        List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
        //        int index = isExist(id);
        //        if (index != -1)
        //        {
        //            cart[index].Quantity++;
        //        }
        //        else
        //        {
        //            cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
        //        }
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
