using GeekBurger.Products.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Products.Contract.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetProductsByStoreName(string storeName);
    }

}
