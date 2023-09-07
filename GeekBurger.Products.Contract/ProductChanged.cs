using System;
namespace GeekBurger.Products.Contract
{
    public class ProductChanged
    {
        public ProductState State { get; set; }
        public Product Product { get; set; }
    }

}

