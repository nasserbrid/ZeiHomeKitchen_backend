using System;
using System.Collections.Generic;

namespace ZeiHomeKitchen_backend.Models;

public partial class Paiement
{
    public int IdPaiement { get; set; }

    public decimal? Montant { get; set; }

    public string Statut { get; set; } = null!;

    public string Moyen { get; set; } = null!;

    public int IdReservation { get; set; }

    public virtual Reservation IdReservationNavigation { get; set; } = null!;
}
