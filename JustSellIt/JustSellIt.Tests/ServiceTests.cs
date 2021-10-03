using AutoMapper;
using FluentAssertions;
using JustSellIt.Application;
using JustSellIt.Application.Mapping;
using JustSellIt.Application.Services;
using JustSellIt.Application.ViewModels.Product;
using JustSellIt.Domain.Model;
using JustSellIt.Infrastructure;
using JustSellIt.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JustSellIt.Tests
{
    public class ServiceTests
    {
        private ProductService BasicConfiguration(Context context)
        {
            context.Database.EnsureDeleted();

            Owner owner = new Owner()
            {
                Id = 1,
                Name = "Test",
                CreateDate = DateTime.Now,
                SexId = 1,
                UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
            };

            context.Owners.Add(owner);

            Category category = new Category()
            {
                Id = 1,
                Name = "Home",
                ColorHex = "white",
                Image = "house.png"
            };

            context.Categories.Add(category);

            Sex sex = new Sex()
            {
                Id = 1,
                Name = "Male"
            };

            context.Sex.Add(sex);

            ProductStatus productStatus = new ProductStatus()
            {
                Id = 1,
                Name = "Published"
            };

            context.ProductStatuses.Add(productStatus);

            ProductRepository productRepository = new ProductRepository(context);
            var mockAutoMapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider;
            var mapper = new Mapper(mockAutoMapper);
            return new ProductService(productRepository, mapper);
        }

        [Fact]
        public void CanAddProduct()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Id = 1,
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                var id = productService.AddProduct(productVm);
                var product = productService.GetProductDetails(id);

                //Assert
                product.Should().NotBeNull();
                product.Title.Should().Be("Lorem ipsum");
                product.Id.Should().Be(id);
            }
        }

        [Fact]
        public void CanGetAllProduct()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                NewOrEditProductVm productVm2 = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum 2",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                productService.AddProduct(productVm2);
                var products = productService.GetAllProduct(null, null, null, null, null, null, null, false, SystemConfiguration.DefaultPageSize, null);

                //Assert
                products.Should().NotBeNull();
                products.Count.Should().Be(2);
            }
        }

        [Fact]
        public void CanGetOwnerProduct()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                NewOrEditProductVm productVm2 = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum 2",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 2,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "909b9f42-6e4f-4336-80d2-1d3c360aac31"
                };

                Owner owner = new Owner()
                {
                    Id = 2,
                    Name = "Test",
                    CreateDate = DateTime.Now,
                    SexId = 1,
                    UserGuid = "909b9f42-6e4f-4336-80d2-1d3c360aac31"
                };

                context.Owners.Add(owner);

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                productService.AddProduct(productVm2);
                var productsOwner1 = productService.GetOwnerProducts(productVm.OwnerId, null, SystemConfiguration.DefaultPageSize);
                var productsOwner2 = productService.GetOwnerProducts(productVm2.OwnerId, null, SystemConfiguration.DefaultPageSize);

                //Assert
                productsOwner1.Should().NotBeNull();
                productsOwner2.Should().NotBeNull();
                productsOwner1.Count.Should().Be(1);
                productsOwner2.Count.Should().Be(1);
            }
        }

        [Fact]
        public void CanGetMyProduct()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                Owner owner = new Owner()
                {
                    Id = 2,
                    Name = "Test",
                    CreateDate = DateTime.Now,
                    SexId = 1,
                    UserGuid = "909b9f42-6e4f-4336-80d2-1d3c360aac31"
                };

                context.Owners.Add(owner);

                NewOrEditProductVm productVm2 = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum 2",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 2,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "909b9f42-6e4f-4336-80d2-1d3c360aac31"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                productService.AddProduct(productVm2);
                var myProducts = productService.GetMyProducts(productVm.UserGuid, null, SystemConfiguration.DefaultPageSize);

                //Assert
                myProducts.Should().NotBeNull();
                myProducts.Count.Should().Be(1);
            }
        }

        [Fact]
        public void CanGetLatestProduct()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                NewOrEditProductVm productVm2 = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum 2",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now,
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "909b9f42-6e4f-4336-80d2-1d3c360aac31"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                productService.AddProduct(productVm2);
                var latestProducts = productService.GetLatesProducts();
                var theLatestProduct = latestProducts.Products.FirstOrDefault();

                //Assert
                theLatestProduct.Should().NotBeNull();
                theLatestProduct.Title.Should().Be("Lorem ipsum");
            }
        }

        [Fact]
        public void CanUpdateProduct()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                Product product = new Product()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    Price = 48.00M,
                    ProductStatusId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"                 
                };

                var productService = BasicConfiguration(context);

                //Act
                context.Products.Add(product);
                context.SaveChanges();
                context.Entry<Product>(product).State = EntityState.Detached;

                var productToUpdate = productService.GetMyProducts("6db7a2e8-564c-4ce3-be03-5892de03fba0",null,SystemConfiguration.DefaultPageSize)
                    .Products.FirstOrDefault();

                var update = new NewOrEditProductVm()
                {
                    Id = productToUpdate.Id,
                    Title = "Lorem ipsum Update",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };
                productService.UpdateProduct(update);
                var afterUpdate = productService.GetProductDetails(productToUpdate.Id);

                //Assert
                afterUpdate.Should().NotBeNull();
                afterUpdate.Title.Should().Be("Lorem ipsum Update");
            }
        }

        [Fact]
        public void CanGetAllCategory()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Lorem ipsum",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                var products = productService.GetLatesProducts();
                var categories = productService.GetAllCategory();


                //Assert
                categories.Should().NotBeNull();
                categories.Count.Should().Be(1);
                categories.FirstOrDefault().Name.Should().Be("Home");
            }
        }

        [Fact]
        public void CanAutoComplete()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                var autoCompleteString = productService.AutoCompleteString("lo");
                var autoCompleteLocation = productService.AutoCompleteLocation("wa");

                //Assert
                autoCompleteString.Should().NotBeNull();
                autoCompleteLocation.Should().NotBeNull();
                autoCompleteString.Count.Should().Be(1);
                autoCompleteLocation.Count.Should().Be(1);
                autoCompleteString.FirstOrDefault().Should().Be("Lorem ipsum");
                autoCompleteLocation.FirstOrDefault().Should().Be("Warszawa");
            }
        }

        [Fact]
        public void CanDeactivateProducts()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                NewOrEditProductVm productVm2 = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum 2",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                productService.AddProduct(productVm2);
                productService.DeactivateProducts("6db7a2e8-564c-4ce3-be03-5892de03fba0");
                var result = productService.GetOwnerProducts(1, null, SystemConfiguration.DefaultPageSize);

                //Assert
                result.Products.Should().BeNullOrEmpty();
            }
        }

        [Fact]
        public void CanGetContact()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);
                context.Database.EnsureDeleted();

                //Act
                var id = productService.AddProduct(productVm);
                var contact = productService.GetContactById(id);
                //Assert
                contact.Should().NotBeNullOrEmpty();
                contact.Should().Be("000000000");
            }
        }

        [Fact]
        public void CanGetNameCategory()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                productService.AddProduct(productVm);
                var categoryName = productService.GetNameCategoryById(productVm.CategoryId);
                var categoryImage = productService.GetImageCategoryById(productVm.CategoryId);

                //Assert
                categoryName.Should().NotBeNullOrEmpty();
                categoryImage.Should().NotBeNullOrEmpty();
                categoryName.Should().Be("Home");
                categoryImage.Should().Be("house.png");
            }
        }

        [Fact]
        public void CanGetOwner()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 1,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                //Act
                var id = productService.AddProduct(productVm);
                var owner = productService.GetOwnerIdByProductId(id);

                //Assert
                owner.Should().Be(productVm.OwnerId);
            }
        }

        [Fact]
        public void CanClearReason()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 3,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                ProductStatus rejected = new ProductStatus()
                {
                    Id = 2,
                    Name = "Rejected"
                };

                ProductStatus verify = new ProductStatus()
                {
                    Id = 3,
                    Name = "ForVeryfication"
                };

                context.ProductStatuses.Add(rejected);
                context.ProductStatuses.Add(verify);

                var productService = BasicConfiguration(context);

                ProductRepository productRepository = new ProductRepository(context);
                var mockAutoMapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider;
                var mapper = new Mapper(mockAutoMapper);
                AdminService adminService = new AdminService(productRepository, mapper);


                //Act
                var id = productService.AddProduct(productVm);
                var productToReject = adminService.GetProductsToVerify(null, SystemConfiguration.DefaultPageSize).Products.FirstOrDefault();
                adminService.RejectProduct(productToReject.Id, "Rejected reason");
                productService.ClearReason(productToReject.Id);
                var details = productService.GetProductDetails(productToReject.Id);

                //Assert
                details.RejectionReason.Should().BeNull();
            }
        }

        [Fact]
        public void CanUpdateImages()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "JustSellIt")
            .Options;

            using (var context = new Context(options))
            {
                NewOrEditProductVm productVm = new NewOrEditProductVm()
                {
                    Title = "Lorem ipsum",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean eleifend et lacus fringilla sollicitudin. Aliquam erat volutpat. Sed vel nisi placerat odio egestas efficitur vel eget lacus. Praesent vel nisi elit.",
                    CategoryId = 1,
                    CreateDate = DateTime.Now.AddDays(1),
                    IsNegotiate = true,
                    IsNew = true,
                    Location = "Warszawa",
                    OwnerId = 1,
                    PhoneContact = "000000000",
                    PriceField = "48",
                    ProductStatusId = 3,
                    SexId = 1,
                    StorePolicy = true,
                    UserGuid = "6db7a2e8-564c-4ce3-be03-5892de03fba0"
                };

                var productService = BasicConfiguration(context);

                var id = productService.AddProduct(productVm);

                ImageProductVm image1 = new ImageProductVm()
                {
                    Name = "Lorem ipsum 1",
                    Position = 4,
                    ProductId = id
                };

                ImageProductVm image2 = new ImageProductVm()
                {
                    Name = "Lorem ipsum 2",
                    Position = 2,
                    ProductId = id
                };

                ImageProductVm image3 = new ImageProductVm()
                {
                    Name = "Lorem ipsum 3",
                    Position = 3,
                    ProductId = id
                };

                ImageProductVm image4 = new ImageProductVm()
                {
                    Name = "Lorem ipsum 4",
                    Position = 1,
                    ProductId = id
                };

                var configuration = new Mock<IConfiguration>();
                ImageRepository imageRepository = new ImageRepository(context);
                var mockAutoMapper = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider;
                var mapper = new Mapper(mockAutoMapper);
                ImageService imageService = new ImageService(imageRepository, mapper, configuration.Object);

                //Act
                var listImages = new List<ImageProductVm>();
                listImages.Add(image1);
                listImages.Add(image2);
                listImages.Add(image3);
                imageService.UpdateImages(listImages,id);

                var listImages2 = new List<ImageProductVm>();
                listImages2.Add(image4);
                imageService.UpdateImages(listImages2, id);

                var list = imageService.GetImages(id);

                //Assert
                list.Where(x=>x.IsMain == true).FirstOrDefault().Name.Should().Be("Lorem ipsum 4");
            }
        }


    }
}
