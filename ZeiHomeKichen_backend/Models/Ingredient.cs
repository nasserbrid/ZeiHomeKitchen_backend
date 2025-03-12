using System;
using System.Collections.Generic;

namespace ZeiHomeKichen_backend.Models;

public partial class Ingredient
{
    public int IdIngredient { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Plat> IdPlats { get; set; } = new List<Plat>();
}
