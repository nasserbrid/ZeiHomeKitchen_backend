using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;
using ZeiHomeKitchen_backend.Domain.Models;

public class CreateReservationService : ICreateReservationService
{
    private readonly ICreateReservationRepository _repository;
    private readonly IPaiementRepository _paiementRepository; 
    private readonly ILogger<CreateReservationService> _logger;

    public CreateReservationService(
        ICreateReservationRepository repository,
        IPaiementRepository paiementRepository,
        ILogger<CreateReservationService> logger)
    {
        _repository = repository;
        _paiementRepository = paiementRepository; 
        _logger = logger;
    }

    public async Task<ReservationDto> CreateReservation(CreateReservationDto reservationDto)
    {
        int userId = 3; 
    
        var reservation = reservationDto.ToModel(userId);

        var createdReservation = await _repository.CreateReservation(reservation);

        return createdReservation.ToDto(); 
    }
}
