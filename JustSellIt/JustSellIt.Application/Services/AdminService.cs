using AutoMapper;
using AutoMapper.QueryableExtensions;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustSellIt.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public AdminService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public ListProductForAdminListVm GetProductsToVerify(int? actualPage, int pageSize)
        {
            if (!actualPage.HasValue)
                actualPage = 1;

            var products = _productRepo.GetAllProducts().Where(x =>x.ProductStatus.Name == "ForVeryfication");

            var productsAfterFiltrs = products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.OrderByDescending(x => x.CreatedOn).Skip((int)(pageSize * (actualPage - 1))).Take(pageSize).ToList();

            var productList = new ListProductForAdminListVm()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
            };

            return productList;
        }

        public void RejectProduct(int id,string reason)
        {
            _productRepo.RejectProduct(id,reason);
        }

        public void AcceptProduct(int id)
        {
            _productRepo.AcceptProduct(id);
        }
    }
}
