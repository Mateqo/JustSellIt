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
        ListProductForListVm GetAllProduct(SearchProductVm searchProduct);
        int AddProduct(NewOrEditProductVm product);
        ProductDetailsVm GetProductDetails(int productId);
        NewOrEditProductVm GetProductForEdit(int id);
        void UpdateProduct(NewOrEditProductVm model);
        void DeleteProduct(int id);
        List<CategoryProductVm> GetAllCategory();
        ListProductForListVm GetLatesProducts();
    }
}
