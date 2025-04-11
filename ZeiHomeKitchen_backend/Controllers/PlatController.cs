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
    public class PlatController : ControllerBase
    {
        private readonly IPlatService _platService;
        private readonly ILogger<PlatController> _logger;
        private readonly IImagesService _imagesService;

        public PlatController(IPlatService platService, ILogger<PlatController> logger, IImagesService imagesService)
        {
            _platService = platService;
            _logger = logger;
            _imagesService = imagesService;
        }

        /// <summary>
        /// Récupère la liste de tous les plats.
        /// </summary>
        /// <returns>Une collection de plats sous forme de DTO.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatDto>>> GetAllPlats()
        {
            try
            {
                var plats = await _platService.GetAllPlats();
                return Ok(plats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des plats");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des plats");
            }
        }

        /// <summary>
        /// Récupère un plat par son ID.
        /// </summary>
        /// <param name="platId">ID du plat.</param>
        /// <returns>Le plat sous forme de DTO.</returns>
        [HttpGet("{platId}")]
        public async Task<ActionResult<PlatDto>> GetPlatById(int platId)
        {
            try
            {
                var plat = await _platService.GetPlatById(platId);
                if (plat == null)
                {
                    return NotFound();
                }
                return Ok(plat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération du plat {platId}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération du plat");
            }
        }

        /// <summary>
        /// Crée un nouveau plat.
        /// </summary>
        /// <param name="nom">Nom du plat.</param>
        /// <param name="description">Description du plat.</param>
        /// <param name="prix">Prix du plat.</param>
        /// <param name="image">Image du plat.</param>
        /// <returns>Le plat créé sous forme de DTO.</returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PlatDto>> CreateNewPlat([FromForm] string nom, [FromForm] string description, [FromForm] decimal? prix, IFormFile image)
        {
            if (string.IsNullOrEmpty(nom) || image == null)
            {
                return BadRequest("Le nom et l'image sont obligatoires.");
            }

            try
            {
                byte[] optimizedImage;

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    optimizedImage = _imagesService.OptimizeImage(imageBytes);
                }

                var platDto = new PlatDto(0, nom, description, optimizedImage, prix);
                var createdPlat = await _platService.CreateNewPlat(platDto);
                return CreatedAtAction(nameof(GetPlatById), new { platId = createdPlat.IdPlat }, createdPlat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du plat");
                return StatusCode(500, "Une erreur est survenue lors de la création du plat");
            }
        }

        /// <summary>
        /// Met à jour un plat existant.
        /// </summary>
        /// <param name="platId">ID du plat à mettre à jour.</param>
        /// <param name="platDto">Données du plat mises à jour.</param>
        /// <returns>Le plat mis à jour sous forme de DTO.</returns>
        [HttpPut("{platId}")]
        public async Task<ActionResult<PlatDto>> UpdateExistingPlat(int platId, [FromBody] PlatDto platDto)
        {
            if (platId != platDto.IdPlat)
            {
                return BadRequest("L'ID ne correspond pas");
            }

            try
            {
                var updatedPlat = await _platService.UpdateExistingPlat(platDto);
                if (updatedPlat == null)
                {
                    return NotFound();
                }
                return Ok(updatedPlat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la mise à jour du plat {platId}");
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du plat");
            }
        }

        /// <summary>
        /// Supprime un plat par son ID.
        /// </summary>
        /// <param name="platId">ID du plat à supprimer.</param>
        /// <returns>Status 204 si la suppression a réussi, sinon NotFound.</returns>
        [HttpDelete("{platId}")]
        public async Task<IActionResult> DeletePlatById(int platId)
        {
            try
            {
                var result = await _platService.DeletePlatById(platId);
                if (!result)
                {
                    return NotFound($"Le plat avec l'ID {platId} n'existe pas.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la suppression du plat {platId}");
                return StatusCode(500, "Une erreur est survenue lors de la suppression du plat");
            }
        }

        /// <summary>
        /// Récupère les ingrédients d'un plat donné.
        /// </summary>
        /// <param name="platId">ID du plat.</param>
        /// <returns>Une collection d'ingrédients sous forme de DTO.</returns>
        [HttpGet("{platId}/ingredients")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredientsByPlatId(int platId)
        {
            try
            {
                var ingredients = await _platService.GetIngredientsByPlat(platId);
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération des ingrédients du plat {platId}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des ingrédients du plat");
            }
        }

        ///// <summary>
        ///// Ajoute un ingrédient à un plat.
        ///// </summary>
        ///// <param name="platId">ID du plat.</param>
        ///// <param name="ingredientId">ID de l'ingrédient à ajouter.</param>
        ///// <returns>Status 200 si l'ajout a réussi, sinon NotFound.</returns>
        //[HttpPost("{platId}/ingredients/{ingredientId}")]
        //public async Task<IActionResult> AddIngredientToPlatById(int platId, int ingredientId)
        //{
        //    try
        //    {
        //        var result = await _platService.LinkIngredientToPlat(platId, ingredientId);
        //        if (!result)
        //        {
        //            return NotFound("Le plat ou l'ingrédient n'existe pas.");
        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Erreur lors de l'ajout de l'ingrédient {ingredientId} au plat {platId}");
        //        return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'ingrédient au plat");
        //    }
        //}

        ///// <summary>
        ///// Supprime un ingrédient d'un plat.
        ///// </summary>
        ///// <param name="platId">ID du plat.</param>
        ///// <param name="ingredientId">ID de l'ingrédient à supprimer.</param>
        ///// <returns>Status 204 si la suppression a réussi, sinon NotFound.</returns>
        //[HttpDelete("{platId}/ingredients/{ingredientId}")]
        //public async Task<IActionResult> RemoveIngredientFromPlatById(int platId, int ingredientId)
        //{
        //    try
        //    {
        //        var result = await _platService.RemoveIngredientFromPlat(platId, ingredientId);
        //        if (!result)
        //        {
        //            return NotFound("Le plat ou l'ingrédient n'existe pas ou n'est pas associé.");
        //        }
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Erreur lors de la suppression de l'ingrédient {ingredientId} du plat {platId}");
        //        return StatusCode(500, "Une erreur est survenue lors de la suppression de l'ingrédient du plat");
        //    }
        //}

        ///// <summary>
        ///// Récupère un plat avec ses ingrédients associés.
        ///// </summary>
        ///// <param name="platId">ID du plat.</param>
        ///// <returns>Le plat avec ses ingrédients sous forme de DTO.</returns>
        //[HttpGet("{platId}/with-ingredients")]
        //public async Task<ActionResult<PlatDto>> GetPlatDetailsWithIngredients(int platId)
        //{
        //    try
        //    {
        //        var plat = await _platService.GetPlatDetailsWithIngredients(platId);
        //        if (plat == null)
        //        {
        //            return NotFound($"Le plat avec l'ID {platId} n'existe pas.");
        //        }
        //        return Ok(plat);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Erreur lors de la récupération du plat {platId} avec ses ingrédients");
        //        return StatusCode(500, "Une erreur est survenue lors de la récupération du plat avec ses ingrédients");
        //    }
        //}
    }
}
