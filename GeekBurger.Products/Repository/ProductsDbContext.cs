﻿using GeekBurger.Products.Contract.Model;
using GeekBurger.Products.Contract;
using Microsoft.EntityFrameworkCore;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }

    public DbSet<Item> Items { get; set; }
}