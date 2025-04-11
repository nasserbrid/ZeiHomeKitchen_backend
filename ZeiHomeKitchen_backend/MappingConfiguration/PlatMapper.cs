using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.MappingConfiguration
{
    public static class PlatMapper
    {
        public static PlatDto ToDto(this Plat plat)
        {
            return new PlatDto(
                plat.IdPlat,
                plat.Nom,
                plat.Description,
                plat.Image,
                plat.Prix,
                plat.Ingredients?.Select(i => i.IdIngredient).ToList()
                //plat.Reservations?.Select(r => r.IdReservation).ToList()
            );
        }

        public static Plat ToModel(this PlatDto dto)
        {
            return new Plat
            {
                IdPlat = dto.IdPlat,
                Nom = dto.Nom,
                Description = dto.Description,
                Image = dto.Image,
                Prix = dto.Prix,

                // Ignoré pour éviter que je charge l'entité inutilement
                Ingredients = new List<Ingredient>(),
                Reservations = new List<Reservation>()
            };
        }
    }

}
