using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllReservations();
        Task<Reservation> GetReservationById(int reservationId);
        Task<Reservation> CreateReservation(Reservation reservation);
        Task<Reservation> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservationById(int reservationId);
        Task<Reservation> GetReservationForPlats(int reservationId);
        Task<IEnumerable<Reservation>> GetReservationsByUserId(int utilisateurId);
        Task<IEnumerable<Reservation>> GetReservationsByDate(DateTime date);
        Task<Reservation> UpdateReservationStatus(int reservationId, ReservationStatusDto nouveauStatut);
        Task<bool> AddPlatToReservation(PlatReservationDto platReservationDto);
        Task<bool> RemovePlatFromReservation(PlatReservationDto platReservationDto);
        Task<Paiement> CreatePaiementForReservation(int reservationId, decimal montant, PaiementMoyenDto moyen);


    }
}
