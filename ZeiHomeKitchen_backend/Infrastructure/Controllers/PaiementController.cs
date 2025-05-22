using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaiementController : ControllerBase
    {
        private readonly IPaiementService _paiementService;
        private readonly IStripeService _stripeService;
        private readonly ILogger<PaiementController> _logger;

        public PaiementController(IPaiementService paiementService, IStripeService stripeService, ILogger<PaiementController> logger)
        {
            _paiementService = paiementService;
            _stripeService = stripeService;
            _logger = logger;
        }

        /// <summary>
        /// Crée un PaymentIntent Stripe pour une réservation
        /// </summary>
        /// <param name="request">Données de la requête</param>
        /// <returns>Le client secret du PaymentIntent</returns>
        [HttpPost("create-payment-intent")]
        public async Task<ActionResult<PaymentIntentResponseDto>> CreatePaymentIntent([FromBody] CreatePaymentIntentRequestDto request)
        {
            try
            {
                // Validation des données
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Récupérer le paiement associé à la réservation
                var paiement = await _paiementService.GetPaiementByReservationId(request.ReservationId);

                if (paiement == null)
                {
                    return NotFound("Aucun paiement trouvé pour cette réservation");
                }

                // Création du PaymentIntent avec Stripe
                var paymentIntent = await _stripeService.CreatePaymentIntent(
                    paiement.Montant,
                    request.Currency,
                    request.Description ?? $"Paiement pour réservation #{request.ReservationId}"
                );

                // Mis à jour paiement avec les informations Stripe
                var updatedPaiement = new PaiementDto(
                    paiement.IdPaiement,
                    paiement.Montant,
                    paiement.Statut,
                    paiement.Moyen,
                    paiement.IdReservation,
                    paymentIntent.Id,
                    paymentIntent.ClientSecret,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    paymentIntent.Status
                );

                await _paiementService.UpdatePaiement(updatedPaiement);

                var response = new PaymentIntentResponseDto
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentIntentId = paymentIntent.Id,
                    Amount = paiement.Montant,
                    Currency = request.Currency,
                    ReservationId = request.ReservationId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la création du PaymentIntent pour la réservation {request.ReservationId}");
                return StatusCode(500, "Une erreur est survenue lors de la création du paiement");
            }
        }


        /// <summary>
        /// Confirme le paiement et met à jour le statut
        /// </summary>
        /// <param name="request">Données de confirmation du paiement</param>
        /// <returns>Statut de la confirmation</returns>
        [HttpPost("confirm-payment")]
        public async Task<ActionResult<ConfirmPaymentResponseDto>> ConfirmPayment([FromBody] ConfirmPaymentRequestDto request)
        {
            try
            {
                // Validation des données
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Récupération du PaymentIntent depuis Stripe
                var paymentIntent = await _stripeService.GetPaymentIntent(request.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    // Mise à jour du statut du paiement dans ma bdd
                    var paiement = await _paiementService.GetPaiementByReservationId(request.ReservationId);

                    if (paiement != null)
                    {
                        var updatedPaiement = new PaiementDto(
                            paiement.IdPaiement,
                            paiement.Montant,
                            PaiementStatusDto.Valide, // Statut validé
                            paiement.Moyen,
                            paiement.IdReservation,
                            paiement.StripePaymentIntentId,
                            paiement.StripeClientSecret,
                            paiement.DateCreation,
                            DateTime.UtcNow,
                            paymentIntent.Status
                        );

                        var updatedResult = await _paiementService.UpdatePaiement(updatedPaiement);

                        _logger.LogInformation($"Paiement confirmé avec succès pour la réservation {request.ReservationId}");

                        return Ok(new ConfirmPaymentResponseDto
                        {
                            Success = true,
                            Message = "Paiement confirmé avec succès",
                            Paiement = updatedResult
                        });
                    }
                }

                return BadRequest(new ConfirmPaymentResponseDto
                {
                    Success = false,
                    Message = "Le paiement n'a pas été confirmé"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la confirmation du paiement");
                return StatusCode(500, new ConfirmPaymentResponseDto
                {
                    Success = false,
                    Message = "Une erreur est survenue lors de la confirmation du paiement"
                });
            }
        }

        /// <summary>
        /// Webhook pour recevoir les événements de Stripe
        /// </summary>
        /// <returns>Statut de traitement du webhook</returns>
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                //On parse l'événement
                var stripeEvent = EventUtility.ParseEvent(json);

                // Traitement des différents types d'événements
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogInformation($"PaymentIntent {paymentIntent.Id} a réussi");
                        break;

                    case "payment_intent.payment_failed":
                        var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogWarning($"PaymentIntent {failedPaymentIntent.Id} a échoué");
                        // Mettre à jour le statut à "Echoue"
                        break;

                    default:
                        _logger.LogInformation($"Événement non traité: {stripeEvent.Type}");
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement du webhook Stripe");
                return StatusCode(500);
            }
        }




        /// <summary>
        /// Récupère tous les paiements.
        /// </summary>
        /// <returns>Une liste de DTOs de paiements.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaiementDto>>> GetAllPaiements()
        {
            try
            {
                var paiements = await _paiementService.GetAllPaiements();
                return Ok(paiements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des paiements");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des paiements");
            }
        }

        /// <summary>
        /// Récupère un paiement par son ID.
        /// </summary>
        /// <param name="idPaiement">L'ID du paiement à récupérer.</param>
        /// <returns>Le DTO du paiement correspondante.</returns>
        [HttpGet("{idPaiement}")]
        public async Task<ActionResult<PaiementDto>> GetPaiementById(int idPaiement)
        {
            try
            {
                var paiement = await _paiementService.GetPaiementById(idPaiement);
                if (paiement == null)
                {
                    return NotFound();
                }
                return Ok(paiement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération du paiement {idPaiement}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération du paiement");
            }
        }

        /// <summary>
        /// Crée un nouveau paiement à partir d'un DTO de paiement.
        /// </summary>
        /// <param name="paiementDto">Le DTO de paiement à créer.</param>
        /// <returns>Le DTO du paiement créé.</returns>
        [HttpPost]
        public async Task<ActionResult<PaiementDto>> CreatePaiement([FromBody] PaiementDto paiementDto)
        {
            if (paiementDto == null)
            {
                return BadRequest("Le DTO de paiement ne peut pas être nul.");
            }

            try
            {
                var createdPaiement = await _paiementService.CreatePaiement(paiementDto);
                return CreatedAtAction(nameof(GetPaiementById), new { idPaiement = createdPaiement.IdPaiement }, createdPaiement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du paiement");
                return StatusCode(500, "Une erreur est survenue lors de la création du paiement");
            }
        }

        /// <summary>
        /// Met à jour un paiement existant à partir d'un DTO de paiement.
        /// </summary>
        /// <param name="paiementDto">Le DTO de paiement avec les nouvelles données.</param>
        /// <returns>Le DTO du paiement mis à jour.</returns>
        [HttpPut]
        public async Task<ActionResult<PaiementDto>> UpdatePaiement([FromBody] PaiementDto paiementDto)
        {
            if (paiementDto == null)
            {
                return BadRequest("Le DTO de paiement ne peut pas être nul.");
            }

            try
            {
                var updatedPaiement = await _paiementService.UpdatePaiement(paiementDto);
                return Ok(updatedPaiement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du paiement");
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du paiement");
            }
        }

        /// <summary>
        /// Supprime un paiement par son ID.
        /// </summary>
        /// <param name="idPaiement">L'ID du paiement à supprimer.</param>
        /// <returns>Un résultat d'action indiquant le succès ou l'échec de la suppression.</returns>
        [HttpDelete("{idPaiement}")]
        public async Task<IActionResult> DeletePaiement(int idPaiement)
        {
            try
            {
                var result = await _paiementService.DeletePaiement(idPaiement);
                if (!result)
                {
                    return NotFound($"Le paiement avec l'ID {idPaiement} n'existe pas.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la suppression du paiement {idPaiement}");
                return StatusCode(500, "Une erreur est survenue lors de la suppression du paiement");
            }
        }

        /// <summary>
        /// Récupère un paiement par l'ID de réservation.
        /// </summary>
        /// <param name="reservationId">L'ID de la réservation liée au paiement.</param>
        /// <returns>Le DTO du paiement lié à la réservation.</returns>
        [HttpGet("by-reservation/{reservationId}")]
        public async Task<ActionResult<PaiementDto>> GetPaiementByReservationId(int reservationId)
        {
            try
            {
                var paiement = await _paiementService.GetPaiementByReservationId(reservationId);
                if (paiement == null)
                {
                    return NotFound();
                }
                return Ok(paiement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération du paiement {reservationId}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération du paiement");
            }
        }
    }
}
