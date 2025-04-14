using System;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeiHomeKitchen_backend.Application.Ports;
using ZeiHomeKitchen_backend.Domain.Dtos;

namespace ZeiHomeKitchen_backend.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "User")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRegisterService _registerService;
    private readonly IUtilisateurService _utilisateurService;

    public AuthController(ILoginService loginService, IRegisterService registerService, IUtilisateurService utilisateurService)
    {
        _loginService = loginService;
        _registerService = registerService;
        _utilisateurService = utilisateurService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _loginService.Login(loginDto.username, loginDto.Password);
        Console.WriteLine($"Token : {token}");

        if (token == null)
        {
            //Retourne une 401 si les identifiants sont invalides
            return Unauthorized();
        }

        var user = await _utilisateurService.GetUtilisateurByUsername(loginDto.username);
        if (user == null)
        {
            return Unauthorized();
        }

        Console.WriteLine($"Nouveau Token : {token}, user : {user}");
        return Ok(new
        {
            Token = token,
            User = user
        });
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

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        Console.WriteLine($"Token reçu: {Request.Headers["Authorization"]}");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Console.WriteLine($"User ID from token: {userId}");

        if (userId == null)
        {
            Console.WriteLine("Utilisateur non authentifié.");
            return Unauthorized("Utilisateur non authentifié.");
        }

        var user = await _utilisateurService.GetUtilisateurById(int.Parse(userId));

        if (user == null)
        {
            return NotFound("Utilisateur non trouvé.");
        }

        return Ok(user);
    }

    // Endpoint pour vérifier si un utilisateur existe avec le username ou l'email donné
    [HttpGet("user/exists")]
    public async Task<IActionResult> CheckUserExists([FromQuery] string username, [FromQuery] string email)
    {
        Console.WriteLine($"Vérification de l'existence d'un utilisateur : username = {username}, email = {email}");

        // Vérifie si un utilisateur existe avec le nom d'utilisateur donné
        var userByUsername = !string.IsNullOrWhiteSpace(username) ? await _utilisateurService.GetUtilisateurByUsername(username) : null;

        // Vérifie si un utilisateur existe avec l'email donné
        var userByEmail = !string.IsNullOrWhiteSpace(email) ? await _utilisateurService.GetUtilisateurByEmail(email) : null;

        var exists = userByUsername != null || userByEmail != null;

        return Ok(new { exists });
    }

}


//using System;
//using System.Runtime.Intrinsics.X86;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ZeiHomeKitchen_backend.Dtos;
//using ZeiHomeKitchen_backend.Services;

//namespace ZeiHomeKitchen_backend.Controllers;


//[ApiController]
//[Route("api/[controller]")]
////[Authorize(Roles = "User")]
//public class AuthController: ControllerBase
//{
//    private readonly ILoginService _loginService;

//    private readonly IRegisterService _registerService;

//    private readonly IUtilisateurService _utilisateurService;

//    public AuthController(ILoginService loginService,IRegisterService registerService, IUtilisateurService utilisateurService)
//    {
//        _loginService = loginService;

//        _registerService = registerService;

//        _utilisateurService = utilisateurService;

//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
//    {
//        var token = await _loginService.Login(loginDto.username, loginDto.Password);
//        Console.WriteLine($"Token : {token}");

//        if (token == null)
//        {
//            //Retourne une 401 si les identifiants sont invalides
//            return Unauthorized();
//        }

//        var user = await _utilisateurService.GetUtilisateurByUsername(loginDto.username);
//        if (user == null) { 
//            return Unauthorized();
//        }

//        //return Ok(new { Token = token });
//        Console.WriteLine($"Nouveau Token : {token}, user : {user}");
//        return Ok(new { Token = token,
//               User = user
//            //user = new
//            //{
//            //    id = user.Id.ToString(),
//            //    email = user.Email,
//            //    firstName = user.Prenom,
//            //    lastName = user.Nom
//            //}
//        });
//    }

//    [HttpPost("register")]
//    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
//    {
//        Console.WriteLine("Requête de création d'utilisateur reçue.");
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }

//        var result = await _registerService.Register(registerDto);
//        if (!result)
//        {
//            return Conflict("Un utilisateur avec ce nom d'utilisateur ou email existe déjà !");
//        }

//        return Ok(new { message = "Inscription réussie !" });
//    }

//    [Authorize]
//    [HttpGet("user")]
//    public async Task<IActionResult> GetCurrentUser()
//    {
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Récupérer l'ID de l'utilisateur à partir du token

//        Console.WriteLine($"User ID from token: {userId}"); // Log de l'ID utilisateur


//        if (userId == null)
//        {
//            Console.WriteLine("Utilisateur non authentifié.");
//            return Unauthorized("Utilisateur non authentifié."); // Retourner 401 si l'utilisateur n'est pas authentifié
//        }

//        var user = await _utilisateurService.GetUtilisateurById(int.Parse(userId)); // Récupérer l'utilisateur par ID

//        if (user == null)
//        {
//            return NotFound("Utilisateur non trouvé."); // Gérer le cas où l'utilisateur n'est pas trouvé
//        }

//        return Ok(user); // Retourner l'utilisateur
//    }


//    //[HttpGet("user")]
//    //public async Task<IActionResult> GetCurrentUser()
//    //{
//    //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
//    //    if (userIdClaim == null)
//    //    {
//    //        return Unauthorized("Utilisateur non authentifié.");
//    //    }

//    //    if (!int.TryParse(userIdClaim.Value, out int userId))
//    //    {
//    //        return BadRequest("ID utilisateur invalide.");
//    //    }

//    //    var utilisateur = await _utilisateurService.GetUtilisateurById(userId);
//    //    if (utilisateur == null)
//    //    {
//    //        return NotFound("Utilisateur non trouvé.");
//    //    }

//    //    return Ok(utilisateur);
//    //}

//}
