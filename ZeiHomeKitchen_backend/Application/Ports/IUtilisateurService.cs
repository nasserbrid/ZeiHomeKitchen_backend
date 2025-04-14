using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports;

public interface IUtilisateurService
{
    Task<UtilisateurDto?> GetUtilisateurById(int id);

    Task<UtilisateurDto?> GetUtilisateurByUsername(string username);

    Task<UtilisateurDto?> GetUtilisateurByEmail(string email);
}
