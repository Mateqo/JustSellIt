using AutoMapper;
using AutoMapper.QueryableExtensions;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ProductService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        public int AddProduct(NewOrEditProductVm productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            var id = _productRepo.AddProduct(product);

            return id;
        }

        public ListProductForListVm GetAllProduct(SearchProductVm searchProduct)
        {
            if (!searchProduct.ActualPage.HasValue || searchProduct.IsNewSearch)
                searchProduct.ActualPage = 1;

            if (searchProduct.SearchString is null)
                searchProduct.SearchString = String.Empty;

            if (searchProduct.SearchLocation is null)
                searchProduct.SearchLocation = String.Empty;

            var products = _productRepo.GetAllProducts().Where(x => x.Title.StartsWith(searchProduct.SearchString) && x.Location.StartsWith(searchProduct.SearchLocation));

            if (searchProduct.SearchCategory.HasValue)
                products = products.Where(x => x.CategoryId == searchProduct.SearchCategory);

            var productsAfterFiltrs= products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.OrderByDescending(x => x.CreatedOn).Skip((int)(searchProduct.PageSize * (searchProduct.ActualPage - 1))).Take(searchProduct.PageSize).ToList();

            var productList = new ListProductForListVm()
            {
                PageSize = searchProduct.PageSize,
                ActualPage = searchProduct.ActualPage,
                SearchString = searchProduct.SearchString,
                SearchLocation = searchProduct.SearchLocation,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
                Categories = GetAllCategory()
            };

            return productList;
        }

        public ListProductForListVm GetLatesProducts()
        {
            var products = _productRepo.GetAllProducts().ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = products.OrderByDescending(x => x.CreatedOn).Take(SystemConfiguration.DefaultNumberOfLatestProduct).ToList();

            var productList = new ListProductForListVm()
            {
                Products = productToShow,
                Count = products.Count,
                Categories = GetAllCategory()
            };

            return productList;
        }

        public ProductDetailsVm GetProductDetails(int productId)
        {
            var product = _productRepo.GetProductById(productId);
            var productVM = _mapper.Map<ProductDetailsVm>(product);

            return productVM;
        }

        public NewOrEditProductVm GetProductForEdit(int id)
        {
            var product = _productRepo.GetProductById(id);
            var productVM = _mapper.Map<NewOrEditProductVm>(product);

            return productVM;
        }

        public void UpdateProduct(NewOrEditProductVm model)
        {
            var product = _mapper.Map<Product>(model);
            _productRepo.UpdateProduct(product);
        }

        public void DeleteProduct(NewOrEditProductVm model)
        {
            var product = _mapper.Map<Product>(model);
            _productRepo.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepo.DeleteProduct(id);
        }

        public List<CategoryProductVm> GetAllCategory()
        {
            return _productRepo.GetAllCategory().ProjectTo<CategoryProductVm>(_mapper.ConfigurationProvider).ToList();
        }

        public List<string> AutoCompleteString(string text)
        {
            var listOfProducts = _productRepo.GetAllProducts().Where(x => x.Title.StartsWith(text)).Take(SystemConfiguration.DefaultNumberOfAutocompleteSearch);
            List<string> autoComplete = listOfProducts.Select(x => x.Title).Distinct().ToList();

            return autoComplete;
        }

        public List<string> AutoCompleteLocation(string text)
        {
            var listOfProducts = _productRepo.GetAllProducts().Where(x => x.Location.StartsWith(text)).Take(SystemConfiguration.DefaultNumberOfAutocompleteSearch);
            List<string> autoComplete = listOfProducts.Select(x => x.Location).Distinct().ToList();

            return autoComplete;
        }


    }
}
