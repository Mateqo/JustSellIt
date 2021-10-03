using JustSellIt.Application.Mapping;
using System;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ProductForListVm : IMapFrom<JustSellIt.Domain.Model.Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public bool IsFavorite { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime CreateDate { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Product, ProductForListVm>()
                .ForMember(s => s.Status, opt => opt.MapFrom(d => d.ProductStatus.Name))
                .ForMember(s => s.MainImageUrl, opt => opt.MapFrom(d => SystemConfiguration.ProductImageUrl.Replace("{{name}}", d.MainImageName)));
        }

    }
}
