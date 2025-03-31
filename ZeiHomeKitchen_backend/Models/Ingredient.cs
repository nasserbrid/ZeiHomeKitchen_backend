using System;
using System.Collections.Generic;

namespace ZeiHomeKitchen_backend.Models;

public partial class Ingredient
{
    public int IdIngredient { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Plat> Plats { get; set; } = new List<Plat>();
}
