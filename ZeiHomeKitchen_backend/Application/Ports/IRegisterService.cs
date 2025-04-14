using System;
using ZeiHomeKitchen_backend.Domain.Dtos;

namespace ZeiHomeKitchen_backend.Application.Ports;

public interface IRegisterService
{
    Task<bool> Register(RegisterDto registerDto);

}
