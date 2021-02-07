using JustSellIt.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ProductForListVm : IMapFrom<JustSellIt.Domain.Model.Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public DateTime CreatedOn { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Product, ProductForListVm>();
        }

    }
}
