public class CreateReservationDto
{
    public DateTime? DateReservation { get; set; }
    public string Adresse { get; set; }
    public int NombrePersonnes { get; set; }
    public List<int> PlatIds { get; set; } = new List<int>();
}
