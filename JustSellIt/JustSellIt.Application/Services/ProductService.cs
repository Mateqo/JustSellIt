using AutoMapper;
using AutoMapper.QueryableExtensions;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustSellIt.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo, IMapper mapper, IImageService imageService)
        {
            _productRepo = productRepo;
            _imageService = imageService;
            _mapper = mapper;
        }
        public int AddProduct(NewOrEditProductVm productVM)
        {
            productVM.Price = Convert.ToDecimal(productVM.PriceField);
            var product = _mapper.Map<Product>(productVM);
            var id = _productRepo.AddProduct(product);

            return id;
        }

        public ListProductForListVm GetAllProduct(string searchString, string searchLocation, int? searchCategory, int? searchMinPrice, int? searchMaxPrice, string searchCondition, string sorting, bool isNewSearch, int pageSize, int? actualPage)
        {
            if (!actualPage.HasValue || isNewSearch)
                actualPage = 1;

            if (searchString is null)
                searchString = String.Empty;

            if (searchLocation is null)
                searchLocation = String.Empty;

            var products = _productRepo.GetAllProducts().Where(x => x.ProductStatus.Name == "Published").Where(x => x.Title.StartsWith(searchString) && x.Location.StartsWith(searchLocation));

            if (searchCategory.HasValue)
                products = products.Where(x => x.CategoryId == searchCategory);

            if (searchMinPrice.HasValue)
                products = products.Where(x => x.Price >= searchMinPrice);

            if (searchMaxPrice.HasValue)
                products = products.Where(x => x.Price <= searchMaxPrice);

            switch (searchCondition)
            {
                case "new":
                    products = products.Where(x => x.IsNew == true);
                    break;
                case "used":
                    products = products.Where(x => x.IsNew == false);
                    break;
            }
            switch (sorting)
            {
                case "new":
                    products = products.OrderByDescending(x => x.CreateDate);
                    break;
                case "asc":
                    products = products.OrderBy(x => x.Price);
                    break;
                case "desc":
                    products = products.OrderByDescending(x => x.Price);
                    break;
                default:
                    products = products.OrderByDescending(x => x.CreateDate);
                    break;
            }

            var productsAfterFiltrs = products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.Skip((int)(pageSize * (actualPage - 1))).Take(pageSize).ToList();

            var productList = new ListProductForListVm()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                SearchString = searchString,
                SearchLocation = searchLocation,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
                Categories = GetAllCategory(),
                SearchMinPrice = searchMinPrice,
                SearchMaxPrice = searchMaxPrice
            };

            return productList;
        }

        public ListOwnerProducts GetOwnerProducts(int ownerId, int? actualPage, int pageSize)
        {
            if (!actualPage.HasValue)
                actualPage = 1;

            var owner = _productRepo.GetOwnerById(ownerId);

            var products = _productRepo.GetAllProducts().Where(x => x.OwnerId == ownerId && x.ProductStatus.Name == "Published");

            var productsAfterFiltrs = products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.OrderByDescending(x => x.CreateDate).Skip((int)(pageSize * (actualPage - 1))).Take(pageSize).ToList();

            if (owner == null)
                return null;

            var productList = new ListOwnerProducts()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
                Id = owner.Id,
                Owner = owner.Name,
                Location = owner.City,
                SexId = owner.SexId,
                AvatarUrl = owner.AvatarImage,
                UserGuid = owner.UserGuid,
                CreateDate = owner.CreateDate
            };

            return productList;
        }

        public ListOwnerProducts GetMyProducts(string userGuid, int? actualPage, int pageSize)
        {
            if (!actualPage.HasValue)
                actualPage = 1;

            var owner = _productRepo.GetOwnerByGuid(userGuid);

            var products = _productRepo.GetAllProducts().Where(x => x.UserGuid == userGuid && x.ProductStatus.Name != "Deleted");

            var productsAfterFiltrs = products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs.OrderByDescending(x => x.CreateDate).Skip((int)(pageSize * (actualPage - 1))).Take(pageSize).ToList();

            var productList = new ListOwnerProducts()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
                Id = owner.Id,
                Owner = owner.Name,
                Location = owner.City,
                SexId = owner.SexId,
                AvatarUrl = owner.AvatarImage,
                UserGuid = owner.UserGuid,
                CreateDate = owner.CreateDate
            };

            return productList;
        }

        public ListFavouritProducts GetMyFavourites(string[] favouritesIds, int? actualPage, int pageSize)
        {
            if (!actualPage.HasValue)
                actualPage = 1;

            var products = _productRepo.GetAllProducts()
                .Where(x => favouritesIds.Contains(x.Id.ToString()) && x.ProductStatus.Name == "Published");

            var productsAfterFiltrs = products.ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = productsAfterFiltrs
                .OrderByDescending(x => x.CreateDate)
                .Skip((int)(pageSize * (actualPage - 1)))
                .Take(pageSize)
                .ToList();

            var productList = new ListFavouritProducts()
            {
                PageSize = pageSize,
                ActualPage = actualPage,
                Products = productToShow,
                Count = productsAfterFiltrs.Count,
            };

            return productList;
        }

        public ListProductForListVm GetLatesProducts()
        {
            var products = _productRepo.GetAllProducts().Where(x => x.ProductStatus.Name == "Published").ProjectTo<ProductForListVm>(_mapper.ConfigurationProvider).ToList();

            var productToShow = products.OrderByDescending(x => x.CreateDate).Take(SystemConfiguration.DefaultNumberOfLatestProduct).ToList();

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
            model.Price = Convert.ToDecimal(model.PriceField);
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
            var listOfProducts = _productRepo.GetAllProducts().Where(x => x.ProductStatus.Name == "Published").Where(x => x.Title.ToLower().Contains(text.ToLower())).Take(SystemConfiguration.DefaultNumberOfAutocompleteSearch);
            List<string> autoComplete = listOfProducts.Select(x => x.Title).Distinct().ToList();

            return autoComplete;
        }

        public List<string> AutoCompleteLocation(string text)
        {
            var listOfProducts = _productRepo.GetAllProducts().Where(x => x.ProductStatus.Name == "Published").Where(x => x.Location.ToLower().Contains(text.ToLower())).Take(SystemConfiguration.DefaultNumberOfAutocompleteSearch);
            List<string> autoComplete = listOfProducts.Select(x => x.Location).Distinct().ToList();

            return autoComplete;
        }

        public void DeactivateProducts(string userGuid)
        {
            var products = GetMyProducts(userGuid, null, int.MaxValue);
            foreach (var product in products.Products)
            {
                var images = _imageService.GetImages(product.Id);
                if (images != null)
                    _imageService.DeleteImages(product.Id);
                foreach (var item in images)
                {
                    _imageService.DeleteImageProductFromAzure(item.Name);
                }
            }

            _productRepo.DeactivateProducts(userGuid);
        }

        public string GetContactById(int id)
        {
            return _productRepo.GetProductById(id).PhoneContact;
        }

        public string GetNameCategoryById(int id)
        {
            return _productRepo.GetCategoryById(id).Name;
        }

        public string GetImageCategoryById(int id)
        {
            return _productRepo.GetCategoryById(id).Image;
        }

        public int GetOwnerIdByProductId(int id)
        {
            return _productRepo.GetProductById(id).OwnerId;
        }

        public void ClearReason(int id)
        {
            _productRepo.ClearReason(id);
        }

    }
}
