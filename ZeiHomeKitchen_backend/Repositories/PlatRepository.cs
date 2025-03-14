using System;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public class PlatRepository : IPlatRepository
    {
        private readonly ZeiHomeKitchenContext _zeiHomeKitchenContext;

        public PlatRepository(ZeiHomeKitchenContext zeiHomeKitchenContext)
        {
            _zeiHomeKitchenContext = zeiHomeKitchenContext;
        }
        public async Task<Plat> CreatePlat(Plat plat)
        {
            var result = await _zeiHomeKitchenContext.Plats.AddAsync(plat);

            await _zeiHomeKitchenContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeletePlat(int PlatId)
        {
            var result = await _zeiHomeKitchenContext.Plats
                  .FirstOrDefaultAsync(p => p.IdPlat == PlatId);

            if (result == null) 
            { 
                return false;
            }

            _zeiHomeKitchenContext.Plats.Remove(result);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }

        public async Task<Plat> GetPlat(int PlatId)
        {
            return await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == PlatId);
        }

        public async Task<IEnumerable<Plat>> GetPlats()
        {
            return await _zeiHomeKitchenContext.Plats.ToListAsync();
        }

        public async Task<Plat> UpdatePlat(Plat plat)
        {
            var result = await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == plat.IdPlat);

            if (result != null)
            {
                result.Nom = plat.Nom;
                result.Description = plat.Description;
                result.Image = plat.Image;
                result.Prix = plat.Prix;
                await _zeiHomeKitchenContext.SaveChangesAsync();

                return result;
            }
            return null;
        }
    }
}
