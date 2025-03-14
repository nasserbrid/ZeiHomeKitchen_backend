using System;

namespace ZeiHomeKitchen_backend.Services;

public interface ILoginService
{
    Task<string> Login(string username, string password);

}
