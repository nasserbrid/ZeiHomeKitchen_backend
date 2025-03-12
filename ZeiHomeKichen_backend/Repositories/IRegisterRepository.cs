using System;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using ZeiHomeKichen_backend.Dtos;
using ZeiHomeKichen_backend.Models;

namespace ZeiHomeKichen_backend.Repositories;

public interface IRegisterRepository
{
    Task<Utilisateur> CreateUser(RegisterDto registerDto);

    Task<bool> UserExists(string username, string email);

}
