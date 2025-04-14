using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports;

public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetUtilisateurById(int id);
    Task<Utilisateur?> GetUtilisateurByUsername(string username);
    Task<Utilisateur?> GetUtilisateurByEmail(string email);
}
