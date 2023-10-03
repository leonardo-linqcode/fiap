using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.Products.Model;

namespace GeekBurger.Products.AutoMapperConfig
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Product, ProductToGet>();
            CreateMap<Item, ItemToGet>();
        }
    }
}
