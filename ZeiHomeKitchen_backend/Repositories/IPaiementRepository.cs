using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public interface IPaiementRepository
    {
        Task<Paiement> CreatePaiement(Paiement paiement);
        Task<IEnumerable<Paiement>> GetAllPaiements();
        Task<Paiement> GetPaiementById(int idPaiement);
        Task<Paiement> GetPaiementByReservationId(int reservationId);
        Task<Paiement> UpdatePaiement(Paiement paiement);
        Task<bool> DeletePaiement(int idPaiement);
    }
}
