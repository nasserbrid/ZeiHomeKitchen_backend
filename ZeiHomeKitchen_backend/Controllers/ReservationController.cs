using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Services;

namespace ZeiHomeKitchen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;

        /// <summary>
        /// Constructeur pour le contrôleur ReservationController.
        /// </summary>
        /// <param name="reservationService">L'interface du service de réservation.</param>
        /// <param name="logger">L'instance du logger pour les journaux d'activité.</param>
        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        /// <summary>
        /// Récupère toutes les réservations.
        /// </summary>
        /// <returns>Une liste de DTOs de réservations.</returns>
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
        /// <param name="reservationId">L'ID de la réservation à récupérer.</param>
        /// <returns>Le DTO de la réservation correspondante.</returns>
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
        /// Crée une nouvelle réservation à partir d'un DTO de réservation.
        /// </summary>
        /// <param name="reservationDto">Le DTO de réservation à créer.</param>
        /// <returns>Le DTO de la réservation créée.</returns>
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReservationDto reservationDto)
        {
            if (reservationDto == null)
            {
                return BadRequest("reservationDto ne peut pas être nul.");
            }

            try
            {
                var createReservation = await _reservationService.CreateReservation(reservationDto);
                return CreatedAtAction(nameof(GetReservationById), new { reservationId = createReservation.IdReservation }, createReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la réservation");
                return StatusCode(500, "Une erreur est survenue lors de la création de la réservation");
            }
        }

        /// <summary>
        /// Ajoute un plat à une réservation existante.
        /// </summary>
        /// <param name="platReservationDto">Données du plat et de la réservation.</param>
        /// <returns>True si l'ajout est réussi, sinon False.</returns>
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
        /// <param name="platReservationDto">Données du plat et de la réservation.</param>
        /// <returns>True si la suppression est réussie, sinon False.</returns>
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
        /// <param name="reservationId">L'ID de la réservation à mettre à jour.</param>
        /// <param name="reservationDto">Le DTO de réservation avec les nouvelles données.</param>
        /// <returns>Le DTO de la réservation mise à jour.</returns>
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
        /// <param name="reservationId">L'ID de la réservation à supprimer.</param>
        /// <returns>Un résultat d'action indiquant le succès ou l'échec de la suppression.</returns>
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
