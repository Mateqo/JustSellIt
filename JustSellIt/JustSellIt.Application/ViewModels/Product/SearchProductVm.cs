using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class SearchProductVm
    {
        public int PageSize { get; set; } = SystemConfiguration.DefaultPageSize;
        public int? ActualPage { get; set; }
        public string SearchString { get; set; }
        public string SearchLocation { get; set; }
        public bool IsNewSearch { get; set; }
    }
}
