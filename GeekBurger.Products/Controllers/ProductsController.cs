using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.Products.Contract.Interfaces;
using GeekBurger.Products.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeekBurger.Products.Controllers
{
    [Route("api/products")]

    public class ProductsController : Controller
    {
        private IProductsRepository _productsRepository;
        private IMapper _mapper;

        public ProductsController(IProductsRepository productsRepository, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }

        [HttpGet("{storeName}")]
        public IActionResult GetProductsByStoreName([FromQuery] string storeName)
        {
            var productsByStore = _productsRepository.GetProductsByStoreName(storeName).ToList();

            if (productsByStore.Count <= 0)
                return NotFound("Nenhum dado encontrado");

            var productsToGet = _mapper.Map<IEnumerable<ProductToGet>>(productsByStore);

            return Ok(productsToGet);
        }
        private IList<Product> Products = new List<Product>();

        //    public ProductsController()
        //    {
        //        var paulistaStore = "Paulista";
        //        var morumbiStore = "Morumbi";

        //        var beef = new Item { ItemId = Guid.NewGuid(), Name = "beef" };
        //        var pork = new Item { ItemId = Guid.NewGuid(), Name = "pork" };
        //        var mustard = new Item { ItemId = Guid.NewGuid(), Name = "mustard" };
        //        var ketchup = new Item { ItemId = Guid.NewGuid(), Name = "ketchup" };
        //        var bread = new Item { ItemId = Guid.NewGuid(), Name = "bread" };
        //        var wBread = new Item { ItemId = Guid.NewGuid(), Name = "whole bread" };

        //        Products = new List<Product>()
        //{
        //    new Product { ProductId = Guid.NewGuid(), Name = "Darth Bacon",
        //        Image = "hamb1.png", StoreName = paulistaStore,
        //        Items = new List<Item> {beef, ketchup, bread }
        //    },
        //    new Product { ProductId = Guid.NewGuid(), Name = "Cap. Spork",
        //        Image = "hamb2.png", StoreName = paulistaStore,
        //        Items = new List<Item> { pork, mustard, wBread }
        //    },
        //    new Product { ProductId = Guid.NewGuid(), Name = "Beef Turner",
        //        Image = "hamb3.png", StoreName = morumbiStore,
        //        Items = new List<Item> {beef, mustard, bread }
        //    }
        //};
        //    }

    }

}

