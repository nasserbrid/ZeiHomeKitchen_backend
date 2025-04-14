using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace ZeiHomeKitchen_backend.Domain.Models;

public partial class Statistique
{
    public int IdStatistique { get; set; }

    public DateOnly DateStatistique { get; set; }

    public int TotalReservation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
