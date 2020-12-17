using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class NewOrEditProductVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
        public int ProductStatusId { get; set; }
        public bool StorePolicy { get; set; }
    }
}
