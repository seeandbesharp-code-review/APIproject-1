using AutoMapper;
using DTOs;
using Entities;

namespace WebAPIShop
{
    public class AutoMapping: Profile
    {
        public AutoMapping() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            //CreateMap<OrderDTO, Order>();//.ForMember(d;
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
