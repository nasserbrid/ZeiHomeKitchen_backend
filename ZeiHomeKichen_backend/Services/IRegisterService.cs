using System;
using ZeiHomeKichen_backend.Dtos;

namespace ZeiHomeKichen_backend.Services;

public interface IRegisterService
{
    Task<bool> Register(RegisterDto registerDto);

}
