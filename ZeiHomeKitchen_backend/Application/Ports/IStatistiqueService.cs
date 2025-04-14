using ZeiHomeKitchen_backend.Domain.Dtos;
using ZeiHomeKitchen_backend.Domain.Models;

namespace ZeiHomeKitchen_backend.Application.Ports
{
    public interface IStatistiqueService
    {
        Task<IEnumerable<StatistiqueDto>> GetAllStatistiques();
        Task<StatistiqueDto> GetStatistiqueById(int statistiqueId);
        Task<StatistiqueDto> CreateStatistique(StatistiqueDto statistiqueDto);
        Task<StatistiqueDto> UpdateStatistique(StatistiqueDto statistiqueDto);
        Task<bool> DeleteStatistiqueById(int statistiqueId);
        Task<StatistiqueDto> GetStatistiqueByDate(DateOnly date);
        Task<IEnumerable<StatistiqueDto>> GetStatistiquesForPeriod(DateOnly startDate, DateOnly endDate);
    }
}