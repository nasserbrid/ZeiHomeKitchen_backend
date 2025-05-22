using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Infrastructure.MappingConfiguration
{
    public static class PaiementMapper
    {
        // Méthode pour convertir un objet Paiement en PaiementDto
        public static PaiementDto ToDto(this Paiement paiement)
        {
            return new PaiementDto(
                paiement.IdPaiement,
                paiement.Montant ?? 0,
                Enum.Parse<PaiementStatusDto>(paiement.Statut),
                Enum.Parse<PaiementMoyenDto>(paiement.Moyen),
                paiement.IdReservation,
                paiement.StripePaymentIntentId,
                paiement.StripeClientSecret,
                paiement.DateCreation,
                paiement.DateMiseAJour,
                paiement.StripeStatus
            );
        }

        // Méthode pour convertir un PaiementDto en objet Paiement
        public static Paiement ToModel(this PaiementDto dto)
        {
            return new Paiement
            {
                IdPaiement = dto.IdPaiement,
                Montant = dto.Montant,
                Statut = dto.Statut.ToString(),
                Moyen = dto.Moyen.ToString(),
                IdReservation = dto.IdReservation,
                StripePaymentIntentId = dto.StripePaymentIntentId,
                StripeClientSecret = dto.StripeClientSecret,
                DateCreation = dto.DateCreation,
                DateMiseAJour = dto.DateMiseAJour,
                StripeStatus = dto.StripeStatus
            };
        }
    }
}
