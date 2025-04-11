using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public class StatistiqueRepository : IStatistiqueRepository
    {
        private readonly ZeiHomeKitchenContext _zeiHomeKitchenContext;

        public StatistiqueRepository(ZeiHomeKitchenContext zeiHomeKitchenContext)
        {
            _zeiHomeKitchenContext = zeiHomeKitchenContext;
        }

        public async Task<IEnumerable<Statistique>> GetStatistiquesForPeriod(DateOnly startDate, DateOnly endDate)
        {
            return await _zeiHomeKitchenContext.Statistiques
                .Where(s => s.DateStatistique >= startDate && s.DateStatistique <= endDate)
                .ToListAsync();
        }

        // Implémentez les autres méthodes de l'interface ici
        public async Task<IEnumerable<Statistique>> GetAllStatistiques()
        {
            return await _zeiHomeKitchenContext.Statistiques.ToListAsync();
        }

        public async Task<Statistique> GetStatistiqueById(int statistiqueId)
        {
            return await _zeiHomeKitchenContext.Statistiques.FindAsync(statistiqueId);
        }

        public async Task<Statistique> CreateStatistique(Statistique statistique)
        {
            var result = await _zeiHomeKitchenContext.Statistiques.AddAsync(statistique);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Statistique> UpdateStatistique(Statistique statistique)
        {
            var existingStatistique = await _zeiHomeKitchenContext.Statistiques.FindAsync(statistique.IdStatistique);
            if (existingStatistique == null)
                throw new KeyNotFoundException($"Statistique {statistique.IdStatistique} non trouvée.");


            existingStatistique.DateStatistique = statistique.DateStatistique;
            existingStatistique.TotalReservation = statistique.TotalReservation;
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return existingStatistique;
        }

        public async Task<bool> DeleteStatistiqueById(int statistiqueId)
        {
            var statistique = await _zeiHomeKitchenContext.Statistiques.FindAsync(statistiqueId);
            if (statistique == null) return false;

            _zeiHomeKitchenContext.Statistiques.Remove(statistique);
            await _zeiHomeKitchenContext.SaveChangesAsync();
            return true;
        }

        public async Task<Statistique> GetStatistiqueByDate(DateOnly date)
        {
            var statistique = await _zeiHomeKitchenContext.Statistiques
         .FirstOrDefaultAsync(s => s.DateStatistique == date);

            return statistique ;
        }
    }
}
