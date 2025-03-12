using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeiHomeKichen_backend.Dtos;
using ZeiHomeKichen_backend.Services;

namespace ZeiHomeKichen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientController> _logger;

        public IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;    
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
        {
            try
            {
                var ingredients = await _ingredientService.GetIngredients();
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des ingrédients");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des ingrédients");
            }     
        }

        [HttpGet("{ingredientId}")]
        public async Task<ActionResult<IngredientDto>> GetIngredient(int ingredientId)
        {
            try
            {
                var ingredientById = await _ingredientService.GetIngredient(ingredientId);
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
                return CreatedAtAction(nameof(GetIngredient), new { ingredientId = createIngredient.IdIngredient }, createIngredient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'ingrédient");
                return StatusCode(500, "Une erreur est survenue lors de la création de l'ingrédient");   
            }  
        }

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
    }
}
