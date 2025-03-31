using System;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Services;

public interface IIngredientService
{
    Task<IEnumerable<IngredientDto>> GetAllIngredients();
    Task<IngredientDto> GetIngredientById(int IngredientId);
    Task<IngredientDto> CreateIngredient(IngredientDto ingredientDto);
    Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto);
    Task<bool> DeleteIngredient(int IngredientId);

    Task<IngredientDto> GetIngredientWithPlats(int ingredientId);

    Task<IEnumerable<PlatDto>> GetPlatsByIngredientId(int ingredientId);

}
