using System;

namespace ZeiHomeKitchen_backend.Application.Ports;

public interface ILoginService
{
    Task<string> Login(string username, string password);

}
