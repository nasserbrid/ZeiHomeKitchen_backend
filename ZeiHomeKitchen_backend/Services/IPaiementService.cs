using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Services
{
    public interface IPaiementService
    {
        Task<PaiementDto> CreatePaiement(PaiementDto paiementDto);
        Task<IEnumerable<PaiementDto>> GetAllPaiements();
        Task<PaiementDto> GetPaiementById(int idPaiement);
        Task<PaiementDto> GetPaiementByReservationId(int reservationId);
        Task<PaiementDto> UpdatePaiement(PaiementDto paiementDto);
        Task<bool> DeletePaiement(int idPaiement);
    }
}
