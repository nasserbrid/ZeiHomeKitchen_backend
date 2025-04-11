using AutoMapper;
using Newtonsoft.Json;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.MappingConfiguration;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;

namespace ZeiHomeKitchen_backend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPlatRepository _platRepository;
        private readonly IPaiementRepository _paiementRepository;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IReservationRepository reservationRepository,
            IPlatRepository platRepository,
            IPaiementRepository paiementRepository,
            ILogger<ReservationService> logger)
        {
            _reservationRepository = reservationRepository;
            _paiementRepository = paiementRepository;
            _platRepository = platRepository;
            _logger = logger;
        }

        /// <summary>
        /// Récupère toutes les réservations.
        /// </summary>
        public async Task<IEnumerable<ReservationDto>> GetAllReservations()
        {
            try
            {
                var reservations = await _reservationRepository.GetAllReservations();
                return reservations.Select(r => r.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des réservations depuis le dépôt");
                throw; 
            }
        }


        /// <summary>
        /// Récupère une réservation par son ID.
        /// </summary>
        public async Task<ReservationDto> GetReservationById(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationById(reservationId);
            return reservation?.ToDto();
        }

        /// <summary>
        /// Crée une nouvelle réservation et déclenche le paiement.
        /// </summary>
        public async Task<ReservationDto> CreateReservation(ReservationDto reservationDto)
        {
            if (reservationDto == null)
                throw new ArgumentNullException(nameof(reservationDto));

            var reservationEntity = reservationDto.ToModel();

            if (reservationDto.PlatIds != null && reservationDto.PlatIds.Any())
            {
                foreach (var platId in reservationDto.PlatIds)
                {
                    var plat = await _platRepository.GetPlatById(platId); 
                    if (plat != null)
                    {
                        reservationEntity.Plats.Add(plat);
                    }
                }
            }

            // Créer la réservation
            var created = await _reservationRepository.CreateReservation(reservationEntity);

            
            var completeReservation = await _reservationRepository.GetReservationById(created.IdReservation);

            decimal montantTotal = await CalculateMontantForReservation(created.IdReservation);
            await CreatePaiementForReservation(created.IdReservation, montantTotal, PaiementMoyenDto.CB);

            //_logger.LogInformation($"Réservation créée avec succès: {JsonConvert.SerializeObject(created)}");
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            _logger.LogInformation($"Réservation créée avec succès: {JsonConvert.SerializeObject(created, settings)}");

            //return created.ToDto();
            return completeReservation.ToDto();
        }

        //public async Task<ReservationDto> CreateReservation(ReservationDto reservationDto)
        //{
        //    if (reservationDto == null)
        //        throw new ArgumentNullException(nameof(reservationDto));

        //    var reservationEntity = reservationDto.ToModel();

        //    // Log des données de la réservation avant la création
        //    _logger.LogInformation($"Tentative de création de réservation: {JsonConvert.SerializeObject(reservationEntity)}");

        //    var created = await _reservationRepository.CreateReservation(reservationEntity);

        //    // Vérifie les plats associés et calcule le montant
        //    decimal montantTotal = await CalculateMontantForReservation(created.IdReservation);
        //    await CreatePaiementForReservation(created.IdReservation, montantTotal, PaiementMoyenDto.CB);

        //    _logger.LogInformation($"Réservation créée avec succès: {JsonConvert.SerializeObject(created)}");

        //    return created.ToDto();
        //}

        //public async Task<ReservationDto> CreateReservation(ReservationDto reservationDto)
        //{
        //    if (reservationDto == null)
        //        throw new ArgumentNullException(nameof(reservationDto));

        //    var reservationEntity = reservationDto.ToModel();
        //    var created = await _reservationRepository.CreateReservation(reservationEntity);

        //    // Calculer le montant total pour la réservation
        //    decimal montantTotal = await CalculateMontantForReservation(created.IdReservation);
        //    await CreatePaiementForReservation(created.IdReservation, montantTotal, PaiementMoyenDto.CB);

        //    _logger.LogInformation($"Réservation créée avec succès: {JsonConvert.SerializeObject(created)}");

        //    return created.ToDto();
        //}

        /// <summary>
        /// Met à jour une réservation existante.
        /// </summary>
        public async Task<ReservationDto> UpdateReservation(ReservationDto reservationDto)
        {
            if (reservationDto == null)
                throw new ArgumentNullException(nameof(reservationDto));

            var entity = reservationDto.ToModel();
            var updated = await _reservationRepository.UpdateReservation(entity);
            return updated.ToDto();
        }

        /// <summary>
        /// Supprime une réservation par son ID.
        /// </summary>
        public async Task<bool> DeleteReservationById(int reservationId)
        {
            return await _reservationRepository.DeleteReservationById(reservationId);
        }

        /// <summary>
        /// Récupère une réservation avec les plats associés.
        /// </summary>
        public async Task<ReservationDto> GetReservationForPlats(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationForPlats(reservationId);
            return reservation?.ToDto();
        }

        /// <summary>
        /// Récupère toutes les réservations effectuées par un utilisateur donné.
        /// </summary>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByUserId(int utilisateurId)
        {
            var reservations = await _reservationRepository.GetReservationsByUserId(utilisateurId);
            return reservations.Select(r => r.ToDto());
        }

        /// <summary>
        /// Récupère toutes les réservations effectuées à une date donnée.
        /// </summary>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByDate(DateTime date)
        {
            var reservations = await _reservationRepository.GetReservationsByDate(date);
            return reservations.Select(r => r.ToDto());
        }

        /// <summary>
        /// Met à jour le statut d'une réservation.
        /// </summary>
        public async Task<ReservationDto> UpdateReservationStatus(int reservationId, ReservationStatusDto nouveauStatut)
        {
            var updated = await _reservationRepository.UpdateReservationStatus(reservationId, nouveauStatut);
            return updated?.ToDto();
        }

        /// <summary>
        /// Ajoute un plat à une réservation existante.
        /// </summary>
        public async Task<bool> AddPlatToReservation(PlatReservationDto platReservationDto)
        {
            if (platReservationDto == null)
                throw new ArgumentNullException(nameof(platReservationDto));

            return await _reservationRepository.AddPlatToReservation(platReservationDto);
        }

        /// <summary>
        /// Supprime un plat d'une réservation existante.
        /// </summary>
        public async Task<bool> RemovePlatFromReservation(PlatReservationDto platReservationDto)
        {
            if (platReservationDto == null)
                throw new ArgumentNullException(nameof(platReservationDto));

            return await _reservationRepository.RemovePlatFromReservation(platReservationDto);
        }

        /// <summary>
        /// Calcule le montant total d'une réservation, incluant la TVA.
        /// </summary>
        public async Task<decimal> CalculateMontantForReservation(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationForPlats(reservationId);
            if (reservation == null || reservation.Plats == null || !reservation.Plats.Any())
                throw new Exception("Aucune réservation ou plat trouvé pour le calcul du montant.");

            decimal total = reservation.Plats.Sum(plat => plat.Prix.GetValueOrDefault() * reservation.NombrePersonnes);
            decimal tva = 0.20m; 
            total += total * tva;

            return total;
        }

        /// <summary>
        /// Crée un paiement pour une réservation donnée.
        /// </summary>
        public async Task<Paiement> CreatePaiementForReservation(int reservationId, decimal montant, PaiementMoyenDto moyen)
        {
            if (montant <= 0)
                throw new ArgumentException("Le montant doit être supérieur à zéro.");

            var paiement = new Paiement
            {
                Montant = montant,
                Statut = PaiementStatusDto.EnAttente.ToString(),
                Moyen = moyen.ToString(),
                IdReservation = reservationId
            };

            return await _paiementRepository.CreatePaiement(paiement);
        }
    }
}
