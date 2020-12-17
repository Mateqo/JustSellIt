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
            
        }

        public ListProductForListVm GetAllProduct()
        {
            var products = _productRepo.GetAllProducts().ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productList = new ListProductForListVm()
            {
                Products = products,
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
