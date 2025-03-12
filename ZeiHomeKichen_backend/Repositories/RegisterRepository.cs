using System;
using Microsoft.AspNetCore.Identity;
using ZeiHomeKichen_backend.Dtos;
using ZeiHomeKichen_backend.Models;

namespace ZeiHomeKichen_backend.Repositories;

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
        return result.Succeeded ? user : null;
    }

    public async Task<bool> UserExists(string username, string email)
    {
        return await _userManager.FindByNameAsync(username) != null ||
               await _userManager.FindByEmailAsync(email) != null;
    }
}
