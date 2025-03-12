using System;
using ZeiHomeKichen_backend.Models;

namespace ZeiHomeKichen_backend.Repositories;

public interface IIngredientRepository
{
    Task<IEnumerable<Ingredient>> GetIngredients();
    Task<Ingredient> GetIngredient(int IngredientId);
    Task<Ingredient> CreateIngredient(Ingredient ingredient);
    Task<Ingredient> UpdateIngredient(Ingredient ingredient);
    Task<bool> DeleteIngredient(int IngredientId);
}
