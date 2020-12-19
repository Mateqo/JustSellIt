using JustSellIt.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface IProductService
    {
        ListProductForListVm GetAllProduct(SearchProductVm searchProduct);
        int Add(NewOrEditProductVm product);
        ProductDetailsVm GetProductDetails(int productId);
    }
}
