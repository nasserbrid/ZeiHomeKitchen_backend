using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        // Injection de la classe ZeiHomeKitchenContext
        private readonly ZeiHomeKitchenContext _zeiHomeKitchenContext;
        public ReservationRepository(ZeiHomeKitchenContext zeiHomeKitchenContext)
        {
            _zeiHomeKitchenContext = zeiHomeKitchenContext;
        }

        /// <summary>
        /// Ajoute un plat à une réservation existante.
        /// </summary>
        /// <param name="platReservationDto">L'objet contenant les identifiants du plat et de la réservation.</param>
        /// <returns>Retourne un booléen indiquant si l'ajout a réussi.</returns>
        public async Task<bool> AddPlatToReservation(PlatReservationDto platReservationDto)
        {
            var reservation = await _zeiHomeKitchenContext.Reservations
                .Include(r => r.Plats)
                .FirstOrDefaultAsync(r => r.IdReservation == platReservationDto.IdReservation);

            var plat = await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == platReservationDto.IdPlat);

            if (reservation == null || plat == null) 
            {
                return false;
            }

            reservation.Plats.Add(plat);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Supprime un plat d'une réservation existante.
        /// </summary>
        /// <param name="platReservationDto">L'objet contenant les identifiants du plat et de la réservation.</param>
        /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
        public async Task<bool> RemovePlatFromReservation(PlatReservationDto platReservationDto)
        {
            var reservation = await _zeiHomeKitchenContext.Reservations
                .Include(r => r.Plats)
                .FirstOrDefaultAsync(r => r.IdReservation == platReservationDto.IdReservation);

            var plat = await _zeiHomeKitchenContext.Plats
                .FirstOrDefaultAsync(p => p.IdPlat == platReservationDto.IdPlat);

            //Ici si l'un deux n'existe, on ne fait rien.
            if (reservation == null || plat == null)
            {
                return false; 
            }

            reservation.Plats.Remove(plat);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Crée une nouvelle réservation dans la base de données.
        /// </summary>
        /// <param name="reservation">Objet Reservation à ajouter.</param>
        /// <returns>La réservation créée.</returns>
        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            var result = await _zeiHomeKitchenContext.Reservations.AddAsync(reservation);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Supprime une réservation par son identifiant.
        /// </summary>
        /// <param name="ReservationId">Identifiant de la réservation à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeleteReservationById(int reservationId)
        {
            var reservation = await _zeiHomeKitchenContext.Reservations
                .FirstOrDefaultAsync(r => r.IdReservation == reservationId);

            if (reservation == null)
            {
                return false;
            }

            _zeiHomeKitchenContext.Reservations.Remove(reservation);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Récupère toutes les réservations enregistrées.
        /// </summary>
        /// <returns>Une liste de toutes les réservations.</returns>
        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _zeiHomeKitchenContext.Reservations.ToListAsync();
        }

        /// <summary>
        /// Récupère une réservation par son identifiant.
        /// </summary>
        /// <param name="ReservationId">Identifiant de la réservation.</param>
        /// <returns>La réservation correspondante ou null si inexistante.</returns>
        public async Task<Reservation> GetReservationById(int reservationId)
        {
            return await _zeiHomeKitchenContext.Reservations
                .FirstOrDefaultAsync(r => r.IdReservation == reservationId);
        }

        /// <summary>
        /// Récupère une réservation ainsi que les plats associés.
        /// </summary>
        /// <param name="reservationId">Identifiant de la réservation.</param>
        /// <returns>La réservation avec ses plats.</returns>
        public async Task<Reservation> GetReservationForPlats(int reservationId)
        {
            return await _zeiHomeKitchenContext.Reservations
                .Include(r => r.Plats)
                .FirstOrDefaultAsync(r => r.IdReservation == reservationId);
        }

        /// <summary>
        /// Récupère les réservations effectuées à une date donnée.
        /// </summary>
        /// <param name="date">Date pour laquelle récupérer les réservations.</param>
        /// <returns>Une liste des réservations effectuées ce jour-là.</returns>
        public async Task<IEnumerable<Reservation>> GetReservationsByDate(DateTime date)
        {
            return await _zeiHomeKitchenContext.Reservations
                .Where(r => r.DateReservation.HasValue && r.DateReservation.Value.Date == date.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère toutes les réservations faites par un utilisateur donné.
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur.</param>
        /// <returns>Une liste des réservations de l'utilisateur.</returns>
        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(int utilisateurId)
        {
            return await _zeiHomeKitchenContext.Reservations
                .Where(r => r.IdUtilisateur == utilisateurId)
                .ToListAsync();
        }

        /// <summary>
        /// Met à jour une réservation existante.
        /// </summary>
        /// <param name="reservation">Objet Reservation mis à jour.</param>
        /// <returns>La réservation mise à jour ou null si inexistante.</returns>
        public async Task<Reservation> UpdateReservation(Reservation reservation)
        {
            // Manière plus performante
            //_zeiHomeKitchenContext.Reservations.Update(reservation);
            //await _zeiHomeKitchenContext.SaveChangesAsync();
            //return reservation;

            var result = await _zeiHomeKitchenContext.Reservations
                .FirstOrDefaultAsync(r => r.IdReservation == reservation.IdReservation);

            if (result != null)
            {
                result.Statut = reservation.Statut;
                result.DateReservation = reservation.DateReservation;
                await _zeiHomeKitchenContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        /// <summary>
        /// Met à jour uniquement le statut d'une réservation.
        /// </summary>
        /// <param name="reservationId">L'identifiant de la réservation.</param>
        /// <param name="nouveauStatut">Le nouveau statut de la réservation.</param>
        /// <returns>Retourne la réservation mise à jour ou null si elle n'existe pas.</returns>
        public async Task<Reservation> UpdateReservationStatus(int reservationId, ReservationStatusDto nouveauStatut)
        {
            var reservation = await _zeiHomeKitchenContext.Reservations
                .FirstOrDefaultAsync(r => r.IdReservation == reservationId);

            if (reservation == null)
            {
                return null;
            }
            //Je convertis mon énumération en string
            reservation.Statut = nouveauStatut.ToString();
            await _zeiHomeKitchenContext.SaveChangesAsync();

            return reservation;
        }

    }
}
