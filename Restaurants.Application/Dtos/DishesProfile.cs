using AutoMapper;
using Restaurants.Application.Dishes.Commands;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos;
public class DishesProfile: Profile
{
    public DishesProfile()
    {
        CreateMap<CreateDishCommand, Dish>();

        CreateMap<Dish, DishDto>();

        CreateMap<PatchDishCommand, Dish>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.RestaurantId, opt => opt.Ignore())
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember, ctx) => srcMember != null));

        
        CreateMap<UpdateDishCommand, Dish>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.RestaurantId, opt => opt.Ignore());
    }
}

