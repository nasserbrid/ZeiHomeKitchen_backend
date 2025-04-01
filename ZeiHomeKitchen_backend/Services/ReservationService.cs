using AutoMapper;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZeiHomeKitchen_backend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(IReservationRepository reservationRepository, IMapper mapper, ILogger<ReservationService> logger)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _logger = logger;
            
        }

        /// <summary>
        /// Ajoute un plat à une réservation existante.
        /// </summary>
        /// <param name="platReservationDto">Données du plat et de la réservation.</param>
        /// <returns>True si l'ajout est réussi, sinon False.</returns>
        public async Task<bool> AddPlatToReservation(PlatReservationDto platReservationDto)
        {
            
            if (platReservationDto == null)
            {
                throw new ArgumentNullException(nameof(platReservationDto));
            }

            var reservation = await _reservationRepository.GetReservationById(platReservationDto.IdReservation);

            if (reservation == null) 
            { 
                throw new KeyNotFoundException($"Aucune réservation trouvée avec l'ID {platReservationDto.IdReservation}");
            }

            var plat = await _reservationRepository.GetReservationById(platReservationDto.IdPlat);

            return await _reservationRepository.AddPlatToReservation(platReservationDto);

        }

        /// <summary>
        /// Crée une nouvelle réservation.
        /// </summary>
        /// <param name="reservationDto">Données de la réservation.</param>
        /// <returns>La réservation créée.</returns>
        public async Task<ReservationDto> CreateReservation(ReservationDto reservationDto)
        {
            if (reservationDto == null)
            {
                throw new ArgumentNullException(nameof(reservationDto));
            }

            _logger.LogInformation("Mapping de ReservationDto vers Reservation.");
            var reservationEntity = _mapper.Map<Reservation>(reservationDto);

            _logger.LogInformation("Enregistrement de l'entité en base de données.");
            var createReservation = await _reservationRepository.CreateReservation(reservationEntity);

            _logger.LogInformation("Mapping de l'entité créée vers ReservationDto.");
            return _mapper.Map<ReservationDto>(createReservation);
        }

        /// <summary>
        /// Supprime une réservation par son identifiant.
        /// </summary>
        /// <param name="reservationId">Identifiant de la réservation.</param>
        /// <returns>True si la suppression est réussie, sinon False.</returns>
        public async Task<bool> DeleteReservationById(int reservationId)
        {
            return await _reservationRepository.DeleteReservationById(reservationId);
        }

        /// <summary>
        /// Récupère toutes les réservations.
        /// </summary>
        /// <returns>Liste des réservations.</returns>
        public async Task<IEnumerable<ReservationDto>> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservations();
            return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        }

        /// <summary>
        /// Récupère une réservation par son identifiant.
        /// </summary>
        /// <param name="reservationId">Identifiant de la réservation.</param>
        /// <returns>La réservation correspondante.</returns>
        public async Task<ReservationDto> GetReservationById(int reservationId)
        {
            var reservationById = await _reservationRepository.GetReservationById(reservationId);
            return _mapper.Map<ReservationDto>(reservationById);
        }

        /// <summary>
        /// Récupère une réservation avec les plats associés.
        /// </summary>
        /// <param name="reservationId">Identifiant de la réservation.</param>
        /// <returns>La réservation contenant les informations des plats associés.</returns>
        public async Task<ReservationDto> GetReservationForPlats(int reservationId)
        {
            var reservationForPlats = await _reservationRepository.GetReservationForPlats(reservationId);
            return _mapper.Map<ReservationDto>(reservationForPlats);
        }

        /// <summary>
        /// Récupère toutes les réservations pour une date donnée.
        /// </summary>
        /// <param name="date">Date des réservations à récupérer.</param>
        /// <returns>Liste des réservations correspondant à la date spécifiée.</returns>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByDate(DateTime date)
        {
            var reservationsByDate = await _reservationRepository.GetReservationsByDate(date);
            return _mapper.Map<IEnumerable<ReservationDto>>(reservationsByDate);
        }

        /// <summary>
        /// Récupère toutes les réservations effectuées par un utilisateur donné.
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur.</param>
        /// <returns>Liste des réservations effectuées par l'utilisateur spécifié.</returns>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByUserId(int utilisateurId)
        {
            var reservationsByUserId = await _reservationRepository.GetReservationsByUserId(utilisateurId);
            return _mapper.Map<IEnumerable<ReservationDto>>(reservationsByUserId);
        }


        /// <summary>
        /// Supprime un plat à une réservation existante.
        /// </summary>
        /// <param name="platReservationDto">Données du plat et de la réservation.</param>
        /// <returns>True si la suppression est réussie, sinon False.</returns>
        public async Task<bool> RemovePlatFromReservation(PlatReservationDto platReservationDto)
        {
            if (platReservationDto == null)
            {
                throw new ArgumentNullException(nameof(platReservationDto));
            }

            var reservation = await _reservationRepository.GetReservationById(platReservationDto.IdReservation);

            if (reservation == null) 
            {
                throw new KeyNotFoundException($"Aucune réservation trouvée avec l'ID {platReservationDto.IdReservation}");
            }

            var plat = await _reservationRepository.GetReservationById(platReservationDto.IdPlat);

            return await _reservationRepository.RemovePlatFromReservation(platReservationDto);
        }

        /// <summary>
        /// Met à jour une réservation existante.
        /// </summary>
        /// <param name="reservationDto">Données mises à jour de la réservation.</param>
        /// <returns>La réservation mise à jour.</returns>
        public async Task<ReservationDto> UpdateReservation(ReservationDto reservationDto)
        {
            //Je transforme ReservationDto en entité Reservation
            var reservationEntity = _mapper.Map<Reservation>(reservationDto);

            //Je mets à jour la réservation dans la base de données
            var updatedReservation = await _reservationRepository.UpdateReservation(reservationEntity);

            //On effectue le mapping l'entité mise à jour vers un DTO (reservationDto).
            return _mapper.Map<ReservationDto>(updatedReservation);
        }

        /// <summary>
        /// Met à jour le statut d'une réservation.
        /// </summary>
        /// <param name="reservationId">Identifiant de la réservation.</param>
        /// <param name="nouveauStatut">Nouveau statut de la réservation.</param>
        /// <returns>La réservation mise à jour avec le nouveau statut.</returns>
        public async Task<ReservationDto> UpdateReservationStatus(int reservationId, ReservationStatusDto nouveauStatut)
        {
            //Je vérifie si la réservation existe
            var reservation = await _reservationRepository.GetReservationById(reservationId);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Aucune réservation trouvée avec l'ID {reservationId}");
            }

            //Si la réservation existe à ce moment là je modifie son statut
            reservation.Statut = nouveauStatut.ToString();

            //Je mets à jour la réservation en base de données
            var updatedReservation = await _reservationRepository.UpdateReservation(reservation);

            return _mapper.Map<ReservationDto>(updatedReservation);
        }
    }
}
