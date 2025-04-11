using System.Text.Json.Serialization;

namespace ZeiHomeKitchen_backend.Dtos
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReservationStatusDto
    {
        EnAttente,
        Confirmee,
        Annulee

    }
    public record ReservationDto(
        int IdReservation,
        DateTime? DateReservation,
        string Adresse,
        ReservationStatusDto Statut,
        string Nom,       // Nom de l'utilisateur
        string Prenom,    // Prénom de l'utilisateur
        int NombrePersonnes) // Nombre de personnes pour la réservation
        

    //public record ReservationDto(
    //    int IdReservation,
    //    DateTime? DateReservation,
    //    string Adresse,
    //    ReservationStatusDto Statut,
    //    int IdUtilisateur,
    //    int IdStatistique,
    //    int NombrePersonnes,
    //    int? IdPaiement = null)
    {
        // Propriété mutable pour permettre à AfterMap de la modifier
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<int> PlatIds { get; set; } = new List<int>();

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        //public int? IdStatistique { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int IdUtilisateur { get; set; }
    }

    
}
