using GeekBurger.Products.Contract.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Products.Contract.Repository
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetProductsByStoreName(string storeName);
    }

    public class ProductsRepository : IProductsRepository
    {
        private ProductsDbContext _context;

        public ProductsRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product>
            GetProductsByStoreName(string storeName)
        {
            var products = _context.Products?
            .Where(product =>
                product.Store.Name.Equals(storeName,
                StringComparison.InvariantCultureIgnoreCase))
            .Include(product => product.Ingredients);

            return products;
        }
    }
}
