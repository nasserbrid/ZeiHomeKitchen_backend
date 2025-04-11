using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Services;

public interface IUtilisateurService
{
    Task<UtilisateurDto?> GetUtilisateurById(int id);

    Task<UtilisateurDto?> GetUtilisateurByUsername(string username);

    Task<UtilisateurDto?> GetUtilisateurByEmail(string email);
}
