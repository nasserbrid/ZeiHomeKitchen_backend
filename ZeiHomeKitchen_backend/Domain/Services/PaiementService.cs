using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;

namespace ZeiHomeKitchen_backend.Domain.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly IPaiementRepository _paiementRepository;

        public PaiementService(IPaiementRepository paiementRepository)
        {
            _paiementRepository = paiementRepository;
        }

        public async Task<PaiementDto> CreatePaiement(PaiementDto paiementDto)
        {
            var paiement = paiementDto.ToModel();
            var created = await _paiementRepository.CreatePaiement(paiement);
            return created.ToDto();
        }

        public async Task<IEnumerable<PaiementDto>> GetAllPaiements()
        {
            var paiements = await _paiementRepository.GetAllPaiements();
            return paiements.Select(p => p.ToDto());
        }

        public async Task<PaiementDto> GetPaiementById(int idPaiement)
        {
            var paiement = await _paiementRepository.GetPaiementById(idPaiement);
            return paiement?.ToDto();
        }

        public async Task<PaiementDto> GetPaiementByReservationId(int reservationId)
        {
            var paiement = await _paiementRepository.GetPaiementByReservationId(reservationId);
            return paiement?.ToDto();
        }

        public async Task<PaiementDto> UpdatePaiement(PaiementDto paiementDto)
        {
            var paiement = paiementDto.ToModel();
            var updated = await _paiementRepository.UpdatePaiement(paiement);
            return updated.ToDto();
        }

        public async Task<bool> DeletePaiement(int idPaiement)
        {
            return await _paiementRepository.DeletePaiement(idPaiement);
        }
    }
}
