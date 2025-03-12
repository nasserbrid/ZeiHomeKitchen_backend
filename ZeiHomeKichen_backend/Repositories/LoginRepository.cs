using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKichen_backend.Models;

namespace ZeiHomeKichen_backend.Repositories;

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
