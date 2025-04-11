using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.Models;
using System.Linq;

namespace ZeiHomeKitchen_backend.MappingConfiguration
{
    public static class StatistiqueMapper
    {
        public static StatistiqueDto ToDto(this Statistique statistique)
        {
            // Vérifie si l'objet statistique est null
            if (statistique == null)
                throw new ArgumentNullException(nameof(statistique), "L'objet Statistique ne peut pas être nul.");

            return new StatistiqueDto
            {
                IdStatistique = statistique.IdStatistique,
                DateStatistique = statistique.DateStatistique,
                TotalReservation = statistique.TotalReservation,
                //Reservations = statistique.Reservations?.Select(s => s.IdReservation).ToList() ?? new List<int>()
            };
        }

        public static Statistique ToModel(this StatistiqueDto dto)
        {
            // Vérifie si l'objet dto est null
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "L'objet StatistiqueDto ne peut pas être nul.");

            return new Statistique
            {
                IdStatistique = dto.IdStatistique,
                DateStatistique = dto.DateStatistique,
                TotalReservation = dto.TotalReservation,
                Reservations = new List<Reservation>() 
            };
        }
    }
}
