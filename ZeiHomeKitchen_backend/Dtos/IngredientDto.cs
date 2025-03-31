namespace ZeiHomeKitchen_backend.Dtos;
//Record (immutable)/class (mutable)
public record IngredientDto(int IdIngredient, string? Nom,ICollection<int>? PlatIds = null);



