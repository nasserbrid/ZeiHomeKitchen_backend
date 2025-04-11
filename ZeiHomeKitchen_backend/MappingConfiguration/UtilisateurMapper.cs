using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Mappers;

public static class UtilisateurMapper
{
    public static UtilisateurDto ToDto(this Utilisateur utilisateur)
    {
        return new UtilisateurDto(
            utilisateur.Id,
            utilisateur.UserName,
            utilisateur.Nom,
            utilisateur.Prenom,
            utilisateur.Email
        );
    }

    public static Utilisateur ToModel(this UtilisateurDto dto)
    {
        return new Utilisateur
        {
            Id = dto.Id,
            UserName = dto.Email, 
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email
        };
    }
}
