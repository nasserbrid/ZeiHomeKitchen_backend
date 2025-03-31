using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Services
{
    public interface IPlatService
    {
        Task<IEnumerable<PlatDto>> GetAllPlats();
        Task<PlatDto> GetPlatById(int platId);
        Task<PlatDto> CreateNewPlat(PlatDto platDto);
        Task<PlatDto> UpdateExistingPlat(PlatDto platDto);
        Task<bool> DeletePlatById(int platId);
        Task<PlatDto> GetPlatDetailsWithIngredients(int platId);
        Task<bool> LinkIngredientToPlat(int platId, int ingredientId);
        Task<bool> RemoveIngredientFromPlat(int platId, int ingredientId);
        Task<IEnumerable<IngredientDto>> GetIngredientsByPlat(int platId);
    }
}
