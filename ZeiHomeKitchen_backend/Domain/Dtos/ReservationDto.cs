using System.Text.Json.Serialization;

namespace ZeiHomeKitchen_backend.Domain.Dtos
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
        string Nom,       
        string Prenom,    
        int NombrePersonnes) 
   
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
