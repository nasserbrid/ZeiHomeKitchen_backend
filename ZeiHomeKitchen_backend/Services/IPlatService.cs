using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Services
{
    public interface IPlatService
    {
        Task<IEnumerable<PlatDto>> GetPlats();
        Task<PlatDto> GetPlat(int PlatId);
        Task<PlatDto> CreatePlat(PlatDto platDto);
        Task<PlatDto> UpdatePlat(PlatDto platDto);
        Task<bool> DeletePlat(int PlatId);
    }
}
