using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

public class CreateReservationRepository : ICreateReservationRepository
{
    private readonly ZeiHomeKitchenContext _context;

    public CreateReservationRepository(ZeiHomeKitchenContext context)
    {
        _context = context;
    }

    public async Task<Reservation> CreateReservation(Reservation reservation)
    {
        var result = await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        return result.Entity;
    }
}
