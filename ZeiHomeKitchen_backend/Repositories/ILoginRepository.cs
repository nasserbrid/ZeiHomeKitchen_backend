using System;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public interface ILoginRepository
{    
   Task<bool> ValidateUser(string username, string password);
   Task<Utilisateur> GetByUsername(string username);
}
