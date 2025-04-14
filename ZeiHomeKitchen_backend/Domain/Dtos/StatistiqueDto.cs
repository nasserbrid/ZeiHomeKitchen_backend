using System.Collections.Generic;
using System.Text.Json.Serialization;

public class StatistiqueDto
{
    public int IdStatistique { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly DateStatistique { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int TotalReservation { get; set; }

    //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    //public ICollection<int> Reservations { get; set; } = new List<int>(); 
}
