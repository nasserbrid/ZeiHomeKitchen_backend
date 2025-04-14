using System;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports;

public interface ILoginRepository
{
    Task<bool> ValidateUser(string username, string password);
    Task<Utilisateur> GetByUsername(string username);
}
