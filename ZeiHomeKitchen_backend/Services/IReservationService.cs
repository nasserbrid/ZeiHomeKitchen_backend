using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllReservations();
        Task<ReservationDto> GetReservationById(int reservationId);
        Task<ReservationDto> CreateReservation(ReservationDto reservationDto);
        Task<ReservationDto> UpdateReservation(ReservationDto reservationDto);
        Task<bool> DeleteReservationById(int reservationId);
        Task<ReservationDto> GetReservationForPlats(int reservationId);
        Task<IEnumerable<ReservationDto>> GetReservationsByUserId(int utilisateurId);
        Task<IEnumerable<ReservationDto>> GetReservationsByDate(DateTime date);
        Task<ReservationDto> UpdateReservationStatus(int reservationId, ReservationStatusDto nouveauStatut);
        Task<bool> AddPlatToReservation(PlatReservationDto platReservationDto);
        Task<bool> RemovePlatFromReservation(PlatReservationDto platReservationDto);
    }
}
