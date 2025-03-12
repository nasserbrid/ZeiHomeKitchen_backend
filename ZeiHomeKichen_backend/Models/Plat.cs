using System;
using System.Collections.Generic;

namespace ZeiHomeKichen_backend.Models;

public partial class Plat
{
    public int IdPlat { get; set; }

    public string? Nom { get; set; }

    public string? Description { get; set; }

    public byte[] Image { get; set; } = null!;

    public decimal? Prix { get; set; }

    public virtual ICollection<Ingredient> IdIngredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Reservation> IdReservations { get; set; } = new List<Reservation>();
}
