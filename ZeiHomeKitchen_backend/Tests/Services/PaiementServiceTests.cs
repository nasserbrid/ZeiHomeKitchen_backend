using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;
using ZeiHomeKitchen_backend.Domain.Models;
using ZeiHomeKitchen_backend.Domain.Services;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;

namespace ZeiHomeKitchen_backend.Tests.Services
{
    public class PaiementServiceTests
    {
        private readonly Mock<IPaiementRepository> _mockPaiementRepository;
        private readonly Mock<ILogger<PaiementService>> _mockLogger;
        private readonly PaiementService _paiementService;

        public PaiementServiceTests()
        {
            _mockLogger = new Mock<ILogger<PaiementService>>();
            _mockPaiementRepository = new Mock<IPaiementRepository>();
            _paiementService = new PaiementService(_mockPaiementRepository.Object);
        }

        [Fact]
        public async Task TestGetAllPaiements()
        {
            // ARRANGE
            var paiements = new List<Paiement>
            {
                new Paiement { IdPaiement = 1, Montant = 100, Statut = PaiementStatusDto.EnAttente.ToString(), Moyen = PaiementMoyenDto.CB.ToString(), IdReservation = 1 },
                new Paiement { IdPaiement = 2, Montant = 200, Statut = PaiementStatusDto.Valide.ToString(), Moyen = PaiementMoyenDto.PayPal.ToString(), IdReservation = 2 }
            };

            var paiementDtos = paiements.Select(p => p.ToDto()).ToList();

            _mockPaiementRepository.Setup(repo => repo.GetAllPaiements())
                .ReturnsAsync(paiements);

            // ACT
            var result = await _paiementService.GetAllPaiements();

            // ASSERT
            var okResult = Assert.IsType<List<PaiementDto>>(result);
            Assert.Equal(2, okResult.Count);
            Assert.Equal(paiementDtos, okResult);
        }

        [Fact]
        public async Task TestGetPaiementById()
        {
            // ARRANGE
            var paiement = new Paiement { IdPaiement = 1, Montant = 100, Statut = PaiementStatusDto.EnAttente.ToString(), Moyen = PaiementMoyenDto.CB.ToString(), IdReservation = 1 };
            var paiementDto = paiement.ToDto();

            _mockPaiementRepository.Setup(repo => repo.GetPaiementById(1))
                .ReturnsAsync(paiement);

            // ACT
            var result = await _paiementService.GetPaiementById(1);

            // ASSERT
            var okResult = Assert.IsType<PaiementDto>(result);
            Assert.Equal(paiementDto, okResult);
        }

        [Fact]
        public async Task TestCreatePaiement()
        {
            // ARRANGE
            var paiementEntity = new Paiement { IdPaiement = 3, Montant = 300, Statut = PaiementStatusDto.EnAttente.ToString(), Moyen = PaiementMoyenDto.PayPal.ToString(), IdReservation = 3 };
            var paiementDto = new PaiementDto(3, 300, PaiementStatusDto.EnAttente, PaiementMoyenDto.PayPal, 3);

            _mockPaiementRepository.Setup(repo => repo.CreatePaiement(It.IsAny<Paiement>()))
                .ReturnsAsync(paiementEntity);

            // ACT
            var result = await _paiementService.CreatePaiement(paiementDto);

            // ASSERT
            Assert.NotNull(result);
            var okResult = Assert.IsType<PaiementDto>(result);
            Assert.Equal(paiementDto.IdPaiement, okResult.IdPaiement);
            Assert.Equal(paiementDto.Montant, okResult.Montant);
            Assert.Equal(paiementDto.Statut, okResult.Statut);
            Assert.Equal(paiementDto.Moyen, okResult.Moyen);
            Assert.Equal(paiementDto.IdReservation, okResult.IdReservation);
            _mockPaiementRepository.Verify(repo => repo.CreatePaiement(It.IsAny<Paiement>()), Times.Once);
        }

        [Fact]
        public async Task TestDeletePaiement()
        {
            // ARRANGE
            int paiementId = 1;
            var paiement = new Paiement { IdPaiement = paiementId, Montant = 100, Statut = PaiementStatusDto.EnAttente.ToString(), Moyen = PaiementMoyenDto.CB.ToString(), IdReservation = 1 };

            _mockPaiementRepository.Setup(repo => repo.GetPaiementById(paiementId))
                .ReturnsAsync(paiement);
            _mockPaiementRepository.Setup(repo => repo.DeletePaiement(paiementId))
                .ReturnsAsync(true);

            // ACT
            var result = await _paiementService.DeletePaiement(paiementId);

            // ASSERT
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockPaiementRepository.Verify(repo => repo.DeletePaiement(paiementId), Times.Once);
        }
    }
}
