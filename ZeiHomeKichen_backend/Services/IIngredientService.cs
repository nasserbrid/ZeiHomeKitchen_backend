using System;
using ZeiHomeKichen_backend.Dtos;

namespace ZeiHomeKichen_backend.Services;

public interface IIngredientService
{
    Task<IEnumerable<IngredientDto>> GetIngredients();
    Task<IngredientDto> GetIngredient(int IngredientId);
    Task<IngredientDto> CreateIngredient(IngredientDto ingredientDto);
    Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto);
    Task<bool> DeleteIngredient(int IngredientId);
}
