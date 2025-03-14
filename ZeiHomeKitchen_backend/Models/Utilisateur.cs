using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ZeiHomeKitchen_backend.Models;

public partial class Utilisateur : IdentityUser<int>
{

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public string Role { get; set; } = "User";

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
