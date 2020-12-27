using FluentValidation;
using JustSellIt.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Application.ViewModels.Product
{
    public class NewOrEditProductVm :IMapFrom<JustSellIt.Domain.Model.Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
        public int ProductStatusId { get; set; }
        public bool StorePolicy { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<NewOrEditProductVm, JustSellIt.Domain.Model.Product>().ReverseMap();
        }

    }

    public class NewOrEditProductValidation:AbstractValidator<NewOrEditProductVm>
    {
        public NewOrEditProductValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).Length(2,20).WithMessage("Nieprawidłowa długość");
            RuleFor(x => x.Title).NotNull().WithMessage("Nieprawidłowa długość");
            RuleFor(x => x.Description).MaximumLength(60);
        }
    }
}
