using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories
{
    public interface IStatistiqueRepository
    {
        Task<IEnumerable<Statistique>> GetAllStatistiques();
        Task<Statistique> GetStatistiqueById(int statistiqueId);
        Task<Statistique> CreateStatistique(Statistique statistique);
        Task<Statistique> UpdateStatistique(Statistique statistique);
        Task<bool> DeleteStatistiqueById(int statistiqueId);
        Task<Statistique> GetStatistiqueByDate(DateOnly date);
        Task<IEnumerable<Statistique>> GetStatistiquesForPeriod(DateOnly startDate, DateOnly endDate);
    }
}
