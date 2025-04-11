using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

public static class CreateReservationMapper
{
    public static Reservation ToModel(this CreateReservationDto dto, int userId)
    {
        return new Reservation
        {
            DateReservation = dto.DateReservation,
            Adresse = dto.Adresse,
            NombrePersonnes = dto.NombrePersonnes,
            Statut = ReservationStatusDto.EnAttente.ToString(), 
            IdUtilisateur = userId, 
            Plats = new List<Plat>() 
        };
    }
}
