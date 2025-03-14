using Microsoft.AspNetCore.Identity;
using System;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Repositories;

namespace ZeiHomeKitchen_backend.Services;

public class RegisterService : IRegisterService
{
    private readonly IRegisterRepository _registerRepository;

    public RegisterService(IRegisterRepository registerRepository)
    {
        _registerRepository = registerRepository;
    }

    public async Task<bool> Register(RegisterDto registerDto)
    {
        if (await _registerRepository.UserExists(registerDto.username, registerDto.Email))
        {
            return false;
        }

        var user = await _registerRepository.CreateUser(registerDto);

        
        return user != null;
    }
}
