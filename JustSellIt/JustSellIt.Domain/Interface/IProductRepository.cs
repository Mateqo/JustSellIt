﻿using JustSellIt.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustSellIt.Domain.Interface
{
    public interface IProductRepository
    {
        bool DeleteProduct(int productId);
        int AddProduct(Product product);
        IQueryable<Product> GetProductsByCategoryId(int categoryId);
        Product GetProductById(int productId);
        IQueryable<Category> GetAllCategory();
        IQueryable<Product> GetAllProducts();
        IQueryable<Owner> GetAllOwners();

    }
}