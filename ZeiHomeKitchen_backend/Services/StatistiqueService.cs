using AutoMapper;
using Microsoft.Extensions.Logging;
using ZeiHomeKitchen_backend.Dtos;
using ZeiHomeKitchen_backend.MappingConfiguration;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;

namespace ZeiHomeKitchen_backend.Services
{
    public class StatistiqueService : IStatistiqueService
    {
        private readonly IStatistiqueRepository _statistiqueRepository;
        private readonly ILogger<StatistiqueService> _logger;

        public StatistiqueService(IStatistiqueRepository statistiqueRepository, ILogger<StatistiqueService> logger)
        {
            _statistiqueRepository = statistiqueRepository;
            _logger = logger;
        }

        /// <summary>
        /// Récupère toutes les statistiques.
        /// </summary>
        /// <returns>Liste de tous les statistiques.</returns>
        public async Task<IEnumerable<StatistiqueDto>> GetAllStatistiques()
        {
            var statistiques = await _statistiqueRepository.GetAllStatistiques();
            return statistiques.Select(s => s.ToDto());
        }

        /// <summary>
        /// Récupère une statistique par son identifiant.
        /// </summary>
        /// <param name="statistiqueId">Identifiant de la statistique.</param>
        /// <returns>La statistique correspondante.</returns>
        public async Task<StatistiqueDto> GetStatistiqueById(int statistiqueId)
        {
            var statistique = await _statistiqueRepository.GetStatistiqueById(statistiqueId);
            return statistique?.ToDto();
        }

        /// <summary>
        /// Crée une nouvelle statistique.
        /// </summary>
        /// <param name="statistiqueDto">Données de la statistique à créer.</param>
        /// <returns>La statistique créée.</returns>
        public async Task<StatistiqueDto> CreateStatistique(StatistiqueDto statistiqueDto)
        {
            if (statistiqueDto == null)
            {
                throw new ArgumentNullException(nameof(statistiqueDto));
            }

            var statistiqueEntity = statistiqueDto.ToModel();
            var createdStatistique = await _statistiqueRepository.CreateStatistique(statistiqueEntity);
            return createdStatistique.ToDto();
        }

        /// <summary>
        /// Met à jour une statistique existante.
        /// </summary>
        /// <param name="statistiqueDto">Données mises à jour de la statistique.</param>
        /// <returns>La statistique mise à jour.</returns>
        public async Task<StatistiqueDto> UpdateStatistique(StatistiqueDto statistiqueDto)
        {
            if (statistiqueDto == null)
            {
                throw new ArgumentNullException(nameof(statistiqueDto));
            }

            var statistiqueEntity = statistiqueDto.ToModel();
            var updatedStatistique = await _statistiqueRepository.UpdateStatistique(statistiqueEntity);
            return updatedStatistique.ToDto();
        }

        /// <summary>
        /// Supprime une statistique par son identifiant.
        /// </summary>
        /// <param name="statistiqueId">Identifiant de la statistique à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public async Task<bool> DeleteStatistiqueById(int statistiqueId)
        {
            return await _statistiqueRepository.DeleteStatistiqueById(statistiqueId);
        }

        /// <summary>
        /// Récupère une statistique par date.
        /// </summary>
        /// <param name="date">Date de la statistique à récupérer.</param>
        /// <returns>La statistique correspondante pour la date spécifiée.</returns>
        public async Task<StatistiqueDto> GetStatistiqueByDate(DateOnly date)
        {
            var statistique = await _statistiqueRepository.GetStatistiqueByDate(date);
            return statistique?.ToDto();
        }

        /// <summary>
        /// Récupère les statistiques pour une période donnée.
        /// </summary>
        /// <param name="startDate">Date de début de la période.</param>
        /// <param name="endDate">Date de fin de la période.</param>
        /// <returns>Liste des statistiques pour la période spécifiée.</returns>
        public async Task<IEnumerable<StatistiqueDto>> GetStatistiquesForPeriod(DateOnly startDate, DateOnly endDate)
        {
            var statistiques = await _statistiqueRepository.GetStatistiquesForPeriod(startDate, endDate);
            return statistiques.Select(s => s.ToDto());
        }

    }
}
