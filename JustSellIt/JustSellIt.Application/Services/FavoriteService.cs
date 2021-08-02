using AutoMapper;
using AutoMapper.QueryableExtensions;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace JustSellIt.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public FavoriteService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        //public ListProductForListVm GetProductById(int id)
        //{
        //    _productRepo.GetProductById(id)
        //}


    }
}
