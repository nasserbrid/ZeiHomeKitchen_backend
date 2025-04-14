using System;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports
{
    public interface IPlatRepository
    {
        Task<IEnumerable<Plat>> GetAllPlats();
        Task<Plat> GetPlatById(int platId);
        Task<Plat> CreateNewPlat(Plat plat);
        Task<Plat> UpdateExistingPlat(Plat plat);
        Task<bool> DeletePlatById(int platId);
        Task<Plat> GetPlatDetailsWithIngredients(int platId);
        Task<bool> LinkIngredientToPlat(int platId, int ingredientId);
        Task<bool> RemoveIngredientFromPlat(int platId, int ingredientId);
        Task<IEnumerable<Ingredient>> GetIngredientsByPlat(int platId);
    }
}
