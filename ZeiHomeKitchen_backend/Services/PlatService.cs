using AutoMapper;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;

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
        public async Task<PlatDto> CreatePlat(PlatDto platDto)
        {
            if (platDto == null)
            {
                throw new ArgumentNullException(nameof(platDto));
            }
            _logger.LogInformation("Mapping de PlatDto vers Plat.");
            var platEntity = _mapper.Map<Plat>(platDto);

            // Optimiser l'image si elle est fournie
            if (platDto.Image != null)
            {
                var optimizedImageBytes = _imagesService.OptimizeImage(platDto.Image, 500, 500, 75);

                // Assigner l'image optimisée
                platEntity.Image = optimizedImageBytes; 
            }

            _logger.LogInformation("Enregistrement de l'entité en base de données.");
            var createPlat = await _platRepository.CreatePlat(platEntity);

            _logger.LogInformation("Mapping de l'entité créé vers PlatDto.");
            return _mapper.Map<PlatDto>(createPlat);

        }

        public async Task<bool> DeletePlat(int PlatId)
        {
            return await _platRepository.DeletePlat(PlatId);
        }

        public async Task<PlatDto> GetPlat(int PlatId)
        {
            var platById = await _platRepository.GetPlat(PlatId);
            return _mapper.Map<PlatDto>(platById);
        }

        public async Task<IEnumerable<PlatDto>> GetPlats()
        {
            var plats = await _platRepository.GetPlats();
            return _mapper.Map<IEnumerable<PlatDto>>(plats);
        }

        public async Task<PlatDto> UpdatePlat(PlatDto platDto)
        {
            var platEntity = _mapper.Map<Plat>(platDto);

            // Optimiser l'image si elle est fournie
            if (platDto.Image != null)
            {
                var optimizedImageBytes = _imagesService.OptimizeImage(platDto.Image, 500, 500, 75);

                // Assigner l'image optimisée
                platEntity.Image = optimizedImageBytes;
            }

            var updatePlat = await _platRepository.UpdatePlat(platEntity);

            return _mapper.Map<PlatDto>(updatePlat);
        }
    }
}
