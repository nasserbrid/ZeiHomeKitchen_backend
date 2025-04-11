using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public class PaiementRepository : IPaiementRepository
    {
        private readonly ZeiHomeKitchenContext _zeiHomeKitchenContext;

        public PaiementRepository(ZeiHomeKitchenContext context)
        {
            _zeiHomeKitchenContext = context;
        }

        public async Task<Paiement> CreatePaiement(Paiement paiement)
        {
            _zeiHomeKitchenContext.Paiements.Add(paiement);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return paiement;
        }

        public async Task<IEnumerable<Paiement>> GetAllPaiements()
        {
            return await _zeiHomeKitchenContext.Paiements.ToListAsync();
        }

        public async Task<Paiement> GetPaiementById(int idPaiement)
        {
            return await _zeiHomeKitchenContext.Paiements.FindAsync(idPaiement);
        }

        public async Task<Paiement> GetPaiementByReservationId(int reservationId)
        {
            return await _zeiHomeKitchenContext.Paiements.FirstOrDefaultAsync(p => p.IdReservation == reservationId);
        }

        public async Task<Paiement> UpdatePaiement(Paiement paiement)
        {
            _zeiHomeKitchenContext.Paiements.Update(paiement);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return paiement;
        }

        public async Task<bool> DeletePaiement(int idPaiement)
        {
            var paiement = await _zeiHomeKitchenContext.Paiements.FindAsync(idPaiement);
            if (paiement == null) return false;

            _zeiHomeKitchenContext.Paiements.Remove(paiement);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }
    }
}
