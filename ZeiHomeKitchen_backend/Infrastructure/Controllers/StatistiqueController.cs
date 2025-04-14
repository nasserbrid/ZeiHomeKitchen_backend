//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using ZeiHomeKitchen_backend.Dtos;
//using ZeiHomeKitchen_backend.Services;

//namespace ZeiHomeKitchen_backend.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class StatistiqueController : ControllerBase
//    {
//        private readonly IStatistiqueService _statistiqueService;
//        private readonly ILogger<StatistiqueController> _logger;

//        /// <summary>
//        /// Constructeur pour le contrôleur StatistiqueController.
//        /// </summary>
//        /// <param name="statistiqueService">L'interface du service de statistiques.</param>
//        /// <param name="logger">L'instance du logger pour les journaux d'activité.</param>
//        public StatistiqueController(IStatistiqueService statistiqueService, ILogger<StatistiqueController> logger)
//        {
//            _statistiqueService = statistiqueService;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Récupère toutes les statistiques.
//        /// </summary>
//        /// <returns>Une liste de DTOs de statistiques.</returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<StatistiqueDto>>> GetAllStatistiques()
//        {
//            try
//            {
//                var statistiques = await _statistiqueService.GetAllStatistiques();
//                return Ok(statistiques);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erreur lors de la récupération des statistiques");
//                return StatusCode(500, "Une erreur est survenue lors de la récupération des statistiques");
//            }
//        }

//        /// <summary>
//        /// Récupère une statistique par son ID.
//        /// </summary>
//        /// <param name="statistiqueId">L'ID de la statistique à récupérer.</param>
//        /// <returns>Le DTO de la statistique correspondante.</returns>
//        [HttpGet("{statistiqueId}")]
//        public async Task<ActionResult<StatistiqueDto>> GetStatistiqueById(int statistiqueId)
//        {
//            try
//            {
//                var statistique = await _statistiqueService.GetStatistiqueById(statistiqueId);
//                if (statistique == null)
//                {
//                    return NotFound();
//                }
//                return Ok(statistique);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Erreur lors de la récupération de la statistique {statistiqueId}");
//                return StatusCode(500, "Une erreur est survenue lors de la récupération de la statistique");
//            }
//        }

//        /// <summary>
//        /// Crée une nouvelle statistique à partir d'un DTO de statistique.
//        /// </summary>
//        /// <param name="statistiqueDto">Le DTO de statistique à créer.</param>
//        /// <returns>Le DTO de la statistique créée.</returns>
//        [HttpPost]
//        public async Task<ActionResult<StatistiqueDto>> CreateStatistique([FromBody] StatistiqueDto statistiqueDto)
//        {
//            if (statistiqueDto == null)
//            {
//                return BadRequest("statistiqueDto ne peut pas être nul.");
//            }

//            try
//            {
//                var createdStatistique = await _statistiqueService.CreateStatistique(statistiqueDto);
//                return CreatedAtAction(nameof(GetStatistiqueById), new { statistiqueId = createdStatistique.IdStatistique }, createdStatistique);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erreur lors de la création de la statistique");
//                return StatusCode(500, "Une erreur est survenue lors de la création de la statistique");
//            }
//        }

//        /// <summary>
//        /// Met à jour une statistique existante à partir d'un DTO de statistique.
//        /// </summary>
//        /// <param name="statistiqueDto">Le DTO de statistique avec les nouvelles données.</param>
//        /// <returns>Le DTO de la statistique mise à jour.</returns>
//        [HttpPut]
//        public async Task<ActionResult<StatistiqueDto>> UpdateStatistique([FromBody] StatistiqueDto statistiqueDto)
//        {
//            if (statistiqueDto == null)
//            {
//                return BadRequest("statistiqueDto ne peut pas être nul.");
//            }

//            try
//            {
//                var updatedStatistique = await _statistiqueService.UpdateStatistique(statistiqueDto);
//                if (updatedStatistique == null)
//                {
//                    return NotFound();
//                }
//                return Ok(updatedStatistique);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erreur lors de la mise à jour de la statistique");
//                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de la statistique");
//            }
//        }

//        /// <summary>
//        /// Supprime une statistique par son ID.
//        /// </summary>
//        /// <param name="statistiqueId">L'ID de la statistique à supprimer.</param>
//        /// <returns>Un résultat d'action indiquant le succès ou l'échec de la suppression.</returns>
//        [HttpDelete("{statistiqueId}")]
//        public async Task<IActionResult> DeleteStatistique(int statistiqueId)
//        {
//            try
//            {
//                var result = await _statistiqueService.DeleteStatistiqueById(statistiqueId);
//                if (!result)
//                {
//                    return NotFound($"La statistique avec l'ID {statistiqueId} n'existe pas.");
//                }
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Erreur lors de la suppression de la statistique {statistiqueId}");
//                return StatusCode(500, "Une erreur est survenue lors de la suppression de la statistique");
//            }
//        }

//        /// <summary>
//        /// Récupère une statistique par date.
//        /// </summary>
//        /// <param name="date">Date de la statistique à récupérer.</param>
//        /// <returns>La statistique correspondante pour la date spécifiée.</returns>
//        [HttpGet("date/{date}")]
//        public async Task<ActionResult<StatistiqueDto>> GetStatistiqueByDate(DateOnly date)
//        {
//            try
//            {
//                var statistique = await _statistiqueService.GetStatistiqueByDate(date);
//                if (statistique == null)
//                {
//                    return NotFound();
//                }
//                return Ok(statistique);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Erreur lors de la récupération de la statistique pour la date {date}");
//                return StatusCode(500, "Une erreur est survenue lors de la récupération de la statistique");
//            }
//        }

//        /// <summary>
//        /// Récupère les statistiques pour une période donnée.
//        /// </summary>
//        /// <param name="startDate">Date de début de la période.</param>
//        /// <param name="endDate">Date de fin de la période.</param>
//        /// <returns>Liste des statistiques pour la période spécifiée.</returns>
//        [HttpGet("period")]
//        public async Task<ActionResult<IEnumerable<StatistiqueDto>>> GetStatistiquesForPeriod(DateOnly startDate, DateOnly endDate)
//        {
//            try
//            {
//                var statistiques = await _statistiqueService.GetStatistiquesForPeriod(startDate, endDate);
//                return Ok(statistiques);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Erreur lors de la récupération des statistiques pour la période du {startDate} au {endDate}");
//                return StatusCode(500, "Une erreur est survenue lors de la récupération des statistiques");
//            }
//        }
//    }
//}
