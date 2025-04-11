using ZeiHomeKitchen_backend.Models;

public interface ICreateReservationRepository
{
    Task<Reservation> CreateReservation(Reservation reservation);
}
