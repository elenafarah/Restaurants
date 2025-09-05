using AutoMapper;
using Restaurants.Application.Restaurants.Commands;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos;
public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {

      //  CreateMap<PatchRestaurantDto, PatchRestaurantCommand>();
      CreateMap<PatchRestaurantCommand, Restaurant>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));


        //   CreateMap<UpdateRestaurantDto, UpdateRestaurantCommand>();

        CreateMap<UpdateRestaurantCommand, Restaurant>()
            .ForMember(dest => dest.Address, opt => opt.PreCondition(src =>
                src.City != null || src.Street != null || src.PostalCode != null
            ))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                City = src.City,
                Street = src.Street,
                PostalCode = src.PostalCode
            }))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreateRestaurantDto, CreateRestaurantCommand>();

        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                src => new Address()
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode
                }
            ));

        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(
                src => new Address()
            {
                City = src.City,
                Street = src.Street,
                PostalCode = src.PostalCode
            }
            ));

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(dist => dist.Dishes, opt => opt.MapFrom(src => src.Dishes) );
    }
}

