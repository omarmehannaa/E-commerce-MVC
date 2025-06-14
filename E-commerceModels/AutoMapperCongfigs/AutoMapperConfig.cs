using AutoMapper;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using E_commerce.Models;

namespace E_commerce.Models.AutoMapperCongfigs
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Category, CategoryView>().ReverseMap();
            CreateMap<Product, ProductView>().ReverseMap();
            CreateMap<Company, CompanyView>().ReverseMap();
            CreateMap<CartItem, CartItemView>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderView>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsView>().ReverseMap();
        }

    }
}
