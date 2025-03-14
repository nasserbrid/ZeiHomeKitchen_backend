using System;
using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Services;

public interface IRegisterService
{
    Task<bool> Register(RegisterDto registerDto);

}
