
using JustSellIt.Application.Interfaces;
using System.Collections.Generic;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ListProductForListVm : IPagination, ISearch, IProductList
    {
        public List<ProductForListVm> Products { get; set; }
        public List<CategoryProductVm> Categories { get; set; }
        public int PageSize { get; set; } 
        public int? ActualPage { get; set; }
        public bool IsNewSearch { get; set; }
        public int Count { get; set; }
        public string SearchString { get; set; }
        public string SearchLocation { get; set; }
        public int? SearchCategory { get; set; }
        public int? SearchMinPrice { get; set; }
        public int? SearchMaxPrice { get; set; }
        public string SearchCondition { get; set; }
        public string Sorting { get; set; }
    }

}
