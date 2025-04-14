using System.ComponentModel.DataAnnotations;

namespace ZeiHomeKitchen_backend.Domain.Dtos;

public record class RegisterDto(
    [Required]
    string username,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Nom,



   [Required]
   string Prenom,

    [Required]
    [MinLength(6)]
    string Password
);
