using AutoMapper;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;
using ZeiHomeKitchen_backend.Services;

namespace ZeiHomeKitchen_backend.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _mockReservationRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ReservationService>> _mockLogger;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _mockLogger = new Mock<ILogger<ReservationService>>();
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockMapper = new Mock<IMapper>();
            _reservationService = new ReservationService(_mockReservationRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task TestGetAllReservations()
        {
            // ARRANGE
            var reservations = new List<Reservation>
            {
                new Reservation { IdReservation = 1, DateReservation = new DateTime(2025, 04, 01), Statut = ReservationStatusDto.EnAttente.ToString(), IdUtilisateur = 1 },
                new Reservation { IdReservation = 2, DateReservation = new DateTime(2025, 03, 31), Statut = ReservationStatusDto.Annulee.ToString(), IdUtilisateur = 2 }
            };

            var reservationDtos = new List<ReservationDto>
            {
                new ReservationDto(1, new DateTime(2025, 04, 01), ReservationStatusDto.EnAttente, 1),
                new ReservationDto(2, new DateTime(2025, 03, 31), ReservationStatusDto.Annulee, 2)
            };

            _mockReservationRepository.Setup(repo => repo.GetAllReservations())
                .ReturnsAsync(reservations);

            _mockMapper.Setup(m => m.Map<IEnumerable<ReservationDto>>(It.IsAny<IEnumerable<Reservation>>()))
                .Returns(reservationDtos);

            // ACT
            var result = await _reservationService.GetAllReservations();

            // ASSERT
            var okResult = Assert.IsType<List<ReservationDto>>(result);
            Assert.Equal(2, okResult.Count);
            Assert.Equal(reservationDtos, okResult);
        }

        [Fact]
        public async Task TestGetReservationById()
        {
            // ARRANGE
            var reservation = new Reservation { IdReservation = 1, DateReservation = new DateTime(2025, 04, 01), Statut = ReservationStatusDto.EnAttente.ToString(), IdUtilisateur = 1 };
            var reservationDto = new ReservationDto(1, new DateTime(2025, 04, 01), ReservationStatusDto.EnAttente, 1);

            _mockReservationRepository.Setup(repo => repo.GetReservationById(1))
                .ReturnsAsync(reservation);

            _mockMapper.Setup(m => m.Map<ReservationDto>(It.IsAny<Reservation>()))
                .Returns(reservationDto);

            // ACT
            var result = await _reservationService.GetReservationById(1);

            // ASSERT
            var okResult = Assert.IsType<ReservationDto>(result);
            Assert.Equal(reservationDto, okResult);
        }

        [Fact]
        public async Task TestDeleteReservation()
        {
            // ARRANGE
            int reservationId = 1;
            var reservation = new Reservation { IdReservation = reservationId, DateReservation = new DateTime(2025, 04, 01), Statut = ReservationStatusDto.EnAttente.ToString(), IdUtilisateur = 1 };

            _mockReservationRepository.Setup(repo => repo.GetReservationById(reservationId))
                .ReturnsAsync(reservation);
            _mockReservationRepository.Setup(repo => repo.DeleteReservationById(reservationId))
                .ReturnsAsync(true);

            // ACT
            var result = await _reservationService.DeleteReservationById(reservationId);

            // ASSERT
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockReservationRepository.Verify(repo => repo.DeleteReservationById(reservationId), Times.Once);
        }

        [Fact]
        public async Task TestCreateReservation()
        {
            // ARRANGE
            var reservationEntity = new Reservation { IdReservation = 3, DateReservation = new DateTime(2025, 04, 02), Statut = ReservationStatusDto.Confirmee.ToString(), IdUtilisateur = 1 };
            var reservationDto = new ReservationDto(3, new DateTime(2025, 04, 02), ReservationStatusDto.Confirmee, 1);

            _mockMapper.Setup(m => m.Map<Reservation>(reservationDto)).Returns(reservationEntity);
            _mockReservationRepository.Setup(repo => repo.CreateReservation(It.IsAny<Reservation>()))
                .ReturnsAsync(reservationEntity);
            _mockMapper.Setup(m => m.Map<ReservationDto>(reservationEntity))
                .Returns(reservationDto);

            // ACT
            var result = await _reservationService.CreateReservation(reservationDto);

            // ASSERT
            Assert.NotNull(result);
            var okResult = Assert.IsType<ReservationDto>(result);
            Assert.Equal(reservationDto.IdReservation, okResult.IdReservation);
            Assert.Equal(reservationDto.DateReservation, okResult.DateReservation);
            Assert.Equal(reservationDto.Statut, okResult.Statut);
            Assert.Equal(reservationDto.IdUtilisateur, okResult.IdUtilisateur);
            _mockMapper.Verify(m => m.Map<Reservation>(reservationDto), Times.Once);
            _mockReservationRepository.Verify(repo => repo.CreateReservation(It.IsAny<Reservation>()), Times.Once);
            _mockMapper.Verify(m => m.Map<ReservationDto>(reservationEntity), Times.Once);
        }
    }
}
