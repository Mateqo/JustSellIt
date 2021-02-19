using JustSellIt.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ListProductForAdminListVm : IPagination, IProductList
    {
        public List<ProductForListVm> Products { get; set; }
        public int PageSize { get; set; } 
        public int? ActualPage { get; set; }
        public bool IsNewSearch { get; set; }
        public int Count { get; set; }
    }
}
