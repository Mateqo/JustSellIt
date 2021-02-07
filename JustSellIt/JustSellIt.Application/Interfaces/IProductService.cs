using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface IProductService
    {
        ListProductForListVm GetAllProduct(string searchString, string searchLocation, int? searchCategory, int? actualPage, bool isNewSearch, int pageSize);
        int AddProduct(NewOrEditProductVm product);
        ProductDetailsVm GetProductDetails(int productId);
        NewOrEditProductVm GetProductForEdit(int id);
        void UpdateProduct(NewOrEditProductVm model);
        void DeleteProduct(int id);
        List<CategoryProductVm> GetAllCategory();
        ListProductForListVm GetLatesProducts();
        List<string> AutoCompleteString(string text);
        List<string> AutoCompleteLocation(string text);
        string GetContactById(int id);
        //ListProductForListVm GetOwnerProducts(int ownerId, SearchProductVm searchProduct);
    }
}
