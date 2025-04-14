using System;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Domain.Models;
using ZeiHomeKitchen_backend.Infrastructure.Data;

namespace ZeiHomeKitchen_backend.Infrastructure.Repositories
{
    public class PlatRepository : IPlatRepository
    {
        private readonly ZeiHomeKitchenContext _zeiHomeKitchenContext;

        public PlatRepository(ZeiHomeKitchenContext zeiHomeKitchenContext)
        {
            _zeiHomeKitchenContext = zeiHomeKitchenContext;
        }

        /// <summary>
        /// Ajoute un nouveau plat à la base de données.
        /// </summary>
        /// <param name="plat">Le plat à ajouter.</param>
        /// <returns>Le plat ajouté avec son ID généré.</returns>
        public async Task<Plat> CreateNewPlat(Plat plat)
        {
            var result = await _zeiHomeKitchenContext.Plats.AddAsync(plat);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Supprime un plat de la base de données par son ID.
        /// </summary>
        /// <param name="platId">L'ID du plat à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeletePlatById(int platId)
        {
            var plat = await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == platId);

            if (plat == null)
                return false;

            _zeiHomeKitchenContext.Plats.Remove(plat);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Récupère un plat par son ID.
        /// </summary>
        /// <param name="platId">L'ID du plat à récupérer.</param>
        /// <returns>Le plat correspondant à l'ID ou null s'il n'existe pas.</returns>
        public async Task<Plat> GetPlatById(int platId)
        {
            return await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == platId);
        }

        /// <summary>
        /// Récupère tous les plats enregistrés dans la base de données.
        /// </summary>
        /// <returns>Une liste de plats.</returns>
        public async Task<IEnumerable<Plat>> GetAllPlats()
        {
            return await _zeiHomeKitchenContext.Plats.ToListAsync();
        }

        /// <summary>
        /// Met à jour un plat existant.
        /// </summary>
        /// <param name="plat">Le plat avec les nouvelles données.</param>
        /// <returns>Le plat mis à jour ou null s'il n'existe pas.</returns>
        public async Task<Plat> UpdateExistingPlat(Plat plat)
        {
            var existingPlat = await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == plat.IdPlat);

            if (existingPlat != null)
            {
                existingPlat.Nom = plat.Nom;
                existingPlat.Description = plat.Description;
                existingPlat.Image = plat.Image;
                existingPlat.Prix = plat.Prix;
                await _zeiHomeKitchenContext.SaveChangesAsync();

                return existingPlat;
            }
            return null;
        }

        /// <summary>
        /// Récupère un plat avec ses ingrédients associés.
        /// </summary>
        /// <param name="platId">L'ID du plat.</param>
        /// <returns>Le plat avec sa liste d'ingrédients.</returns>
        public async Task<Plat> GetPlatDetailsWithIngredients(int platId)
        {
            return await _zeiHomeKitchenContext.Plats
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.IdPlat == platId);
        }

        /// <summary>
        /// Ajoute un ingrédient à un plat.
        /// </summary>
        /// <param name="platId">L'ID du plat.</param>
        /// <param name="ingredientId">L'ID de l'ingrédient à ajouter.</param>
        /// <returns>True si l'ajout a réussi, sinon False.</returns>
        public async Task<bool> LinkIngredientToPlat(int platId, int ingredientId)
        {
            var plat = await _zeiHomeKitchenContext.Plats
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.IdPlat == platId);

            if (plat == null)
                return false;

            var ingredient = await _zeiHomeKitchenContext.Ingredients
                .FirstOrDefaultAsync(i => i.IdIngredient == ingredientId);

            if (ingredient == null)
                return false;

            if (!plat.Ingredients.Any(i => i.IdIngredient == ingredientId))
            {
                plat.Ingredients.Add(ingredient);
                await _zeiHomeKitchenContext.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// Supprime un ingrédient d'un plat.
        /// </summary>
        /// <param name="platId">L'ID du plat.</param>
        /// <param name="ingredientId">L'ID de l'ingrédient à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> RemoveIngredientFromPlat(int platId, int ingredientId)
        {
            var plat = await _zeiHomeKitchenContext.Plats
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.IdPlat == platId);

            if (plat == null)
                return false;

            var ingredient = plat.Ingredients
                .FirstOrDefault(i => i.IdIngredient == ingredientId);

            if (ingredient == null)
                return false;

            plat.Ingredients.Remove(ingredient);
            await _zeiHomeKitchenContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Récupère la liste des ingrédients d'un plat.
        /// </summary>
        /// <param name="platId">L'ID du plat.</param>
        /// <returns>Une liste d'ingrédients associés au plat.</returns>
        public async Task<IEnumerable<Ingredient>> GetIngredientsByPlat(int platId)
        {
            var plat = await _zeiHomeKitchenContext.Plats
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.IdPlat == platId);

            return plat?.Ingredients ?? new List<Ingredient>();
        }
    }
}
