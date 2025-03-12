using System;

namespace ZeiHomeKichen_backend.Services;

public interface ILoginService
{
    Task<string> Login(string username, string password);

}
