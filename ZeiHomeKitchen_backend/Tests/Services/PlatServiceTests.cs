using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ZeiHomeKitchen_backend.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Services;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Tests.Services
{
    public class PlatServiceTests
    {
        private readonly Mock<IPlatRepository> _mockPlatRepository;
        //private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PlatService>> _mockLogger;
        private readonly PlatService _platService;
        private readonly Mock<IImagesService> _mockImagesService;

        public PlatServiceTests()
        {
            _mockLogger = new Mock<ILogger<PlatService>>();
            _mockPlatRepository = new Mock<IPlatRepository>();
           // _mockMapper = new Mock<IMapper>();
            _mockImagesService = new Mock<IImagesService>();
            _platService = new PlatService(_mockPlatRepository.Object, /*_mockMapper.Object,*/ _mockLogger.Object, _mockImagesService.Object);
        }

        [Fact]
        public async Task TestGetAllPlats()
        {
            // ARRANGE
            var plats = new List<Plat>
            {
                new Plat() { IdPlat = 1, Nom = "Omelette Djiboutienne", Description = "Description1", Image = new byte[] { 1, 2, 3 }, Prix = 20 },
                new Plat() { IdPlat = 2, Nom = "Riz à l'agneau", Description = "Description2", Image = new byte[] { 4, 5, 6 }, Prix = 30 }
            };

            var platDtos = new List<PlatDto>
            {
                new PlatDto(1, "Omelette Djiboutienne", "Description1", new byte[] { 1, 2, 3 }, 20),
                new PlatDto(2, "Riz à l'agneau", "Description2", new byte[] { 4, 5, 6 }, 30)
           };

            _mockPlatRepository.Setup(repo => repo.GetAllPlats()).ReturnsAsync(plats);
            //_mockMapper.Setup(m => m.Map<IEnumerable<PlatDto>>(It.IsAny<IEnumerable<Plat>>())).Returns(platDtos);

            // ACT
            var result = await _platService.GetAllPlats();

            // ASSERT
            var okResult = Assert.IsType<List<PlatDto>>(result);
            Assert.Equal(2, okResult.Count);
            Assert.Equal(platDtos, okResult);
        }


        [Fact]
        public async Task TestGetPlatById()
        {
            //ARRANGE
            
            var plat = new Plat() { IdPlat = 1, Nom = "Omelette Djiboutienne", Description = "Description1", Image = new byte[] { 1, 2, 3 }, Prix = 20 };

            var platDto = new PlatDto(1, "Omelette Djiboutienne", "Description1", new byte[] { 1, 2, 3 }, 20);

            //Utilisation du mock pour retourner un plat
            _mockPlatRepository.Setup(repo => repo.GetPlatById(1))
                 .ReturnsAsync(plat);

            //Utilisation de mock pour mapper un plat en DTO
           // _mockMapper.Setup(m => m.Map<PlatDto>(It.IsAny<Plat>()))
                    //.Returns(platDto);

            //ACT
            var result = await _platService.GetPlatById(1);


            //ASSERT

            //Je vérifie que le résultat est de type <PlatDto>
            var okResult = Assert.IsType<PlatDto>(result);

            //Je vérifie que les résultats sont égaux
            Assert.Equal(platDto, okResult);
        }

        [Fact]
        public async Task TestDeletePlatById()
        {
            //ARRANGE
            var platId = 1;

            var plat = new Plat() { IdPlat = 1, Nom = "Omelette Djiboutienne", Description = "Description1", Image = new byte[] { 1, 2, 3 }, Prix = 20 };

            
            _mockPlatRepository.Setup(repo => repo.GetPlatById(platId))
                   .ReturnsAsync(plat);

            
            _mockPlatRepository.Setup(repo => repo.DeletePlatById(platId))
                   .ReturnsAsync(true);

            //ACT
            var result = await _platService.DeletePlatById(platId);


            //ASSERT

            //Je vérifie que le résultat est de type <bool>
            var okResult = Assert.IsType<bool>(result);

            //Je vérifie que les résultats sont égaux
            Assert.True(result);

            //Je vérifie que la méthode DeletePlat() a été appelée.
            _mockPlatRepository.Verify(repo => repo.DeletePlatById(1), Times.Once);
        }

        [Fact]
        public async Task TestCreateNewPlat()
        {
            // ARRANGE
           
            var platEntity = new Plat() { IdPlat = 3, Nom = "Steak aux frites", Description = "Description3", Image = new byte[] { 7, 8, 9 }, Prix = 15 };

           
            var platDto = new PlatDto(3, "Steak aux frites", "Description3", new byte[] { 7, 8, 9 }, 15);

            // Utilisation du mock pour mapper un DTO en entité.
            //_mockMapper.Setup(m => m.Map<Plat>(platDto)).Returns(platEntity);

            // Utilisation du mock pour retourner l'entité lors de la création.
            _mockPlatRepository.Setup(repo => repo.CreateNewPlat(It.IsAny<Plat>()))
                .ReturnsAsync(platEntity);

            // Utilisation du mock pour mapper l'entité créé en DTO.
            //_mockMapper.Setup(m => m.Map<PlatDto>(platEntity))
            //    .Returns(platDto);

            // ACT
          
            var result = await _platService.CreateNewPlat(platDto);

            // ASSERT
           
            Assert.NotNull(result);

           
            var okResult = Assert.IsType<PlatDto>(result);

            
            Assert.Equal(platDto.IdPlat, okResult.IdPlat);
            Assert.Equal(platDto.Nom, okResult.Nom);

            // Je vérifie que les mocks ont bien été appelés une seule fois.
            //_mockMapper.Verify(m => m.Map<Plat>(platDto), Times.Once);
            _mockPlatRepository.Verify(repo => repo.CreateNewPlat(It.IsAny<Plat>()), Times.Once);
           // _mockMapper.Verify(m => m.Map<PlatDto>(platEntity), Times.Once);

            // Vérification des appels au logger avec n'importe quel message (sans méthode d'extension)
            _mockLogger.Verify(logger => logger.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((state, type) => true), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(3));
        }

        [Fact]
        public async Task TestCreatePlat_InvalidData()
        {
            // ARRANGE
            // Cas de données null
            PlatDto platDto = null;

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _platService.CreateNewPlat(platDto));

            // Je Vérifie que le message de l'exception contient le nom du paramètre
            Assert.Equal("platDto", exception.ParamName);
        }

        [Fact]
        public async Task TestUpdateExistingPlat()
        {
            // ARRANGE
            var platEntity = new Plat() { IdPlat = 2, Nom = "Riz à l'agneau", Description = "Description2", Image = new byte[] { 4, 5, 6 }, Prix = 30 };
            var platDto = new PlatDto(2, "Riz à l'agneau", "Description2", new byte[] { 4, 5, 6 }, 30);

            // Utilisation du mock pour mapper un DTO en entité
            //_mockMapper.Setup(m => m.Map<Plat>(platDto)).Returns(platEntity);

            // Utilisation du mock pour retourner l'entité lors de la création
            _mockPlatRepository.Setup(repo => repo.UpdateExistingPlat(It.IsAny<Plat>()))
                .ReturnsAsync(platEntity);

            // Utilisation de mock pour mapper l'entité en DTO
            //_mockMapper.Setup(m => m.Map<PlatDto>(platEntity))
            //    .Returns(platDto);

            // ACT
            var result = await _platService.UpdateExistingPlat(platDto);

            // ASSERT
            // Je vérifie que le résultat est de type <IngredientDto>
            var okResult = Assert.IsType<PlatDto>(result);

            // Je vérifie que les résultats sont égaux
            Assert.Equal(platDto, okResult);
        }

    }
}
