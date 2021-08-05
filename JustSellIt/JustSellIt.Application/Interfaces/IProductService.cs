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
        ListProductForListVm GetAllProduct(string searchString, string searchLocation, int? searchCategory, int? searchMinPrice, int? searchMaxPrice, string searchCondition, string sorting, bool isNewSearch, int pageSize, int? actualPage);
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
        ListOwnerProducts GetOwnerProducts(int ownerId, int? actualPage, int pageSize);
        ListOwnerProducts GetMyProducts(int ownerId, int? actualPage, int pageSize);
        string GetNameCategoryById(int id);
        string GetImageCategoryById(int id);
        int GetOwnerIdByProductId(int id);
        void ClearReason(int id);
        ListFavouritProducts GetMyFavourites(string[] favouritesIds, int? actualPage, int pageSize);
    }
}
