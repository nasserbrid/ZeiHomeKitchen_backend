using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Services;

namespace ZeiHomeKitchen_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "User")]
public class AuthController: ControllerBase
{
    private readonly ILoginService _loginService;

    private readonly IRegisterService _registerService;

    public AuthController(ILoginService loginService,IRegisterService registerService)
    {
        _loginService = loginService;

        _registerService = registerService;
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _loginService.Login(loginDto.username, loginDto.Password);

        if (token == null)
        {
            //Retourne une 401 si les identifiants sont invalides
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        Console.WriteLine("Requête de création d'utilisateur reçue.");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _registerService.Register(registerDto);
        if (!result)
        {
            return Conflict("Un utilisateur avec ce nom d'utilisateur ou email existe déjà !");
        }

        return Ok(new { message = "Inscription réussie !" });
    }

}
