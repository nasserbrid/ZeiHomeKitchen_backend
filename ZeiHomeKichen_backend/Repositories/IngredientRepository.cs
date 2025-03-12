using System;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKichen_backend.Models;

namespace ZeiHomeKichen_backend.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly ZeiHomeKitchenContext zeiHomeKitchenContext;

    public IngredientRepository(ZeiHomeKitchenContext zeiHomeKitchenContext)
    {
        this.zeiHomeKitchenContext = zeiHomeKitchenContext;
    }
    public async Task<Ingredient> CreateIngredient(Ingredient ingredient)
    {
        var result = await zeiHomeKitchenContext.Ingredients.AddAsync(ingredient);
        await zeiHomeKitchenContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeleteIngredient(int IngredientId)
    {
        var result = await zeiHomeKitchenContext.Ingredients
            .FirstOrDefaultAsync(i => i.IdIngredient == IngredientId );

        if(result == null)
        {
            return false;
        }

        zeiHomeKitchenContext.Ingredients.Remove(result);
        await zeiHomeKitchenContext.SaveChangesAsync();

        return true;
    }

    public async Task<Ingredient> GetIngredient(int IngredientId)
    {
        return await zeiHomeKitchenContext.Ingredients
                .FirstOrDefaultAsync(i => i.IdIngredient == IngredientId );
    }

    public async Task<IEnumerable<Ingredient>> GetIngredients()
    {
         return await zeiHomeKitchenContext.Ingredients.ToListAsync();
    }

    public async Task<Ingredient> UpdateIngredient(Ingredient ingredient)
    {
        var result = await zeiHomeKitchenContext.Ingredients
            .FirstOrDefaultAsync(i => i.IdIngredient == ingredient.IdIngredient);

        if(result != null)
        {
            result.Nom = ingredient.Nom;

            await zeiHomeKitchenContext.SaveChangesAsync();

            return result;
        }

        return null;
    }
}
