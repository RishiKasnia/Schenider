using System.Collections.Generic;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;

namespace Thesaurus.DAL.Interfaces
{
    public interface ISynonymRepository : IGenericRepository<SynonymDTO>
    {
        /// <summary>
        /// Add multiple synonyms
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddRangeAsync(List<SynonymDTO> entity);

        /// <summary>
        /// Delete all synonym of a word 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAllByWordIdAsync(int wordId);

        /// <summary>
        /// Update multiple synonyms
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<SynonymDTO> entityList);
    }

}
