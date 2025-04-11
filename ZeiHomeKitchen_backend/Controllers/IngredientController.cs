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
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientController> _logger;

        /// <summary>
        /// Constructeur pour le contrôleur IngredientController.
        /// </summary>
        /// <param name="ingredientService">L'interface du service d'ingrédients.</param>
        /// <param name="logger">L'instance du logger pour les journaux d'activité.</param>
        public IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        /// <summary>
        /// Récupère tous les ingrédients.
        /// </summary>
        /// <returns>Une liste de DTOs d'ingrédients.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredients()
        {
            try
            {
                var ingredients = await _ingredientService.GetAllIngredients();
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des ingrédients");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des ingrédients");
            }
        }

        /// <summary>
        /// Récupère un ingrédient par son ID.
        /// </summary>
        /// <param name="ingredientId">L'ID de l'ingrédient à récupérer.</param>
        /// <returns>Le DTO de l'ingrédient correspondant.</returns>
        [HttpGet("{ingredientId}")]
        public async Task<ActionResult<IngredientDto>> GetIngredientById(int ingredientId)
        {
            try
            {
                var ingredientById = await _ingredientService.GetIngredientById(ingredientId);
                if (ingredientById == null)
                {
                    return NotFound();
                }
                return Ok(ingredientById);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération de l'ingrédient {ingredientId}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'ingrédient");
            }
        }

        /// <summary>
        /// Crée un nouvel ingrédient à partir d'un DTO d'ingrédient.
        /// </summary>
        /// <param name="ingredientDto">Le DTO d'ingrédient à créer.</param>
        /// <returns>Le DTO de l'ingrédient créé.</returns>
        [HttpPost]
        public async Task<ActionResult<IngredientDto>> CreateIngredient([FromBody] IngredientDto ingredientDto)
        {
            if (ingredientDto == null)
            {
                return BadRequest("Le DTO de l'ingrédient ne peut pas être nul.");
            }

            try
            {
                var createIngredient = await _ingredientService.CreateIngredient(ingredientDto);
                return CreatedAtAction(nameof(GetIngredientById), new { ingredientId = createIngredient.IdIngredient }, createIngredient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'ingrédient");
                return StatusCode(500, "Une erreur est survenue lors de la création de l'ingrédient");
            }
        }

        /// <summary>
        /// Met à jour un ingrédient existant à partir d'un DTO d'ingrédient.
        /// </summary>
        /// <param name="ingredientId">L'ID de l'ingrédient à mettre à jour.</param>
        /// <param name="ingredientDto">Le DTO d'ingrédient avec les nouvelles données.</param>
        /// <returns>Le DTO de l'ingrédient mis à jour.</returns>
        [HttpPut("{ingredientId}")]
        public async Task<ActionResult<IngredientDto>> UpdateIngredient(int ingredientId, [FromBody] IngredientDto ingredientDto)
        {
            if (ingredientId != ingredientDto.IdIngredient)
            {
                return BadRequest("L'ID ne correspond pas");
            }

            try
            {
                var updatedIngredient = await _ingredientService.UpdateIngredient(ingredientDto);
                if (updatedIngredient == null)
                {
                    return NotFound();
                }
                return Ok(updatedIngredient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la mise à jour de l'ingrédient {ingredientId}");
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'ingrédient");
            }
        }

        /// <summary>
        /// Supprime un ingrédient par son ID.
        /// </summary>
        /// <param name="ingredientId">L'ID de l'ingrédient à supprimer.</param>
        /// <returns>Un résultat d'action indiquant le succès ou l'échec de la suppression.</returns>
        [HttpDelete("{ingredientId}")]
        public async Task<IActionResult> DeleteIngredient(int ingredientId)
        {
            try
            {
                var result = await _ingredientService.DeleteIngredient(ingredientId);
                if (!result)
                {
                    return NotFound($"L'ingrédient avec l'ID {ingredientId} n'existe pas.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la suppression de l'ingrédient {ingredientId}");
                return StatusCode(500, "Une erreur est survenue lors de la suppression de l'ingrédient");
            }
        }

        ///// <summary>
        ///// Récupère les plats associés à un ingrédient par son ID.
        ///// </summary>
        ///// <param name="ingredientId">L'ID de l'ingrédient pour lequel récupérer les plats.</param>
        ///// <returns>Une collection de DTOs de plats associés.</returns>
        //[HttpGet("{ingredientId}/plats")]
        //public async Task<ActionResult<IEnumerable<PlatDto>>> GetPlatsByIngredientId(int ingredientId)
        //{
        //    try
        //    {
        //        var plats = await _ingredientService.GetPlatsByIngredientId(ingredientId);
        //        return Ok(plats);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Erreur lors de la récupération des plats pour l'ingrédient {ingredientId}");
        //        return StatusCode(500, "Une erreur est survenue lors de la récupération des plats de l'ingrédient");
        //    }
        //}

        ///// <summary>
        ///// Récupère un ingrédient avec ses plats associés par son ID.
        ///// </summary>
        ///// <param name="ingredientId">L'ID de l'ingrédient à récupérer avec ses plats.</param>
        ///// <returns>Le DTO de l'ingrédient avec ses plats associés.</returns>
        //[HttpGet("{ingredientId}/with-plats")]
        //public async Task<ActionResult<IngredientDto>> GetIngredientWithPlats(int ingredientId)
        //{
        //    try
        //    {
        //        var ingredient = await _ingredientService.GetIngredientWithPlats(ingredientId);
        //        if (ingredient == null)
        //        {
        //            return NotFound($"L'ingrédient avec l'ID {ingredientId} n'existe pas.");
        //        }
        //        return Ok(ingredient);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Erreur lors de la récupération de l'ingrédient {ingredientId} avec ses plats");
        //        return StatusCode(500, "Une erreur est survenue lors de la récupération de l'ingrédient avec ses plats");
        //    }
        //}
    }
}
