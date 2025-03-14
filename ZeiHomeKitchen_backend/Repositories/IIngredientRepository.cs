using System;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public interface IIngredientRepository
{
    Task<IEnumerable<Ingredient>> GetIngredients();
    Task<Ingredient> GetIngredient(int IngredientId);
    Task<Ingredient> CreateIngredient(Ingredient ingredient);
    Task<Ingredient> UpdateIngredient(Ingredient ingredient);
    Task<bool> DeleteIngredient(int IngredientId);
}
