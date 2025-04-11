using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace ZeiHomeKitchen_backend.Models;

public partial class Plat
{
    public int IdPlat { get; set; }

    public string? Nom { get; set; }

    public string? Description { get; set; }

    public byte[] Image { get; set; } = null!;

    public decimal? Prix { get; set; }

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();


    [JsonIgnore]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
