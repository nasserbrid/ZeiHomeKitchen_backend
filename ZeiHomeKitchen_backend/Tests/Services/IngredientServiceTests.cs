using System;
using Xunit;
using Moq;
using ZeiHomeKitchen_backend.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Services;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Tests;

public class IngredientServiceTests
{
    private readonly Mock<IIngredientRepository> _mockIngredientRepository;

   // private readonly Mock<IMapper> _mockMapper;

    private readonly Mock<ILogger<IngredientService>> _mockLogger;

    private readonly IngredientService _ingredientService;

    public IngredientServiceTests()
    {
        _mockLogger = new Mock<ILogger<IngredientService>>();
        _mockIngredientRepository = new Mock<IIngredientRepository>();
        // _mockMapper = new Mock<IMapper>();
        //_ingredientService = new IngredientService(_mockIngredientRepository.Object, _mockMapper.Object,_mockLogger.Object);
        _ingredientService = new IngredientService(_mockIngredientRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task TestGetAllIngredients()
    {
        //ARRANGE
        var ingredients = new List<Ingredient>();
        ingredients.Add(new Ingredient() { IdIngredient = 1, Nom = "Oignons" });
        ingredients.Add(new Ingredient() { IdIngredient = 2, Nom = "Tomates" });

        var ingredientDtos = new List<IngredientDto>();
        ingredientDtos.Add(new IngredientDto(1, "Oignons"));
        ingredientDtos.Add(new IngredientDto(2, "Tomates"));


        //Utilisation du mock pour retourner les ingrédients
        _mockIngredientRepository.Setup(repo => repo.GetAllIngredients())
             .ReturnsAsync(ingredients);

        //Utilisation de mock pour mapper les ingrédients en DTOs
        //_mockMapper.Setup(m => m.Map<IEnumerable<IngredientDto>>(It.IsAny<IEnumerable<Ingredient>>()))
               // .Returns(ingredientDtos);


        //ACT
        var result = await _ingredientService.GetAllIngredients();


        //ASSERT

        //Je vérifie que le résultat est de type List<IngredientDto>
        var okResult = Assert.IsType<List<IngredientDto>>(result);

        //Je vérifie le nombre d'éléments dans ma liste
        Assert.Equal(2, okResult.Count);

        //Je vérifie que les résultats sont égaux
        Assert.Equal(ingredientDtos, okResult);
    }

    [Fact]
    public async Task TestGetIngredientById()
    {
        //ARRANGE
        var ingredient = new Ingredient() { IdIngredient = 1, Nom = "Oignons" };

        var ingredientDto = new IngredientDto(1, "Oignons");

        
        _mockIngredientRepository.Setup(repo => repo.GetIngredientById(1))
             .ReturnsAsync(ingredient);

        //Utilisation de mock pour mapper un ingrédient en DTO
       // _mockMapper.Setup(m => m.Map<IngredientDto>(It.IsAny<Ingredient>()))
               // .Returns(ingredientDto);

        //ACT
        var result = await _ingredientService.GetIngredientById(1);


        //ASSERT

        //Je vérifie que le résultat est de type <IngredientDto>
        var okResult = Assert.IsType<IngredientDto>(result);

        //Je vérifie que les résultats sont égaux
        Assert.Equal(ingredientDto, okResult);
    }

    [Fact]
    public async Task TestDeleteIngredient()
    {
        //ARRANGE
        var ingredientId = 1;

        var ingredient = new Ingredient() { IdIngredient = 1, Nom = "Oignons" };

        //Utilisation du mock pour retourner l'ID d'un ingrédient 
        _mockIngredientRepository.Setup(repo => repo.GetIngredientById(ingredientId))
               .ReturnsAsync(ingredient);

        //Utilisation de mock pour supprimer cet ingrédient 
        _mockIngredientRepository.Setup(repo => repo.DeleteIngredient(ingredientId))
               .ReturnsAsync(true);

        //ACT
        var result = await _ingredientService.DeleteIngredient(ingredientId);


        //ASSERT

        //Je vérifie que le résultat est de type <bool>
        var okResult = Assert.IsType<bool>(result);

        //Je vérifie que les résultats sont égaux
        Assert.True(result);

        //Je vérifie que la méthode DeleteIngredient() a été appelée.
        _mockIngredientRepository.Verify(repo => repo.DeleteIngredient(1), Times.Once);

    }

    [Fact]
    public async Task TestCreateIngredient()
    {
        // ARRANGE
        // Je crée une entité Ingredient pour simuler un ingrédient dans la base de données.
        var ingredientEntity = new Ingredient() { IdIngredient = 3, Nom = "Ails" };

        // Je crée un DTO représentant l'ingrédient que l'on souhaite créer.
        var ingredientDto = new IngredientDto(3, "Ails" );

        // Utilisation du mock pour mapper un DTO en entité.
       // _mockMapper.Setup(m => m.Map<Ingredient>(ingredientDto)).Returns(ingredientEntity);

        // Utilisation du mock pour retourner l'entité lors de la création.
        _mockIngredientRepository.Setup(repo => repo.CreateIngredient(It.IsAny<Ingredient>()))
            .ReturnsAsync(ingredientEntity);

        // Utilisation du mock pour mapper l'entité créé en DTO.
        //_mockMapper.Setup(m => m.Map<IngredientDto>(ingredientEntity))
           // .Returns(ingredientDto);

        // ACT
        // J'appelle la méthode CreateIngredient pour tester son bon fonctionnement.
        var result = await _ingredientService.CreateIngredient(ingredientDto);

        // ASSERT
        // Je vérifie que le résultat n'est pas null.
        Assert.NotNull(result);

        // Je vérifie que le résultat est de type <IngredientDto>.
        var okResult = Assert.IsType<IngredientDto>(result);

        // Je vérifie que les valeurs des propriétés du DTO retourné sont correctes.
        Assert.Equal(ingredientDto.IdIngredient, okResult.IdIngredient);
        Assert.Equal(ingredientDto.Nom, okResult.Nom);

        // Je vérifie que les mocks ont bien été appelés une seule fois.
       // _mockMapper.Verify(m => m.Map<Ingredient>(ingredientDto), Times.Once);
       // _mockIngredientRepository.Verify(repo => repo.CreateIngredient(It.IsAny<Ingredient>()), Times.Once);
       // _mockMapper.Verify(m => m.Map<IngredientDto>(ingredientEntity), Times.Once);

        // Vérification des appels au logger avec n'importe quel message (sans méthode d'extension)
        _mockLogger.Verify(logger => logger.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((state, type) => true), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(3));
    }




    [Fact]
    public async Task TestCreateIngredient_InvalidData()
    {
        // ARRANGE
        // Cas de données null
        IngredientDto ingredientDto = null;

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _ingredientService.CreateIngredient(ingredientDto));

        // Je Vérifie que le message de l'exception contient le nom du paramètre
        Assert.Equal("ingredientDto", exception.ParamName);
    }



    [Fact]
    public async Task TestUpdateIngredient()
    {
        // ARRANGE
        var ingredientEntity = new Ingredient() { IdIngredient = 2, Nom = "Poivrons" };
        var ingredientDto = new IngredientDto(2,"Poivrons");

        // Utilisation du mock pour mapper un DTO en entité
       // _mockMapper.Setup(m => m.Map<Ingredient>(ingredientDto)).Returns(ingredientEntity);

        // Utilisation du mock pour retourner l'entité lors de la création
        _mockIngredientRepository.Setup(repo => repo.UpdateIngredient(It.IsAny<Ingredient>()))
            .ReturnsAsync(ingredientEntity);

        // Utilisation de mock pour mapper l'entité en DTO
       // _mockMapper.Setup(m => m.Map<IngredientDto>(ingredientEntity))
           // .Returns(ingredientDto);

        // ACT
        var result = await _ingredientService.UpdateIngredient(ingredientDto);

        // ASSERT
        // Je vérifie que le résultat est de type <IngredientDto>
        var okResult = Assert.IsType<IngredientDto>(result);

        // Je vérifie que les résultats sont égaux
        Assert.Equal(ingredientDto, okResult);
    }


}
