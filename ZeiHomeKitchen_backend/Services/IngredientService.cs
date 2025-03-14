using System;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.MappingConfiguration;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;

namespace ZeiHomeKitchen_backend.Services;

public class IngredientService : IIngredientService
{
    //injection de IIngredientRepository et IMapper pour les utiliser et en lecture seule.
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;

    private readonly ILogger<IngredientService> _logger;

    //J'ajoute dans mon constructeur la classe mapper qui mappe ingredientDto en Ingredient et vice versa
    //J'ajoute également l'interface IIngredientRepository par lequel on passe pour implémenter les services
    public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper, ILogger<IngredientService> logger)
    {
        _ingredientRepository = ingredientRepository;

        _mapper = mapper ;

        _logger = logger;
    }



    public async Task<IngredientDto> CreateIngredient(IngredientDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            throw new ArgumentNullException(nameof(ingredientDto));
        }
        
        _logger.LogInformation("Mapping de IngredientDto vers Ingredient.");
        var ingredientEntity = _mapper.Map<Ingredient>(ingredientDto);

        _logger.LogInformation("Enregistrement de l'entité en base de données.");
        var createIngredient = await _ingredientRepository.CreateIngredient(ingredientEntity);

        _logger.LogInformation("Mapping de l'entité créé vers IngredientDto.");
        return _mapper.Map<IngredientDto>(createIngredient);
    }

    public async Task<bool> DeleteIngredient(int IngredientId)
    {
        return await _ingredientRepository.DeleteIngredient(IngredientId);  
    }

    public async Task<IngredientDto> GetIngredient(int IngredientId)
    {
        var ingredientById = await _ingredientRepository.GetIngredient(IngredientId);
        return _mapper.Map<IngredientDto>(ingredientById);
    }

    public async Task<IEnumerable<IngredientDto>> GetIngredients()
    {
        var ingredients = await _ingredientRepository.GetIngredients();
        return _mapper.Map<IEnumerable<IngredientDto>>(ingredients); 
    }

    public async Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto)
    {
        var ingredientEntity = _mapper.Map<Ingredient>(ingredientDto);

        var updatedIngredient = await _ingredientRepository.UpdateIngredient(ingredientEntity);

        return _mapper.Map<IngredientDto>(updatedIngredient);
    }
}
