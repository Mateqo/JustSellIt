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

        public ListProductForListVm GetAllProduct(string searchString, string searchLocation, int? searchCategory, int? actualPage, bool isNewSearch,int pageSize)
        {
            if (!actualPage.HasValue || isNewSearch)
                actualPage = 1;

            if (searchString is null)
                searchString = String.Empty;

            if (searchLocation is null)
                searchLocation = String.Empty;

            var products = _productRepo.GetAllProducts().Where(x => x.Title.StartsWith(searchString) && x.Location.StartsWith(searchLocation));

            if (searchCategory.HasValue)
                products = products.Where(x => x.CategoryId == searchCategory);

            var productsAfterFiltrs= products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.OrderByDescending(x => x.CreatedOn).Skip((int)(pageSize * (actualPage - 1))).Take(pageSize).ToList();

            var productList = new ListProductForListVm()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                SearchString = searchString,
                SearchLocation = searchLocation,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
                Categories = GetAllCategory()
            };

            return productList;
        }

        public ListOwnerProducts GetOwnerProducts(int ownerId,int? actualPage,int pageSize)
        {
            if (!actualPage.HasValue )
                actualPage = 1;

            var owner = _productRepo.GetOwnerById(ownerId);

            var products = _productRepo.GetAllProducts().Where(x => x.OwnerId == ownerId);

            var productsAfterFiltrs = products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.OrderByDescending(x => x.CreatedOn).Skip((int)(pageSize * (actualPage - 1))).Take(pageSize).ToList();

            var productList = new ListOwnerProducts()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
                Id = owner.Id,
                Owner = owner.Name,
                Location=owner.City
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

        public string GetContactById(int id)
        {
            return  _productRepo.GetProductById(id).PhoneContact;
        }

    }
}
