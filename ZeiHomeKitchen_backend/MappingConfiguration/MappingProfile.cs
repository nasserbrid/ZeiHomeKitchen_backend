using System;
using AutoMapper;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;


namespace ZeiHomeKitchen_backend.MappingConfiguration;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        //Ici je défini mon mapping en l'entité Ingredient et son DTO IngredientDto
        CreateMap<Ingredient, IngredientDto>()
            //Plats est une collection d'objets Plat associée à Ingrédient.
            //Je veux que dans IngredientDto, PlatIds (une liste de int) contienne seulement les ID des plus associés.
            .ForMember(dest => dest.PlatIds, opt => opt.MapFrom(src => src.Plats.Select(p => p.IdPlat)))
            //Permet le mapping inverse cad IngredientDto vers Ingredient
            .ReverseMap()
            //Comme il n'y a que IngredientDto qui ne contient que les PlatIds, on ne veut pas assigner un objet Plat à un Ingredient et donc on l'ignore.
            .ForMember(dest => dest.Plats, opt => opt.Ignore());
        CreateMap<Plat, PlatDto>()
          //Permet d'ignorer ImageBase64 dans le mapping inverse
          .ForMember(dest => dest.ImageBase64, opt => opt.Ignore())
          .ForMember(dest => dest.IngredientIds, opt => opt.MapFrom(src => src.Ingredients.Select(i => i.IdIngredient)))
          .ReverseMap()
          .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

        CreateMap<Reservation, ReservationDto>()
          .ForMember(dest => dest.PlatIds, opt => opt.MapFrom(src => src.Plats.Select(p => p.IdPlat)))
          .ReverseMap()
          .ForMember(dest => dest.Plats, opt => opt.Ignore());
    }
}
