using System;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports;

public interface IRegisterRepository
{
    Task<Utilisateur> CreateUser(RegisterDto registerDto);

    Task<bool> UserExists(string username, string email);

}
