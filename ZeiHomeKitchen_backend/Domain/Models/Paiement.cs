using System;
using System.Collections.Generic;

namespace ZeiHomeKitchen_backend.Domain.Models
{
    public partial class Paiement
    {
        public int IdPaiement { get; set; }

        public decimal? Montant { get; set; }

        public string Statut { get; set; } = null!;

        public string Moyen { get; set; } = null!;

        public int IdReservation { get; set; }

        public string? StripePaymentIntentId { get; set; }

        public string? StripeClientSecret { get; set; }

        public DateTime? DateCreation { get; set; }

        public DateTime? DateMiseAJour { get; set; }

        public string? StripeStatus { get; set; }
        public virtual Reservation ReservationNavigation { get; set; } = null!;
    }
}
