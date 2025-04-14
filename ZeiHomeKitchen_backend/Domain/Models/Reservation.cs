using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZeiHomeKitchen_backend.Domain.Models
{
    public partial class Reservation
    {
        public int IdReservation { get; set; }

        public DateTime? DateReservation { get; set; }

        public string Adresse { get; set; } = null!;

        public string Statut { get; set; } = null!;

        public int NombrePersonnes { get; set; }

        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;

        public int IdUtilisateur { get; set; }
        public int? IdStatistique { get; set; }

        [JsonIgnore]
        public virtual Statistique StatistiqueNavigation { get; set; } = null!;
        public virtual Utilisateur UtilisateurNavigation { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Plat> Plats { get; set; } = new List<Plat>();

        public virtual Paiement? Paiement { get; set; }
    }
}
