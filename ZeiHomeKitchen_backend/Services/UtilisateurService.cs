using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Repositories;
using ZeiHomeKitchen_backend.Mappers;
using ZeiHomeKitchen_backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ZeiHomeKitchen_backend.Services;

public class UtilisateurService : IUtilisateurService
{
    private readonly IUtilisateurRepository _utilisateurRepository;

    public UtilisateurService(IUtilisateurRepository utilisateurRepository)
    {
        _utilisateurRepository = utilisateurRepository;
    }

    public async Task<UtilisateurDto?> GetUtilisateurById(int id)
    {
        var utilisateur = await _utilisateurRepository.GetUtilisateurById(id);
        return utilisateur?.ToDto();
    }

    public async Task<UtilisateurDto?> GetUtilisateurByUsername(string username)
    {

        var utilisateurByName = await _utilisateurRepository.GetUtilisateurByUsername(username);
        return utilisateurByName?.ToDto();
    }

    public async Task<UtilisateurDto?> GetUtilisateurByEmail(string email)
    {

        var utilisateurByName = await _utilisateurRepository.GetUtilisateurByEmail(email);
        return utilisateurByName?.ToDto();
    }



}
