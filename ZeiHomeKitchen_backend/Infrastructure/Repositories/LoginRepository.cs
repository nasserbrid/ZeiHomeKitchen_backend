using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Infrastructure.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly UserManager<Utilisateur> _userManager;

    public LoginRepository(UserManager<Utilisateur> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Utilisateur> GetByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> ValidateUser(string username, string password)
    {
        var utilisateur = await _userManager.FindByNameAsync(username);

        if (utilisateur == null)
        {
            return false;
        }

        //Je vérifie le mot de passe
        var result = await _userManager.CheckPasswordAsync(utilisateur, password);
        return result;
    }

}
