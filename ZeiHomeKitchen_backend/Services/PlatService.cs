﻿using AutoMapper;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;
using Microsoft.Extensions.Logging;

namespace ZeiHomeKitchen_backend.Services
{
    public class PlatService : IPlatService
    {
        private readonly IPlatRepository _platRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatService> _logger;
        private readonly IImagesService _imagesService;

        public PlatService(IPlatRepository platRepository, IMapper mapper, ILogger<PlatService> logger, IImagesService imagesService)
        {
            _platRepository = platRepository;
            _mapper = mapper;
            _logger = logger;
            _imagesService = imagesService;
        }

        /// <summary>
        /// Récupère la liste de tous les plats.
        /// </summary>
        public async Task<IEnumerable<PlatDto>> GetAllPlats()
        {
            var plats = await _platRepository.GetAllPlats();
            return _mapper.Map<IEnumerable<PlatDto>>(plats);
        }

        /// <summary>
        /// Récupère un plat par son ID.
        /// </summary>
        public async Task<PlatDto> GetPlatById(int platId)
        {
            var plat = await _platRepository.GetPlatById(platId);
            return _mapper.Map<PlatDto>(plat);
        }

        /// <summary>
        /// Crée un nouveau plat.
        /// </summary>
        public async Task<PlatDto> CreateNewPlat(PlatDto platDto)
        {
            if (platDto == null) throw new ArgumentNullException(nameof(platDto));

            _logger.LogInformation("Création d'un nouveau plat.");
            var platEntity = _mapper.Map<Plat>(platDto);

            if (platDto.Image != null)
            {
                platEntity.Image = _imagesService.OptimizeImage(platDto.Image, 500, 500, 90);
            }

            var createdPlat = await _platRepository.CreateNewPlat(platEntity);
            return _mapper.Map<PlatDto>(createdPlat);
        }

        /// <summary>
        /// Met à jour un plat existant.
        /// </summary>
        public async Task<PlatDto> UpdateExistingPlat(PlatDto platDto)
        {
            var platEntity = _mapper.Map<Plat>(platDto);

            if (platDto.Image != null)
            {
                platEntity.Image = _imagesService.OptimizeImage(platDto.Image, 500, 500, 90);
            }

            var updatedPlat = await _platRepository.UpdateExistingPlat(platEntity);
            return _mapper.Map<PlatDto>(updatedPlat);
        }

        /// <summary>
        /// Supprime un plat par son ID.
        /// </summary>
        public async Task<bool> DeletePlatById(int platId)
        {
            return await _platRepository.DeletePlatById(platId);
        }

        /// <summary>
        /// Récupère un plat avec ses ingrédients associés.
        /// </summary>
        public async Task<PlatDto> GetPlatDetailsWithIngredients(int platId)
        {
            var plat = await _platRepository.GetPlatDetailsWithIngredients(platId);
            return _mapper.Map<PlatDto>(plat);
        }

        /// <summary>
        /// Associe un ingrédient à un plat.
        /// </summary>
        public async Task<bool> LinkIngredientToPlat(int platId, int ingredientId)
        {
            return await _platRepository.LinkIngredientToPlat(platId, ingredientId);
        }

        /// <summary>
        /// Supprime un ingrédient d'un plat.
        /// </summary>
        public async Task<bool> RemoveIngredientFromPlat(int platId, int ingredientId)
        {
            return await _platRepository.RemoveIngredientFromPlat(platId, ingredientId);
        }

        /// <summary>
        /// Récupère les ingrédients associés à un plat.
        /// </summary>
        public async Task<IEnumerable<IngredientDto>> GetIngredientsByPlat(int platId)
        {
            var ingredients = await _platRepository.GetIngredientsByPlat(platId);
            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }
    }
}
