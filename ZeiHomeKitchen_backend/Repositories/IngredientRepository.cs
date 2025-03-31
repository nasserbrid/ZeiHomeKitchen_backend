using System;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public class IngredientRepository : IIngredientRepository
{
    //Injection des d�pendances
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

    /// <summary>
    /// M�thode qui permet de supprimer un ingr�dient par son ID
    /// </summary>
    /// <param name="IngredientId"></param>
    /// <returns></returns>
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

    /// <summary>
    /// M�thode qui recup�re un ingr�dient par son ID
    /// </summary>
    /// <param name="IngredientId"></param>
    /// <returns></returns>
    public async Task<Ingredient> GetIngredientById(int IngredientId)
    {
        return await _zeiHomeKitchenContext.Ingredients
                .FirstOrDefaultAsync(i => i.IdIngredient == IngredientId );
    }

    /// <summary>
    /// M�thode qui permet de r�cuperer les plats associ�s � un ingr�dient
    /// </summary>
    /// <param name="ingredientId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Plat>> GetPlatsByIngredientId(int ingredientId)
    {
        return await _zeiHomeKitchenContext.Plats
            .Where(p => p.Ingredients.Any(i => i.IdIngredient == ingredientId))
            .ToListAsync();
    }

    /// <summary>
    /// M�thode qui r�cup�re tous les ingr�dients.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Ingredient>> GetAllIngredients()
    {
         return await _zeiHomeKitchenContext.Ingredients.ToListAsync();
    }

    /// <summary>
    /// M�thode qui recup�re un ingr�dient avec ses plats associ�s.
    /// </summary>
    /// <param name="ingredientId"></param>
    /// <returns></returns>
    public async Task<Ingredient> GetIngredientWithPlats(int ingredientId)
    {
        return await _zeiHomeKitchenContext.Ingredients
        .Include(i => i.Plats)
        .FirstOrDefaultAsync(i => i.IdIngredient == ingredientId);
    }

    /// <summary>
    /// M�thode qui permet de mettre � jour un ingr�dient existant
    /// </summary>
    /// <param name="ingredient"></param>
    /// <returns></returns>
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
