﻿using JustSellIt.Application.Mapping;
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
        public string UserGuid { get; set; }
        public string Owner { get; set; }
        public string OwnerId { get; set; }
        public string PhoneContact { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OwnerCreateDate { get; set; }
        public string[] Images { get; set; }
        public string AvatarUrl { get; set; }
        public int SexId { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<JustSellIt.Domain.Model.Product, ProductDetailsVm>()
                .ForMember(s => s.Category, opt => opt.MapFrom(d => d.Category.Name))
                .ForMember(s => s.Owner, opt => opt.MapFrom(d => d.Owner.Name))
                .ForMember(s => s.SexId, opt => opt.MapFrom(d => d.Owner.SexId))
                .ForMember(s => s.OwnerCreateDate, opt => opt.MapFrom(d => d.Owner.CreateDate))
                .ForMember(s => s.AvatarUrl, opt => opt.MapFrom(d => d.Owner.AvatarImage))
                .ForMember(s => s.Status, opt => opt.MapFrom(d => d.ProductStatus.Name));
        }
    }
}
