using JustSellIt.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.Interfaces
{
    public interface IProductList
    {
        List<ProductForListVm> Products { get; set; }
    }
}
