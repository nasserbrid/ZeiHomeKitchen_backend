using System;
using Microsoft.AspNetCore.Identity;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public class RegisterRepository : IRegisterRepository
{
    private readonly UserManager<Utilisateur> _userManager;

    public RegisterRepository(UserManager<Utilisateur> userManager)
    {
        _userManager = userManager;
        
    }
    public async Task<Utilisateur> CreateUser(RegisterDto registerDto)
    {
        var user = new Utilisateur{

            UserName = registerDto.username,

            Email = registerDto.Email,

            Nom = registerDto.Nom, 
          
            Prenom = registerDto.Prenom, 
           
            DateCreation = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            
            await _userManager.AddToRoleAsync(user, "User");
            return user;
        }

        
        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<bool> UserExists(string username, string email)
    {
        var userByUsername = await _userManager.FindByNameAsync(username);
        var userByEmail = await _userManager.FindByEmailAsync(email);

        return userByUsername != null || userByEmail != null;
    }
}
