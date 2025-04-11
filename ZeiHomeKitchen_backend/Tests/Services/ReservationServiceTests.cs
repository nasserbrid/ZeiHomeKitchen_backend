using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;
using ZeiHomeKitchen_backend.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZeiHomeKitchen_backend.MappingConfiguration;

namespace ZeiHomeKitchen_backend.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _mockReservationRepository;
        private readonly Mock<IPlatRepository> _mockPlatRepository;
        private readonly Mock<IPaiementRepository> _mockPaiementRepository; 
        private readonly Mock<ILogger<ReservationService>> _mockLogger;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _mockLogger = new Mock<ILogger<ReservationService>>();
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockPlatRepository = new Mock<IPlatRepository>(); 
            _mockPaiementRepository = new Mock<IPaiementRepository>(); 
            _reservationService = new ReservationService(
                _mockReservationRepository.Object,
                _mockPlatRepository.Object, 
                _mockPaiementRepository.Object, 
                _mockLogger.Object);
        }

        [Fact]
        public async Task TestGetAllReservations()
        {
            // ARRANGE
            var reservations = new List<Reservation>
            {
                new Reservation { IdReservation = 1, DateReservation = new DateTime(2025, 04, 01), Adresse = "123 Rue Exemple", Statut = ReservationStatusDto.EnAttente.ToString(), IdStatistique = 1, IdUtilisateur = 1, NombrePersonnes = 1 },
                new Reservation { IdReservation = 2, DateReservation = new DateTime(2025, 03, 31), Adresse = "456 Avenue Test", Statut = ReservationStatusDto.Annulee.ToString(), IdStatistique = 2, IdUtilisateur = 2, NombrePersonnes = 5 }
            };

            var reservationDtos = reservations.Select(r => r.ToDto()).ToList();

            _mockReservationRepository.Setup(repo => repo.GetAllReservations())
                .ReturnsAsync(reservations);

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
            var reservation = new Reservation { IdReservation = 1, DateReservation = new DateTime(2025, 04, 01), Adresse = "4 Place de l'Abbaye", Statut = ReservationStatusDto.EnAttente.ToString(), IdStatistique = 1, IdUtilisateur = 1, NombrePersonnes = 2 };
            var reservationDto = reservation.ToDto();

            _mockReservationRepository.Setup(repo => repo.GetReservationById(1))
                .ReturnsAsync(reservation);

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
        public async Task TestCreateReservation_WithPlats()
        {
            // ARRANGE
            var reservationEntity = new Reservation
            {
                IdReservation = 3,
                DateReservation = new DateTime(2025, 04, 02),
                Adresse = "19F Rue Roger Martin Du Gard",
                Statut = ReservationStatusDto.Confirmee.ToString(),
                IdStatistique = 3,
                IdUtilisateur = 3,
                NombrePersonnes = 6
            };

            var reservationDto = new ReservationDto(3, new DateTime(2025, 04, 02), "19F Rue Roger Martin Du Gard", ReservationStatusDto.Confirmee, "NomUtilisateur", "PrenomUtilisateur", 6)
            {
                PlatIds = new List<int> { 1, 2 } 
            };

            _mockReservationRepository.Setup(repo => repo.CreateReservation(It.IsAny<Reservation>()))
                .ReturnsAsync(reservationEntity);

           
            var plat = new Plat { IdPlat = 1, Nom = "Plat 1", Prix = 10.0m }; 
            _mockPlatRepository.Setup(repo => repo.GetPlatById(1))
                .ReturnsAsync(plat);

           
            _mockPaiementRepository.Setup(repo => repo.CreatePaiement(It.IsAny<Paiement>()))
                .ReturnsAsync(new Paiement());

            // ACT
            var result = await _reservationService.CreateReservation(reservationDto);

            // ASSERT
            Assert.NotNull(result);
            var okResult = Assert.IsType<ReservationDto>(result);
            Assert.Equal(reservationDto.IdReservation, okResult.IdReservation);
            Assert.Equal(reservationDto.DateReservation, okResult.DateReservation);
            Assert.Equal(reservationDto.Statut, okResult.Statut);
            Assert.Equal(reservationDto.Nom, okResult.Nom);
            Assert.Equal(reservationDto.Prenom, okResult.Prenom);
            Assert.Equal(reservationDto.NombrePersonnes, okResult.NombrePersonnes);
            _mockReservationRepository.Verify(repo => repo.CreateReservation(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public void MappingReservationToReservationDto()
        {
            // Arrange
            var reservation = new Reservation
            {
                IdReservation = 1,
                DateReservation = DateTime.Now,
                Adresse = "35 Rue Chamars",
                Statut = ReservationStatusDto.EnAttente.ToString(),
                IdStatistique = 1,
                IdUtilisateur = 1
            };

            // Act
            var reservationDto = reservation.ToDto();

            // Assert
            Assert.NotNull(reservationDto);
            Assert.Equal(reservation.IdReservation, reservationDto.IdReservation);
            Assert.Equal(reservation.DateReservation, reservationDto.DateReservation);
            Assert.Equal(reservation.Statut, reservationDto.Statut.ToString());
        }
    }
}
