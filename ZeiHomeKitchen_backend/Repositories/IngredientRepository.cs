using System;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public class IngredientRepository : IIngredientRepository
{
    //Injection des dépendances
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
    /// Méthode qui permet de supprimer un ingrédient par son ID
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
    /// Méthode qui recupère un ingrédient par son ID
    /// </summary>
    /// <param name="IngredientId"></param>
    /// <returns></returns>
    public async Task<Ingredient> GetIngredientById(int IngredientId)
    {
        return await _zeiHomeKitchenContext.Ingredients
                .FirstOrDefaultAsync(i => i.IdIngredient == IngredientId );
    }

    /// <summary>
    /// Méthode qui permet de récuperer les plats associés à un ingrédient
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
    /// Méthode qui récupère tous les ingrédients.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Ingredient>> GetAllIngredients()
    {
         return await _zeiHomeKitchenContext.Ingredients.ToListAsync();
    }

    /// <summary>
    /// Méthode qui recupère un ingrédient avec ses plats associés.
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
    /// Méthode qui permet de mettre à jour un ingrédient existant
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
