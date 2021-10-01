using JustSellIt.Application.Interfaces;
using System.Collections.Generic;

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
        public string AvatarUrl { get; set; }
        public int SexId { get; set; }
        public string UserGuid { get; set; }     
    }
}
