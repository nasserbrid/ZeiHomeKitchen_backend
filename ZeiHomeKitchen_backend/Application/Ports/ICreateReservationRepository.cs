using ZeiHomeKitchen_backend.Domain.Models;

public interface ICreateReservationRepository
{
    Task<Reservation> CreateReservation(Reservation reservation);
}
