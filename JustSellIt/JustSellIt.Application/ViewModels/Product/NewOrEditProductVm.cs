﻿using FluentValidation;
using JustSellIt.Application.Mapping;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JustSellIt.Application.ViewModels.Product
{
    public class NewOrEditProductVm : IMapFrom<JustSellIt.Domain.Model.Product>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PriceField { get; set; }
        public decimal Price { get; set; }
        public bool IsNegotiate { get; set; }
        public bool IsNew { get; set; }
        public string CategoryImage { get; set; }
        public string CategoryName { get; set; }
        public string Location { get; set; }
        public string PhoneContact { get; set; }
        public int CategoryId { get; set; }
        public int OwnerId { get; set; }
        public int ProductStatusId { get; set; }
        public bool StorePolicy { get; set; }
        public DateTime CreateDate { get; set; }
        public List<CategoryProductVm> Categories { get; set; }
        public string MainImageName { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
        public IFormFile Image3 { get; set; }
        public IFormFile Image4 { get; set; }
        public string Action { get; set; }
        public string UserGuid { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public int SexId { get; set; }
        public string AvatarUrl { get; set; }

        public void Mapping(MappingProfile profile)
        {
            profile.CreateMap<NewOrEditProductVm, JustSellIt.Domain.Model.Product>().ReverseMap();
        }

    }

    public class NewOrEditProductValidation : AbstractValidator<NewOrEditProductVm>
    {
        public NewOrEditProductValidation()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Title).NotNull().WithMessage("Tytuł jest wymagany");
            RuleFor(x => x.Title).MinimumLength(3).WithMessage("Minimalna długość to 3");
            RuleFor(x => x.Title).MaximumLength(20).WithMessage("Maksymalna długosć to 20");

            RuleFor(x => x.Description).NotNull().WithMessage("Opis jest wymagany");
            RuleFor(x => x.Description).MinimumLength(10).WithMessage("Minimalna długość to 10");
            RuleFor(x => x.Description).MaximumLength(600).WithMessage("Maksymalna długosć to 600");

            RuleFor(x => x.PriceField).NotNull().NotEmpty().WithMessage("Cena jest wymagana");
            RuleFor(x => x.PriceField).Must(BeAValidPrice).WithMessage("Nieprawidłowy format ceny, oczekiwany np. 37,48");

            RuleFor(x => x.CategoryId).NotNull().WithMessage("Wybierz kategorię");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Wybierz kategorię");

            RuleFor(x => x.StorePolicy).Equal(true).WithMessage("Wymagana jest akceptacja regulaminu");

            RuleFor(x => x.Location).NotNull().WithMessage("Wybierz miasto");
            RuleFor(x => x.Location).MinimumLength(2).WithMessage("Minimalna długość to 2");
            RuleFor(x => x.Location).MaximumLength(25).WithMessage("Maksymalna długosć to 25");
            RuleFor(x => x.Location).Must(BeAValidCity).WithMessage("Nieprawidłowy format miasta");

            RuleFor(x => x.PhoneContact).NotNull().WithMessage("Podaj numer kontaktowy");
            RuleFor(x => x.PhoneContact).Must(BeAValidPhone).WithMessage("Podano nieprawidłowy format");

            decimal value;
            When(x => Decimal.TryParse(x.PriceField, out value), () =>
            {
                //Oddam
                When(x => x.CategoryId != 12, () =>
                {
                    RuleFor(x => Convert.ToDecimal(x.PriceField)).GreaterThan(0).WithMessage("Przy tej kategori cena musi być większa od 0zł").OverridePropertyName("PriceField"); 
                });

                //Oddam
                When(x => x.CategoryId == 12, () =>
                {
                    RuleFor(x => Convert.ToDecimal(x.PriceField)).Equal(0).WithMessage("Przy tej kategori cena musi być równa 0zł").OverridePropertyName("PriceField");
                });
            });
        }

        private bool BeAValidPrice(string price)
        {
            Regex properPrice = new Regex(@"^\d+(,\d{2})?$");
            if (properPrice.IsMatch(price.ToString()))
                return true;
            else
                return false;
        }

        private bool BeAValidPhone(string number)
        {
            if (number != null)
            {
                Regex properNumber = new Regex(@"^[0-9]{9}$");
                if (properNumber.IsMatch(number.ToString()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private bool BeAValidCity(string location)
        {
            if (location != null)
            {
                Regex properNumber = new Regex(@"^[A-Z a-z żźćńółęąśŻŹĆĄŚĘŁÓŃ]*$");
                if (properNumber.IsMatch(location.ToString()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
