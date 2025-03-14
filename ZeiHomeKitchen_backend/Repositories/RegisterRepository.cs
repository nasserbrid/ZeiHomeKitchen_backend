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

            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            // Vous pouvez assigner des rôles ici si nécessaire
            // await _userManager.AddToRoleAsync(user, "User");
            return user;
        }

        // Si l'utilisateur n'est pas créé, vous pouvez lever une exception ou retourner null
        // Par exemple, vous pouvez lever une exception avec les erreurs
        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<bool> UserExists(string username, string email)
    {
        return await _userManager.FindByNameAsync(username) != null ||
               await _userManager.FindByEmailAsync(email) != null;
    }
}
