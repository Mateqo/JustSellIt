using JustSellIt.Application.Interfaces;
using System.Collections.Generic;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ListFavouritProducts : IPagination, IProductList
    {
        public List<ProductForListVm> Products { get; set; }
        public int PageSize { get; set; }
        public int? ActualPage { get; set; }
        public int Count { get; set; }
        public bool IsNewSearch { get; set; }
    }
}
