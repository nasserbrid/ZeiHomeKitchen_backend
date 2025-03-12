using System;
using AutoMapper;
using ZeiHomeKichen_backend.Dtos;
using ZeiHomeKichen_backend.Models;


namespace ZeiHomeKichen_backend.MappingConfiguration;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Ingredient, IngredientDto>().ReverseMap();

    }
}
