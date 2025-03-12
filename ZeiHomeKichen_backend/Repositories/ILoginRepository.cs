using System;
using ZeiHomeKichen_backend.Models;

namespace ZeiHomeKichen_backend.Repositories;

public interface ILoginRepository
{    
   Task<bool> ValidateUser(string username, string password);
   Task<Utilisateur> GetByUsername(string username);
}
