using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.Products.Contract;
using GeekBurger.Products.Contract.Model;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Products.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private ProductsDbContext _dbContext;
        

        public ProductsRepository(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
          
        }

        public Product GetProductById(Guid productId)
        {
            return _dbContext.Products?
                .Include(product => product.Items)
                .FirstOrDefault(product => product.ProductId == productId);
        }

        public List<Item> GetFullListOfItems()
        {
            return _dbContext.Items.ToList();
        }

        public bool Add(Product product)
        {
            product.ProductId = Guid.NewGuid();
            _dbContext.Products.Add(product);
            return true;
        }

        public bool Update(Product product)
        {
            return true;
        }

        public IEnumerable<Product> GetProductsByStoreName(string storeName)
        {
            var products = _dbContext.Products?
                .Where(product =>
                    product.Store.Name.Equals(storeName,
                    StringComparison.InvariantCultureIgnoreCase))
                .Include(product => product.Items);

            return products;
        }

        public void Delete(Product product)
        {
            _dbContext.Products.Remove(product);
        }

        public void Save()
        {
          
        }
    }
}