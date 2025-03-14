using System;
using AutoMapper;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;


namespace ZeiHomeKitchen_backend.MappingConfiguration;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Ingredient, IngredientDto>().ReverseMap();
        CreateMap<Plat, PlatDto>()
            //Permet d'ignorer ImageBase64 dans le mapping inverse
          .ForMember(dest => dest.ImageBase64, opt => opt.Ignore())
          .ReverseMap();
    }
}
