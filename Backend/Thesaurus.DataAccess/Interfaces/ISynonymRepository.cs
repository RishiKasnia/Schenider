using System.Collections.Generic;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;

namespace Thesaurus.DAL.Interfaces
{
    public interface ISynonymRepository : IGenericRepository<SynonymDTO>
    {
        Task<List<SynonymDTO>> GetByWordIdAsync(int wordId);

    }
}
