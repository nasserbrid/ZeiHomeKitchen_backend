using System;
using ZeiHomeKichen_backend.Repositories;

namespace ZeiHomeKichen_backend.Services;

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
