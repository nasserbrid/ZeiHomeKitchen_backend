using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetUtilisateurById(int id);
    Task<Utilisateur?> GetUtilisateurByUsername(string username);
    Task<Utilisateur?> GetUtilisateurByEmail(string email);
}
