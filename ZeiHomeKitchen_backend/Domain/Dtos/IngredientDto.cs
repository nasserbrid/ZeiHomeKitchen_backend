using System.Text.Json.Serialization;

namespace ZeiHomeKitchen_backend.Domain.Dtos
{

    public record IngredientDto(
        int IdIngredient,
        string? Nom,

        // Ignore si null lors de la sérialisation JSON
        [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        ICollection<int>? PlatIds = null
    );
    //{
    //    // Ignore PlatIds si null lors de la sérialisation JSON
    //    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    //    public ICollection<int>? PlatIds
    //    {
    //        // Retourne null si PlatIds est vide
    //        get => PlatIds is { Count: > 0 } ? PlatIds : null;
    //        // Initialise PlatIds
    //        init => PlatIds = value; 
    //    }
    //}
}


