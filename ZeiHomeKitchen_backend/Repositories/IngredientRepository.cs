using System;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly ZeiHomeKitchenContext _zeiHomeKitchenContext;

    public IngredientRepository(ZeiHomeKitchenContext zeiHomeKitchenContext)
    {
        _zeiHomeKitchenContext = zeiHomeKitchenContext;
    }
    public async Task<Ingredient> CreateIngredient(Ingredient ingredient)
    {
        var result = await _zeiHomeKitchenContext.Ingredients.AddAsync(ingredient);
        await _zeiHomeKitchenContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeleteIngredient(int IngredientId)
    {
        var result = await _zeiHomeKitchenContext.Ingredients
            .FirstOrDefaultAsync(i => i.IdIngredient == IngredientId );

        if(result == null)
        {
            return false;
        }

        _zeiHomeKitchenContext.Ingredients.Remove(result);
        await _zeiHomeKitchenContext.SaveChangesAsync();

        return true;
    }

    public async Task<Ingredient> GetIngredient(int IngredientId)
    {
        return await _zeiHomeKitchenContext.Ingredients
                .FirstOrDefaultAsync(i => i.IdIngredient == IngredientId );
    }

    public async Task<IEnumerable<Ingredient>> GetIngredients()
    {
         return await _zeiHomeKitchenContext.Ingredients.ToListAsync();
    }

    public async Task<Ingredient> UpdateIngredient(Ingredient ingredient)
    {
        var result = await _zeiHomeKitchenContext.Ingredients
            .FirstOrDefaultAsync(i => i.IdIngredient == ingredient.IdIngredient);

        if(result != null)
        {
            result.Nom = ingredient.Nom;

            await _zeiHomeKitchenContext.SaveChangesAsync();

            return result;
        }

        return null;
    }
}
