using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration
{
    public static class IngredientMapper
    {
        public static IngredientDto ToDto(this Ingredient ingredient)
        {
            return new IngredientDto(
                ingredient.IdIngredient,
                ingredient.Nom,
                ingredient.Plats?.Select(p => p.IdPlat).ToList()
            );
        }

        public static Ingredient ToModel(this IngredientDto dto)
        {
            return new Ingredient
            {
                IdIngredient = dto.IdIngredient,
                Nom = dto.Nom,
                //Ici j'ignore Plats pour éviter de charger inutilement
                Plats = new List<Plat>()
            };
        }
    }

}
