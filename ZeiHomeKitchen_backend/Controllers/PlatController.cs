using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Services;



namespace ZeiHomeKitchen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatController:ControllerBase
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatDto>>> GetPlats()
        {
            try
            {
                var plats = await _platService.GetPlats();
                return Ok(plats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des plats");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des plats");
            }
        }


        [HttpGet("{platId}")]
        public async Task<ActionResult<PlatDto>> GetPlat(int platId)
        {
            try
            {
                var platById = await _platService.GetPlat(platId);
                if (platById == null)
                {
                    return NotFound();
                }
                return Ok(platById);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération du plat {platId}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération du plat");
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PlatDto>> CreatePlat([FromForm] string nom, [FromForm] string description, [FromForm] decimal? prix, IFormFile image)
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

                    // Optimiser l'image avant de l'enregistrer
                    optimizedImage = _imagesService.OptimizeImage(imageBytes);
                }

                // Créer l'objet DTO
                var platDto = new PlatDto(0, nom, description, optimizedImage, prix);

                var createdPlat = await _platService.CreatePlat(platDto);
                return CreatedAtAction(nameof(GetPlat), new { platId = createdPlat.IdPlat }, createdPlat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du plat");
                return StatusCode(500, "Une erreur est survenue lors de la création du plat");
            }
        }



        [HttpPut("{platId}")]
        public async Task<ActionResult<PlatDto>> UpdatePlat(int platId, [FromBody] PlatDto platDto)
        {
            if (platId != platDto.IdPlat)
            {
                return BadRequest("L'ID ne correspond pas");
            }

            try
            {
                var updatedPlat = await _platService.UpdatePlat(platDto);
                if (updatedPlat == null)
                {
                    return NotFound();
                }
                return Ok(updatedPlat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la mise à jour du plan {platId}");
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du plat");
            }
        }

        [HttpDelete("{platId}")]
        public async Task<IActionResult> DeletePlat(int platId)
        {
            try
            {
                var result = await _platService.DeletePlat(platId);
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
    }
}
