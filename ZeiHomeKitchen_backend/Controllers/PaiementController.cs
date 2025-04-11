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
    public class PaiementController : ControllerBase
    {
        private readonly IPaiementService _paiementService;
        private readonly ILogger<PaiementController> _logger;

        public PaiementController(IPaiementService paiementService, ILogger<PaiementController> logger)
        {
            _paiementService = paiementService;
            _logger = logger;
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
    }
}
