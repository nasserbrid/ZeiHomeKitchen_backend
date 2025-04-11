using ZeiHomeKitchen_backend.Dtos;
using System.Threading.Tasks;

public interface ICreateReservationService
{
    Task<ReservationDto> CreateReservation(CreateReservationDto reservationDto);
}
