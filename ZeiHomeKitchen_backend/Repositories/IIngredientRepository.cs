using System;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public interface IIngredientRepository
{
    Task<IEnumerable<Ingredient>> GetAllIngredients();
    Task<Ingredient> GetIngredientById(int IngredientId);
    Task<Ingredient> CreateIngredient(Ingredient ingredient);
    Task<Ingredient> UpdateIngredient(Ingredient ingredient);
    Task<bool> DeleteIngredient(int IngredientId);
    Task<Ingredient> GetIngredientWithPlats(int ingredientId);
    Task<IEnumerable<Plat>> GetPlatsByIngredientId(int ingredientId);
}
