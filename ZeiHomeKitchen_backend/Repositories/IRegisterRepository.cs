using System;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public interface IRegisterRepository
{
    Task<Utilisateur> CreateUser(RegisterDto registerDto);

    Task<bool> UserExists(string username, string email);

}
