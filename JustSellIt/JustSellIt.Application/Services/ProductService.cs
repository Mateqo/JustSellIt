using AutoMapper;
using AutoMapper.QueryableExtensions;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustSellIt.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo,IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        public int Add(NewOrEditProductVm product)
        {
            return 1;
        }

        public ListProductForListVm GetAllProduct(SearchProductVm searchProduct)
        {
            if (!searchProduct.ActualPage.HasValue)
            {
                searchProduct.ActualPage = 1;
            }
            if (searchProduct.SearchString is null)
            {
                searchProduct.SearchString = String.Empty;
            }

            var products = _productRepo.GetAllProducts().Where(x=>x.Title.StartsWith(searchProduct.SearchString))
                .ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = products.Skip((int)(searchProduct.PageSize * (searchProduct.ActualPage - 1))).Take(searchProduct.PageSize).ToList();

            var productList = new ListProductForListVm()
            {
                PageSize = searchProduct.PageSize,
                ActualPage = searchProduct.ActualPage,
                SearchString = searchProduct.SearchString,
                Products = productToShow,
                Count = products.Count
            };

            return productList;
        }

        public ProductDetailsVm GetProductDetails(int productId)
        {
            var product = _productRepo.GetProductById(productId);
            var productVM = _mapper.Map<ProductDetailsVm>(product);

            return productVM;
        }
    }
}
