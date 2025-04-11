using System.Text.Json.Serialization;

namespace ZeiHomeKitchen_backend.Dtos
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
        int IdReservation        
    );
}

