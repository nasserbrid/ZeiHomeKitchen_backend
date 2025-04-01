namespace ZeiHomeKitchen_backend.Dtos
{
    public enum ReservationStatusDto
    {
        EnAttente,
        Confirmee,
        Annulee

    }

    public record ReservationDto(
        int IdReservation, 
        DateTime? DateReservation,
        //string Adresse,
        ReservationStatusDto Statut,
        int IdUtilisateur,
        int? IdPaiement = null,
        ICollection<int>? PlatIds = null
        );
}
