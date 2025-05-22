using System.Text.Json.Serialization;

namespace ZeiHomeKitchen_backend.Domain.Dtos
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaiementStatusDto
    {
        EnAttente,
        Valide,
        Echoue

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaiementMoyenDto
    {
        CB,
        PayPal,

    }
    public record PaiementDto(
        int IdPaiement,
        decimal Montant,
        PaiementStatusDto Statut,
        PaiementMoyenDto Moyen,
        int IdReservation,
        string? StripePaymentIntentId = null,
        string? StripeClientSecret = null,
        DateTime? DateCreation = null,
        DateTime? DateMiseAJour = null,
        string? StripeStatus = null
    );
}

