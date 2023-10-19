using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeekBurger.Products.Repository;
using GeekBurger.Products.Controllers;
using GeekBurger.Products.Contract.Model;

public class ProductsControllerUnitTests
{
    private readonly ProductsController _productsController;
    private Mock<IProductsRepository> _productRepositoryMock;
    private Mock<IMapper> _mapperMock;

    public ProductsControllerUnitTests()
    {
        _productRepositoryMock = new Mock<IProductsRepository>();
        _mapperMock = new Mock<IMapper>();
        _productsController = new ProductsController(_productRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void OnGetProductsByStoreNameShouldReturnNotFound()
    {
        //arrange
        var storeName = "Paulista";
        var productList = new List<Product>();
        _productRepositoryMock.Setup(_ => _.GetProductsByStoreName(storeName)).Returns(productList);
        var expected = new NotFoundObjectResult("Nenhum dado encontrado");

        //act
        var response = _productsController.GetProductsByStoreName(storeName);

        //assert            
        Assert.IsType<NotFoundObjectResult>(response);
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void OnGetProductsByStoreNameShouldReturnOKSuccess()
    {
        //arrange
        var storeName = "Paulista";
        var productList = new List<Product>() 
        {
            new Product { 
                Name = "Paulista"
            }
        };

        // Configurar AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductToGet>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        });

        var mapper = config.CreateMapper();

        _mapperMock.Setup(m => m.Map<IEnumerable<ProductToGet>>(It.IsAny<IEnumerable<Product>>()))
            .Returns((IEnumerable<Product> source) => mapper.Map<IEnumerable<ProductToGet>>(source));

        _productRepositoryMock.Setup(_ => _.GetProductsByStoreName(storeName)).Returns(productList);
        
        var expected = new OkObjectResult(new List<ProductToGet>()
        {
            new ProductToGet 
            {
                Name = "Paulista",
                Items = new List<ItemToGet>()
            }
        });

        //act
        var response = _productsController.GetProductsByStoreName(storeName);

        //assert            
        Assert.IsType<OkObjectResult>(response);
        response.Should().BeEquivalentTo(expected);
    }
}