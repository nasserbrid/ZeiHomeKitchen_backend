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

    public virtual Statistique IdStatistiqueNavigation { get; set; } = null!;

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;

    public virtual Paiement? Paiement { get; set; }

    public virtual ICollection<Plat> IdPlats { get; set; } = new List<Plat>();
}
