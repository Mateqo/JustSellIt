﻿using JustSellIt.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class ProductForListVm : IMapFrom<JustSellIt.Domain.Model.Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }
        public string City { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Product, ProductForListVm>()
                .ForMember(s => s.Category, opt => opt.MapFrom(d => d.Category.Name))
                .ForMember(s => s.Owner, opt => opt.MapFrom(d => d.Owner.Name))
                .ForMember(s => s.City, opt => opt.MapFrom(d => d.Owner.City));
        }

    }
}