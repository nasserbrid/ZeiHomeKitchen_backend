using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration
{
    public static class ReservationMapper
    {
        public static ReservationDto ToDto(this Reservation reservation)
        {
            return new ReservationDto(
                reservation.IdReservation,
                reservation.DateReservation,
                reservation.Adresse,
                Enum.Parse<ReservationStatusDto>(reservation.Statut),
                reservation.Nom,                // Ajout du Nom
                reservation.Prenom,             // Ajout du Prénom
                reservation.NombrePersonnes     // Nombre de personnes
                                                // Garder IdStatistique pour le suivi
            )
            {
                PlatIds = reservation.Plats?.Select(p => p.IdPlat).ToList() ?? new List<int>(),
                IdUtilisateur = reservation.IdUtilisateur

            };
        }

        public static Reservation ToModel(this ReservationDto dto)
        {
            return new Reservation
            {
                IdReservation = dto.IdReservation,
                DateReservation = dto.DateReservation,
                Adresse = dto.Adresse,
                Statut = dto.Statut.ToString(),
                NombrePersonnes = dto.NombrePersonnes,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                //IdStatistique = dto.IdStatistique,
                IdUtilisateur = dto.IdUtilisateur,
                Plats = new List<Plat>()
            };
        }
    }
}
