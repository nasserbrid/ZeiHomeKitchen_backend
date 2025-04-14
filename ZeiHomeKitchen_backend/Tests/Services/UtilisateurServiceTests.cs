using System.Threading.Tasks;
using Moq;
using Xunit;
using ZeiHomeKitchen_backend.Domain.Models;
using ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration;
using ZeiHomeKitchen_backend.Domain.Services;
using ZeiHomeKitchen_backend.Application.Ports;

namespace ZeiHomeKitchen_backend.Tests.Services;

public class UtilisateurServiceTests
{
    private readonly Mock<IUtilisateurRepository> _utilisateurRepositoryMock;
    private readonly UtilisateurService _utilisateurService;

    public UtilisateurServiceTests()
    {
        _utilisateurRepositoryMock = new Mock<IUtilisateurRepository>();
        _utilisateurService = new UtilisateurService(_utilisateurRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUtilisateurById_ShouldReturnUtilisateurDto_WhenUtilisateurExists()
    {
        // Arrange
        var utilisateur = new Utilisateur { Id = 1,Nom = "John", Prenom = "Doe", Email = "john@example.com"};
        _utilisateurRepositoryMock.Setup(repo => repo.GetUtilisateurById(1)).ReturnsAsync(utilisateur);

        // Act
        var result = await _utilisateurService.GetUtilisateurById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John", result.Nom);
        Assert.Equal("Doe", result.Prenom);
        Assert.Equal("john@example.com", result.Email);
        //Assert.Equal("User", result.Role);
    }
}
