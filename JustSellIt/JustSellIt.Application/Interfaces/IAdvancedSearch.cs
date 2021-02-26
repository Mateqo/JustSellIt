using System;
using System.Collections.Generic;
using System.Text;
using JustSellIt.Application.ViewModels.Product;

namespace JustSellIt.Application.Interfaces
{
    public interface IAdvancedSearch
    {
        int? SearchCategory { get; set; }
        int? SearchMinPrice { get; set; }
        int? SearchMaxPrice { get; set; }
        string SearchCondition { get; set; }
        string Sorting { get; set; }
        public List<CategoryProductVm> Categories { get; set; }
    }
}
