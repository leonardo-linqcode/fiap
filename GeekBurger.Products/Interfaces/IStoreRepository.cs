﻿using GeekBurger.Products.Contract.Model;
using GeekBurger.Products.Model;

namespace GeekBurger.Products.Repository
{
    public interface IStoreRepository
    {
        Store GetStoreByName(string storeName);
    }
}