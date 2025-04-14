using System.Threading.Tasks;
using ZeiHomeKitchen_backend.Domain.Dtos;

public interface ICreateReservationService
{
    Task<ReservationDto> CreateReservation(CreateReservationDto reservationDto);
}
