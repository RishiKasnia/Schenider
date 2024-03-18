using System.Collections.Generic;
using System.Threading.Tasks;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Services
{
    public interface ISynonymService
    {
        /// <summary>
        /// Add new Synonym to Database
        /// </summary>
        /// <param name="entity">Synonym</param>
        /// <returns></returns>
        Task<Synonym> AddAsync(Synonym entity);


        /// <summary>
        /// Delete Synonym by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Delete all synonym's of a word
        /// </summary>
        /// <param name="wordId"></param>
        /// <returns></returns>
        Task<bool> DeleteAllByWordIdAsync(int wordId);


        /// <summary>
        /// Get the synonym by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<Synonym> GetByIdAsync(int id);

        /// <summary>
        /// Update Synonym
        /// </summary>
        /// <param name="entity">Synonym</param>
        /// <returns></returns>
        Task<Synonym> UpdateAsync(Synonym entity);

        /// <summary>
        /// Update Synonym
        /// </summary>
        /// <param name="entity">Synonym</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(List<Synonym> entity);

        /// <summary>
        /// Add multiple synonyms
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(List<Synonym> entities);

    }
}
