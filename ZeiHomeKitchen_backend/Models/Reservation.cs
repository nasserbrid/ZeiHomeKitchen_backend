using System;
using System.Collections.Generic;

namespace ZeiHomeKitchen_backend.Models;

public partial class Reservation
{
    public int IdReservation { get; set; }

    public DateTime? DateReservation { get; set; }

    public string Adresse { get; set; } = null!;

    public string Statut { get; set; } = null!;

    public int IdStatistique { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual Statistique StatistiqueNavigation { get; set; } = null!;

    public virtual Utilisateur UtilisateurNavigation { get; set; } = null!;

    public virtual Paiement? Paiement { get; set; }

    public virtual ICollection<Plat> Plats { get; set; } = new List<Plat>();
}
