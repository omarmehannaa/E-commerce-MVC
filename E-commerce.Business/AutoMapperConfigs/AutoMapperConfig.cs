using AutoMapper;
using E_commerce.Models;
using E_commerce.Models.ViewModels;

namespace E_commerce.Business.AutoMapperConfigs
{
    internal class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryView>().ReverseMap();
            CreateMap<Product, ProductView>().ReverseMap();

        }
    }
}
