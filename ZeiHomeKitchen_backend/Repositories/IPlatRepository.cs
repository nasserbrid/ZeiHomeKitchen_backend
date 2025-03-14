using System;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public interface IPlatRepository
    {
        Task<IEnumerable<Plat>> GetPlats();
        Task<Plat> GetPlat(int PlatId);
        Task<Plat> CreatePlat(Plat plat);
        Task<Plat> UpdatePlat(Plat plat);
        Task<bool> DeletePlat(int PlatId);
    }
}
