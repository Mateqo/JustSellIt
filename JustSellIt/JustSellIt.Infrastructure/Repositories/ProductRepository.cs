using JustSellIt.Domain.Interface;
using JustSellIt.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        public bool RejectProduct(int productId,string reason)
        {
            var item = _context.Products.Find(productId);
            if (item != null)
            {
                item.ProductStatusId = _context.ProductStatuses.FirstOrDefault(x => x.Name == "Rejected").Id;
                item.RejectionReason = reason;
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool AcceptProduct(int productId)
        {
            var item = _context.Products.Find(productId);
            if (item != null)
            {
                item.ProductStatusId = _context.ProductStatuses.FirstOrDefault(x => x.Name == "Published").Id;
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
            var product = _context.Products.Include(x=>x.Owner).Include(x=>x.Category).Include(x => x.ProductStatus).FirstOrDefault(x => x.Id == productId);

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
            var products = _context.Products.Include(x=>x.ProductStatus).Where(x => x.ProductStatus.Name != "Deleted");

            return products;
        }

        public IQueryable<Owner> GetAllOwners()
        {
            var owners = _context.Owners;

            return owners;
        }

        public Owner GetOwnerById(int ownerId)
        {
            var owner = _context.Owners.FirstOrDefault(x => x.Id == ownerId);

            return owner;
        }

        public void UpdateProduct(Product product)
        {
            //Reset reason because product is update
            product.RejectionReason = null;
            _context.Attach(product);
            _context.Entry(product).Property("Title").IsModified = true;
            _context.Entry(product).Property("Description").IsModified = true;
            _context.Entry(product).Property("Price").IsModified = true;
            _context.Entry(product).Property("CategoryId").IsModified = true;
            _context.Entry(product).Property("ProductStatusId").IsModified = true;
            _context.Entry(product).Property("IsNegotiate").IsModified = true;
            _context.Entry(product).Property("IsNew").IsModified = true;
            _context.Entry(product).Property("Location").IsModified = true;
            _context.Entry(product).Property("PhoneContact").IsModified = true;
            _context.Entry(product).Property("CreatedOn").IsModified = true;
            _context.Entry(product).Property("RejectionReason").IsModified = true;
            _context.SaveChanges();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(x=>x.Id==id);
        }

        public void ClearReason(int id)
        {
            var product = _context.Products.Find(id);
            product.RejectionReason = null;
            _context.SaveChanges();
        }
    }
}
