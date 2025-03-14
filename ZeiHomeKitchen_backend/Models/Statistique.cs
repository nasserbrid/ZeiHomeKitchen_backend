using System;
using System.Collections.Generic;

namespace ZeiHomeKitchen_backend.Models;

public partial class Statistique
{
    public int IdStatistique { get; set; }

    public DateOnly DateStatistique { get; set; }

    public int TotalReservation { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
