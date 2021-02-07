using JustSellIt.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ProductDetailsVm : IMapFrom<JustSellIt.Domain.Model.Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsNegotiate { get; set; }
        public bool IsNew { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }
        public string PhoneContact { get; set; }
        public string Location { get; set; }
        public DateTime CreatedOn { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Product, ProductDetailsVm>()
                .ForMember(s => s.Category, opt => opt.MapFrom(d => d.Category.Name))
                .ForMember(s => s.Owner, opt => opt.MapFrom(d => d.Owner.Name));
        }
    }
}
