using System.ComponentModel.DataAnnotations;

namespace ZeiHomeKitchen_backend.Dtos;

public record class RegisterDto(
    [Required]
    string username,
    
    [Required]
    [EmailAddress]
    string Email,

    [Required] 
    [MinLength(6)]
    string Password
);
