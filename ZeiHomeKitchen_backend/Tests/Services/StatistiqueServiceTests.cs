//using AutoMapper;
//using Moq;
//using Xunit;
//using Microsoft.Extensions.Logging;
//using ZeiHomeKitchen_backend.Dtos;
//using ZeiHomeKitchen_backend.Models;
//using ZeiHomeKitchen_backend.Repositories;
//using ZeiHomeKitchen_backend.Services;

//namespace ZeiHomeKitchen_backend.Tests.Services
//{
//    public class StatistiqueServiceTests
//    {
//        private readonly Mock<IStatistiqueRepository> _mockStatistiqueRepository;
//        private readonly Mock<IMapper> _mockMapper;
//        private readonly Mock<ILogger<StatistiqueService>> _mockLogger;
//        private readonly StatistiqueService _statistiqueService;

//        public StatistiqueServiceTests()
//        {
//            _mockLogger = new Mock<ILogger<StatistiqueService>>();
//            _mockStatistiqueRepository = new Mock<IStatistiqueRepository>();
//            _mockMapper = new Mock<IMapper>();
//            _statistiqueService = new StatistiqueService(_mockStatistiqueRepository.Object, _mockMapper.Object, _mockLogger.Object);
//        }

//        [Fact]
//        public async Task TestGetAllStatistiques()
//        {
//            // ARRANGE
//            var statistiques = new List<Statistique>
//            {
//                new Statistique { IdStatistique = 1, DateStatistique = DateOnly.FromDateTime(new DateTime(2025, 04, 01)), TotalReservation = 10 },
//                new Statistique { IdStatistique = 2, DateStatistique = DateOnly.FromDateTime(new DateTime(2025, 03, 31)), TotalReservation = 5 }
//            };

//            var statistiqueDtos = new List<StatistiqueDto>
//            {
//                new StatistiqueDto(1, DateOnly.FromDateTime(new DateTime(2025, 04, 01)), 10, null),
//                new StatistiqueDto(2, DateOnly.FromDateTime(new DateTime(2025, 03, 31)), 5, null)
//            };

//            _mockStatistiqueRepository.Setup(repo => repo.GetAllStatistiques())
//                .ReturnsAsync(statistiques);

//            _mockMapper.Setup(m => m.Map<IEnumerable<StatistiqueDto>>(It.IsAny<IEnumerable<Statistique>>()))
//                .Returns(statistiqueDtos);

//            // ACT
//            var result = await _statistiqueService.GetAllStatistiques();

//            // ASSERT
//            var okResult = Assert.IsType<List<StatistiqueDto>>(result);
//            Assert.Equal(2, okResult.Count);
//            Assert.Equal(statistiqueDtos, okResult);
//        }

//        [Fact]
//        public async Task TestGetStatistiqueById()
//        {
//            // ARRANGE
//            var statistique = new Statistique { IdStatistique = 1, DateStatistique = DateOnly.FromDateTime(new DateTime(2025, 04, 01)), TotalReservation = 10 };
//            var statistiqueDto = new StatistiqueDto(1, DateOnly.FromDateTime(new DateTime(2025, 04, 01)), 10, null);

//            _mockStatistiqueRepository.Setup(repo => repo.GetStatistiqueById(1))
//                .ReturnsAsync(statistique);

//            _mockMapper.Setup(m => m.Map<StatistiqueDto>(It.IsAny<Statistique>()))
//                .Returns(statistiqueDto);

//            // ACT
//            var result = await _statistiqueService.GetStatistiqueById(1);

//            // ASSERT
//            var okResult = Assert.IsType<StatistiqueDto>(result);
//            Assert.Equal(statistiqueDto, okResult);
//        }

//        [Fact]
//        public async Task TestCreateStatistique()
//        {
//            // ARRANGE
//            var statistiqueEntity = new Statistique { IdStatistique = 3, DateStatistique = DateOnly.FromDateTime(new DateTime(2025, 04, 02)), TotalReservation = 15 };
//            var statistiqueDto = new StatistiqueDto(3, DateOnly.FromDateTime(new DateTime(2025, 04, 02)), 15, null);

//            _mockMapper.Setup(m => m.Map<Statistique>(statistiqueDto)).Returns(statistiqueEntity);
//            _mockStatistiqueRepository.Setup(repo => repo.CreateStatistique(It.IsAny<Statistique>()))
//                .ReturnsAsync(statistiqueEntity);
//            _mockMapper.Setup(m => m.Map<StatistiqueDto>(statistiqueEntity))
//                .Returns(statistiqueDto);

//            // ACT
//            var result = await _statistiqueService.CreateStatistique(statistiqueDto);

//            // ASSERT
//            Assert.NotNull(result);
//            var okResult = Assert.IsType<StatistiqueDto>(result);
//            Assert.Equal(statistiqueDto.IdStatistique, okResult.IdStatistique);
//            Assert.Equal(statistiqueDto.DateStatistique, okResult.DateStatistique);
//            Assert.Equal(statistiqueDto.TotalReservation, okResult.TotalReservation);
//        }

//        [Fact]
//        public async Task TestDeleteStatistique()
//        {
//            // ARRANGE
//            int statistiqueId = 1;
//            _mockStatistiqueRepository.Setup(repo => repo.DeleteStatistiqueById(statistiqueId))
//                .ReturnsAsync(true);

//            // ACT
//            var result = await _statistiqueService.DeleteStatistiqueById(statistiqueId);

//            // ASSERT
//            Assert.IsType<bool>(result);
//            Assert.True(result);
//            _mockStatistiqueRepository.Verify(repo => repo.DeleteStatistiqueById(statistiqueId), Times.Once);
//        }

//        [Fact]
//        public async Task TestGetStatistiqueByDate()
//        {
//            // ARRANGE
//            var date = DateOnly.FromDateTime(new DateTime(2025, 04, 01));
//            var statistique = new Statistique { IdStatistique = 1, DateStatistique = date, TotalReservation = 10 };
//            var statistiqueDto = new StatistiqueDto(1, date, 10, null);

//            _mockStatistiqueRepository.Setup(repo => repo.GetStatistiqueByDate(date))
//                .ReturnsAsync(statistique);

//            _mockMapper.Setup(m => m.Map<StatistiqueDto>(It.IsAny<Statistique>()))
//                .Returns(statistiqueDto);

//            // ACT
//            var result = await _statistiqueService.GetStatistiqueByDate(date);

//            // ASSERT
//            var okResult = Assert.IsType<StatistiqueDto>(result);
//            Assert.Equal(statistiqueDto, okResult);
//        }

//        [Fact]
//        public async Task TestGetStatistiquesForPeriod()
//        {
//            // ARRANGE
//            var startDate = DateOnly.FromDateTime(new DateTime(2025, 04, 01));
//            var endDate = DateOnly.FromDateTime(new DateTime(2025, 04, 03));
//            var statistiques = new List<Statistique>
//            {
//                new Statistique { IdStatistique = 1, DateStatistique = startDate, TotalReservation = 10 },
//                new Statistique { IdStatistique = 2, DateStatistique = endDate, TotalReservation = 5 }
//            };

//            var statistiqueDtos = new List<StatistiqueDto>
//            {
//                new StatistiqueDto(1, startDate, 10, null),
//                new StatistiqueDto(2, endDate, 5, null)
//            };

//            _mockStatistiqueRepository.Setup(repo => repo.GetStatistiquesForPeriod(startDate, endDate))
//                .ReturnsAsync(statistiques);

//            _mockMapper.Setup(m => m.Map<IEnumerable<StatistiqueDto>>(It.IsAny<IEnumerable<Statistique>>()))
//                .Returns(statistiqueDtos);

//            // ACT
//            var result = await _statistiqueService.GetStatistiquesForPeriod(startDate, endDate);

//            // ASSERT
//            var okResult = Assert.IsType<List<StatistiqueDto>>(result);
//            Assert.Equal(2, okResult.Count);
//            Assert.Equal(statistiqueDtos, okResult);
//        }
//    }
//}
