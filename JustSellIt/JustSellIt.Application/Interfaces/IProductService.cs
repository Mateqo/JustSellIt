using JustSellIt.Application.ViewModels.Product;
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
    }
}
