using System;
using ZeiHomeKitchen_backend.Application.Ports;

namespace ZeiHomeKitchen_backend.Domain.Services;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;
    private readonly TokenService _tokenService;

    public LoginService(ILoginRepository loginRepository, TokenService tokenService)
    {
        _loginRepository = loginRepository;
        _tokenService = tokenService;
    }
    public async Task<string> Login(string username, string password)
    {
        var isValid = await _loginRepository.ValidateUser(username, password);

        if (isValid)
        {
            var user = await _loginRepository.GetByUsername(username);
            return _tokenService.GenerateToken(user);
        }

        return null;

    }
}
