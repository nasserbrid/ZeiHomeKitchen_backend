using System;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;
using ZeiHomeKitchen_backend.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;

namespace ZeiHomeKitchen_backend.Domain.Services;

public class IngredientService : IIngredientService

{    //injection de IIngredientRepository et IMapper pour les utiliser et en lecture seule SOLID (D)

    private readonly IIngredientRepository _ingredientRepository;
    //private readonly IMapper _mapper;
    private readonly ILogger<IngredientService> _logger;

    /// <summary>
    /// Constructeur pour le service IngredientService.
    /// </summary>
    /// <param name="ingredientRepository">L'interface du repository d'ingrédients.</param>
    /// <param name="mapper">L'instance d'AutoMapper pour le mapping entre les entités et les DTOs.</param>
    /// <param name="logger">L'instance du logger pour les journaux d'activité.</param>
    public IngredientService(IIngredientRepository ingredientRepository, /*IMapper mapper*/ ILogger<IngredientService> logger)
    {
        _ingredientRepository = ingredientRepository;
        //_mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Crée un nouvel ingrédient à partir d'un DTO d'ingrédient.
    /// </summary>
    /// <param name="ingredientDto">Le DTO d'ingrédient à créer.</param>
    /// <returns>Le DTO d'ingrédient créé.</returns>
    /// <exception cref="ArgumentNullException">Lève une exception si le DTO est nul.</exception>
    public async Task<IngredientDto> CreateIngredient(IngredientDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            throw new ArgumentNullException(nameof(ingredientDto));
        }

        _logger.LogInformation("Mapping de IngredientDto vers Ingredient.");
        //var ingredientEntity = _mapper.Map<Ingredient>(ingredientDto);
        var ingredientEntity = ingredientDto.ToModel();

        _logger.LogInformation("Enregistrement de l'entité en base de données.");
        var createIngredient = await _ingredientRepository.CreateIngredient(ingredientEntity);

        _logger.LogInformation("Mapping de l'entité créée vers IngredientDto.");
        // return _mapper.Map<IngredientDto>(createIngredient);
        return createIngredient.ToDto();
    }

    /// <summary>
    /// Supprime un ingrédient par son ID.
    /// </summary>
    /// <param name="ingredientId">L'ID de l'ingrédient à supprimer.</param>
    /// <returns>Un booléen indiquant si la suppression a réussi.</returns>
    public async Task<bool> DeleteIngredient(int ingredientId)
    {
        return await _ingredientRepository.DeleteIngredient(ingredientId);
    }

    /// <summary>
    /// Récupère un ingrédient par son ID.
    /// </summary>
    /// <param name="ingredientId">L'ID de l'ingrédient à récupérer.</param>
    /// <returns>Le DTO de l'ingrédient correspondant.</returns>
    public async Task<IngredientDto> GetIngredientById(int ingredientId)
    {
        var ingredientById = await _ingredientRepository.GetIngredientById(ingredientId);
        //return _mapper.Map<IngredientDto>(ingredientById);
        return ingredientById?.ToDto();
    }

    /// <summary>
    /// Récupère les plats associés à un ingrédient par son ID.
    /// </summary>
    /// <param name="ingredientId">L'ID de l'ingrédient pour lequel récupérer les plats.</param>
    /// <returns>Une collection de DTOs de plats associés.</returns>
    public async Task<IEnumerable<PlatDto>> GetPlatsByIngredientId(int ingredientId)
    {
        var plats = await _ingredientRepository.GetPlatsByIngredientId(ingredientId);
        //return _mapper.Map<IEnumerable<PlatDto>>(plats);
        return plats.Select(p => p.ToDto());

    }

    /// <summary>
    /// Récupère tous les ingrédients.
    /// </summary>
    /// <returns>Une collection de DTOs d'ingrédients.</returns>
    public async Task<IEnumerable<IngredientDto>> GetAllIngredients()
    {
        var ingredients = await _ingredientRepository.GetAllIngredients();
        //return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        return ingredients.Select(i => i.ToDto());
    }

    /// <summary>
    /// Récupère un ingrédient avec ses plats associés par son ID.
    /// </summary>
    /// <param name="ingredientId">L'ID de l'ingrédient à récupérer avec ses plats.</param>
    /// <returns>Le DTO de l'ingrédient avec ses plats associés.</returns>
    public async Task<IngredientDto> GetIngredientWithPlats(int ingredientId)
    {
        var ingredient = await _ingredientRepository.GetIngredientWithPlats(ingredientId);
        _logger.LogInformation($"Récupéré : {JsonConvert.SerializeObject(ingredient)}");
        //return _mapper.Map<IngredientDto>(ingredient);
        return ingredient?.ToDto();
    }
    //public async Task<IngredientDto> GetIngredientWithPlats(int ingredientId)
    //{
    //    // Log de l'ID d'ingrédient demandé
    //    _logger.LogInformation($"Demande de récupération de l'ingrédient avec ID : {ingredientId}");

    //    var ingredient = await _ingredientRepository.GetIngredientWithPlats(ingredientId);

    //    // Log de l'objet Ingredient récupéré
    //    if (ingredient == null)
    //    {
    //        _logger.LogWarning($"Aucun ingrédient trouvé avec l'ID : {ingredientId}");
    //        return null; // ou gérer selon tes besoins
    //    }

    //    _logger.LogInformation($"Récupéré : {JsonConvert.SerializeObject(ingredient)}");

    //    // Log avant le mapping
    //    _logger.LogInformation("Mapping de l'ingrédient vers IngredientDto");
    //    var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

    //    // Log du DTO résultant
    //    _logger.LogInformation($"Mapped IngredientDto : {JsonConvert.SerializeObject(ingredientDto)}");

    //    return ingredientDto;
    //}

    /// <summary>
    /// Met à jour un ingrédient existant à partir d'un DTO d'ingrédient.
    /// </summary>
    /// <param name="ingredientDto">Le DTO d'ingrédient avec les nouvelles données.</param>
    /// <returns>Le DTO de l'ingrédient mis à jour.</returns>
    public async Task<IngredientDto> UpdateIngredient(IngredientDto ingredientDto)
    {
        if (ingredientDto == null)
        {
            throw new ArgumentNullException(nameof(ingredientDto));
        }
        //var ingredientEntity = _mapper.Map<Ingredient>(ingredientDto);
        var ingredientEntity = ingredientDto.ToModel();

        var updatedIngredient = await _ingredientRepository.UpdateIngredient(ingredientEntity);

        //return _mapper.Map<IngredientDto>(updatedIngredient);
        return updatedIngredient.ToDto();
    }
}
