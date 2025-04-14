using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Domain.Models;
using System.Security.Claims;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Application.Ports;


namespace ZeiHomeKitchen_backend.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;
        private readonly IStatistiqueService _statistiqueService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUtilisateurService _utilisateurService;
        private readonly ICreateReservationService _createReservationService;

        /// <summary>
        /// Constructeur pour le contrôleur ReservationController.
        /// </summary>
        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger, IStatistiqueService statistiqueService, IUtilisateurService utilisateurService, IHttpContextAccessor httpContextAccessor, ICreateReservationService createReservationService)
        {
            _reservationService = reservationService;
            _logger = logger;
            _statistiqueService = statistiqueService;
            _httpContextAccessor = httpContextAccessor;
            _utilisateurService = utilisateurService;
            _createReservationService = createReservationService;
        }

        /// <summary>
        /// Récupère toutes les réservations.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservations();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des réservations");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des réservations");
            }
        }

        /// <summary>
        /// Récupère une réservation par son ID.
        /// </summary>
        [HttpGet("{reservationId}")]
        public async Task<ActionResult<ReservationDto>> GetReservationById(int reservationId)
        {
            try
            {
                var reservationById = await _reservationService.GetReservationById(reservationId);
                if (reservationById == null)
                {
                    return NotFound();
                }
                return Ok(reservationById);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération de la réservation {reservationId}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la réservation");
            }
        }

        /// <summary>
        /// Récupère une réservation avec les plats associés.
        /// </summary>
        [HttpGet("{reservationId}/plats")]
        public async Task<ActionResult<ReservationDto>> GetReservationForPlats(int reservationId)
        {
            try
            {
                var reservationForPlats = await _reservationService.GetReservationForPlats(reservationId);
                if (reservationForPlats == null)
                {
                    return NotFound();
                }
                return Ok(reservationForPlats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération de la réservation {reservationId} avec les plats associés");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la réservation avec les plats associés");
            }
        }

        /// <summary>
        /// Crée une nouvelle réservation à partir d'un DTO de réservation.
        /// </summary>
        //[HttpPost]
        //[Authorize] // Assurez-vous que l'utilisateur est authentifié
        //public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReservationDto reservationDto)
        //{
        //    if (reservationDto == null)
        //    {
        //        return BadRequest("reservationDto ne peut pas être nul.");
        //    }

        //    try
        //    {
        //        _logger.LogInformation($"Réception DTO: IdReservation={reservationDto.IdReservation}, " +
        //            $"DateReservation={reservationDto.DateReservation}, " +
        //            $"Statut={reservationDto.Statut}, " +
        //            $"Nom={reservationDto.Nom}, " +
        //            $"Prenom={reservationDto.Prenom}, " +
        //            $"NombrePersonnes={reservationDto.NombrePersonnes}");

        //        if (!reservationDto.DateReservation.HasValue)
        //        {
        //            return BadRequest("La date de réservation est requise.");
        //        }

        //        // Récupérer le nom d'utilisateur de l'utilisateur connecté
        //        var username = _httpContextAccessor.HttpContext.User.Identity.Name;
        //        _logger.LogInformation($"Nom d'utilisateur récupéré: {username}");

        //        if (string.IsNullOrEmpty(username))
        //        {
        //            return Unauthorized("Utilisateur non authentifié");
        //        }

        //        // Récupérer l'utilisateur par son nom d'utilisateur
        //        var utilisateur = await _utilisateurService.GetUtilisateurByUsername(username);
        //        if (utilisateur == null)
        //        {
        //            return NotFound($"Utilisateur avec le nom d'utilisateur {username} non trouvé");
        //        }

        //        // Assigner l'ID utilisateur à la réservation
        //        reservationDto.IdUtilisateur = utilisateur.Id;
        //        _logger.LogInformation($"ID Utilisateur assigné à la réservation: {utilisateur.Id}");

        //        var date = DateOnly.FromDateTime(reservationDto.DateReservation.Value.Date);

        //        // Étape 1 : Utiliser le StatistiqueService pour obtenir la statistique par date
        //        var statistiqueDto = await _statistiqueService.GetStatistiqueByDate(date);

        //        // Vérifie si la statistique existe, sinon crée une nouvelle statistique
        //        if (statistiqueDto == null)
        //        {
        //            statistiqueDto = await _statistiqueService.CreateStatistique(new StatistiqueDto
        //            {
        //                DateStatistique = date,
        //                TotalReservation = 0 // Valeur initiale
        //            });
        //        }

        //        _logger.LogInformation($"Statistique créée avec ID: {statistiqueDto.IdStatistique}");
        //        reservationDto.IdStatistique = statistiqueDto.IdStatistique;
        //        _logger.LogInformation($"ID Statistique assigné à la réservation: {reservationDto.IdStatistique}");

        //        // Créer la réservation
        //        var createReservation = await _reservationService.CreateReservation(reservationDto);

        //        return CreatedAtAction(nameof(GetReservationById), new { reservationId = createReservation.IdReservation }, createReservation);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erreur lors de la création de la réservation");
        //        _logger.LogError($"Message : {ex.Message}, StackTrace : {ex.StackTrace}");
        //        return StatusCode(500, "Une erreur est survenue lors de la création de la réservation");
        //    }
        //}

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReservationDto reservationDto)
        {
            // Vérifiez que reservationDto n'est pas nul
            if (reservationDto == null)
            {
                return BadRequest("reservationDto ne peut pas être nul.");
            }

            try
            {
                // Log de réception du DTO
                _logger.LogInformation($"Réception DTO: IdReservation={reservationDto.IdReservation}, " +
                    $"DateReservation={reservationDto.DateReservation}, " +
                    $"Statut={reservationDto.Statut}, " +
                    $"Nom={reservationDto.Nom}, " +
                    $"Prenom={reservationDto.Prenom}, " +
                    $"NombrePersonnes={reservationDto.NombrePersonnes}");

                // Vérification de la date de réservation
                if (!reservationDto.DateReservation.HasValue)
                {
                    return BadRequest("La date de réservation est requise.");
                }


                //var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id");
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("Utilisateur non authentifié");
                }

                //Je convertis l'ID de l'utilisateur en entier
                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest("L'ID de l'utilisateur est invalide.");
                }

                //J'assigne l'ID utilisateur à la réservation
                reservationDto.IdUtilisateur = userId;
                _logger.LogInformation($"ID Utilisateur assigné à la réservation: {reservationDto.IdUtilisateur}");

                // Conversion de la date
                //var date = DateOnly.FromDateTime(reservationDto.DateReservation.Value.Date);

                //// Étape 1 : Obtenir la statistique par date
                //var statistiqueDto = await _statistiqueService.GetStatistiqueByDate(date);

                //// Vérifiez si la statistique existe, sinon créez une nouvelle statistique
                //if (statistiqueDto == null)
                //{
                //    statistiqueDto = await _statistiqueService.CreateStatistique(new StatistiqueDto
                //    {
                //        DateStatistique = date,
                //        TotalReservation = 0 // Valeur initiale
                //    });
                //    _logger.LogInformation($"Nouvelle statistique créée avec ID: {statistiqueDto.IdStatistique}");
                //}


                //// Assigner l'ID de la statistique à la réservation
                //reservationDto.IdStatistique = statistiqueDto.IdStatistique;
                //_logger.LogInformation($"ID Statistique assigné à la réservation: {reservationDto.IdStatistique}");

                //// Log de création de la réservation
                //_logger.LogInformation($"Création de réservation avec IdUtilisateur: {reservationDto.IdUtilisateur}, IdStatistique: {reservationDto.IdStatistique}");

                //_logger.LogInformation("Création de réservation avec les données suivantes: {@ReservationDto}", reservationDto);

                // Log de création de la réservation sans sérialiser les détails des réservations
                _logger.LogInformation("Création de réservation avec les données suivantes: {@ReservationDto}",
                    new { reservationDto.IdReservation, reservationDto.DateReservation, reservationDto.Adresse, reservationDto.Statut, reservationDto.Nom, reservationDto.Prenom, reservationDto.NombrePersonnes });

                // Créer la réservation
                var createReservation = await _reservationService.CreateReservation(reservationDto);

                // Retourner la réponse avec le code 201 Created
                return CreatedAtAction(nameof(GetReservationById), new { reservationId = createReservation.IdReservation }, createReservation);
            }
            catch (Exception ex)
            {
                // Log des erreurs
                _logger.LogError(ex, "Erreur lors de la création de la réservation: {Message}", ex.Message);
                _logger.LogError($"Message: {ex.Message}, StackTrace : {ex.StackTrace}");
                return StatusCode(500, "Une erreur est survenue lors de la création de la réservation");
            }
        }





        /// <summary>
        /// Ajoute un plat à une réservation existante.
        /// </summary>
        [HttpPost("plat")]
        public async Task<ActionResult<bool>> AddPlatToReservation([FromBody] PlatReservationDto platReservationDto)
        {
            if (platReservationDto == null)
            {
                return BadRequest("platReservationDto ne peut pas être nul.");
            }

            try
            {
                var result = await _reservationService.AddPlatToReservation(platReservationDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout du plat à la réservation");
                return StatusCode(500, "Une erreur est survenue lors de l'ajout du plat à la réservation");
            }
        }

        /// <summary>
        /// Supprime un plat d'une réservation existante.
        /// </summary>
        [HttpDelete("plat")]
        public async Task<ActionResult<bool>> RemovePlatFromReservation([FromBody] PlatReservationDto platReservationDto)
        {
            if (platReservationDto == null)
            {
                return BadRequest("Le DTO de la réservation de plat ne peut pas être nul.");
            }

            try
            {
                var result = await _reservationService.RemovePlatFromReservation(platReservationDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du plat de la réservation");
                return StatusCode(500, "Une erreur est survenue lors de la suppression du plat de la réservation");
            }
        }

        /// <summary>
        /// Met à jour une réservation existante à partir d'un DTO de réservation.
        /// </summary>
        [HttpPut("{reservationId}")]
        public async Task<ActionResult<ReservationDto>> UpdateReservation(int reservationId, [FromBody] ReservationDto reservationDto)
        {
            if (reservationId != reservationDto.IdReservation)
            {
                return BadRequest("L'ID ne correspond pas.");
            }

            try
            {
                var updatedReservation = await _reservationService.UpdateReservation(reservationDto);
                if (updatedReservation == null)
                {
                    return NotFound();
                }
                return Ok(updatedReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la mise à jour de la réservation {reservationId}");
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de la réservation");
            }
        }

        /// <summary>
        /// Supprime une réservation par son ID.
        /// </summary>
        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            try
            {
                var result = await _reservationService.DeleteReservationById(reservationId);
                if (!result)
                {
                    return NotFound($"La réservation avec l'ID {reservationId} n'existe pas.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la suppression de la réservation {reservationId}");
                return StatusCode(500, "Une erreur est survenue lors de la suppression de la réservation");
            }
        }
    }
}
