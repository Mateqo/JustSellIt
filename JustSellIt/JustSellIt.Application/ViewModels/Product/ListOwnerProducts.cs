using JustSellIt.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ListOwnerProducts : IPagination, IProductList
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        //public DateTime CreatedOn { get; set; } TO DO 
        public List<ProductForListVm> Products { get; set; }
        public int PageSize { get; set; } 
        public int? ActualPage { get; set; }
        public int Count { get; set; }
        public bool IsNewSearch { get; set; }
        public string Action { get; set; }

    }
}
