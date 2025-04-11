using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.MappingConfiguration;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;

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
        _paiementRepository = paiementRepository; /
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
