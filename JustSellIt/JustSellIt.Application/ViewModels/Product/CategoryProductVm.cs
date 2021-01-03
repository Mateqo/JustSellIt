using JustSellIt.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class CategoryProductVm : IMapFrom<JustSellIt.Domain.Model.Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ColorHex { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Category, CategoryProductVm>();
        }
    }
}
