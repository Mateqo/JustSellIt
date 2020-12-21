using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustSellIt.Infrastructure.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly Context _context;

        public ProductRepository(Context context)
        {
            _context = context;
        }

        public bool DeleteProduct(int productId)
        {
            var item = _context.Products.Find(productId);
            if(item!=null)
            {
                item.ProductStatusId = _context.ProductStatuses.FirstOrDefault(x => x.Name == "Deleted").Id;
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public int AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return product.Id;
        }

        public IQueryable<Product> GetProductsByCategoryId(int categoryId)
        {
            var products = _context.Products.Where(x => x.CategoryId == categoryId);

            return products;
        }

        public Product GetProductById(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);

            return product;
        }

        public IQueryable<Category> GetAllCategory()
        {
            var categories = _context.Categories;

            return categories;
        }

        public IQueryable<ProductStatus> GetAllStatuses()
        {
            var statuses = _context.ProductStatuses;

            return statuses;
        }

        public IQueryable<Product> GetAllProducts()
        {
            var products = _context.Products.Where(x => x.ProductStatus.Name != "Deleted");

            return products;
        }

        public IQueryable<Owner> GetAllOwners()
        {
            var owners = _context.Owners;

            return owners;
        }

    }
}
