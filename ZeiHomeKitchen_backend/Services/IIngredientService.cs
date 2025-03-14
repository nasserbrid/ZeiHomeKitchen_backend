using System;
using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Services;

public interface IIngredientService
{
    Task<IEnumerable<IngredientDto>> GetIngredients();
    Task<IngredientDto> GetIngredient(int IngredientId);
    Task<IngredientDto> CreateIngredient(IngredientDto ingredientDto);
    Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto);
    Task<bool> DeleteIngredient(int IngredientId);
}
