using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ListProductForListVm
    {
        public List<ProductForListVm> Products { get; set; }
        public int Count { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
