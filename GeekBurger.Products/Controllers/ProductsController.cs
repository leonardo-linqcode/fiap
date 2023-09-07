using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Products.Contract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeekBurger.Products.Controllers
{
    [Route("api/products")]


    public class ProductsController : Controller
    {
        private IList<Product> Products = new List<Product>();

        public ProductsController()        {
            var paulistaStore = "Paulista";
            var morumbiStore = "Morumbi";

            var beef = new Ingredient { ItemId = Guid.NewGuid(), Name = "beef" };
            var pork = new Ingredient { ItemId = Guid.NewGuid(), Name = "pork" };
            var mustard = new Ingredient { ItemId = Guid.NewGuid(), Name = "mustard" };
            var ketchup = new Ingredient { ItemId = Guid.NewGuid(), Name = "ketchup" };
            var bread = new Ingredient { ItemId = Guid.NewGuid(), Name = "bread" };
            var wBread = new Ingredient { ItemId = Guid.NewGuid(), Name = "whole bread" };

            Products = new List<Product>()
    {
        new Product { ProductId = Guid.NewGuid(), Name = "Darth Bacon",
            Image = "hamb1.png", StoreName = paulistaStore,
            Ingredients = new List<Ingredient> {beef, ketchup, bread }
        },
        new Product { ProductId = Guid.NewGuid(), Name = "Cap. Spork",
            Image = "hamb2.png", StoreName = paulistaStore,
            Ingredients = new List<Ingredient> { pork, mustard, wBread }
        },
        new Product { ProductId = Guid.NewGuid(), Name = "Beef Turner",
            Image = "hamb3.png", StoreName = morumbiStore,
            Ingredients = new List<Ingredient> {beef, mustard, bread }
        }
    };
        }
        [HttpGet("{storeName}")]
        public IActionResult GetProductsByStoreName(string storeName)
        {
            var productsByStore = Products.Where(product =>
                product.StoreName == storeName).ToList();

            if (productsByStore.Count <= 0)
                return NotFound();

            return Ok(productsByStore);
        }
      
    }

}

