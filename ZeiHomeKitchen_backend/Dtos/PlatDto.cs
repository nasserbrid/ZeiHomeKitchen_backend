using System.Text.Json.Serialization;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Dtos
{
    public record PlatDto(
        int IdPlat,
        string? Nom,
        string? Description,
        byte[] Image,
        decimal? Prix,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
        ICollection<int>? IngredientIds = null

        //[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
        //ICollection<int>? ReservationIds = null
        
        )

    {
        

        public string ImageBase64 => Convert.ToBase64String(Image);
    }

}
