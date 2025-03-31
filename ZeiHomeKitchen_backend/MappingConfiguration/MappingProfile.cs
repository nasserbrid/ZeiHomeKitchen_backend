using System;
using AutoMapper;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;


namespace ZeiHomeKitchen_backend.MappingConfiguration;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Ingredient, IngredientDto>()
            .ForMember(dest => dest.PlatIds, opt => opt.MapFrom(src => src.Plats.Select(p => p.IdPlat)))
            .ReverseMap()
            .ForMember(dest => dest.Plats, opt => opt.Ignore());
        CreateMap<Plat, PlatDto>()
            //Permet d'ignorer ImageBase64 dans le mapping inverse
          .ForMember(dest => dest.ImageBase64, opt => opt.Ignore())
          .ForMember(dest => dest.IngredientIds, opt => opt.MapFrom(src => src.Ingredients.Select(i => i.IdIngredient)))
          .ReverseMap()
          .ForMember(dest => dest.Ingredients, opt => opt.Ignore());
    }
}
