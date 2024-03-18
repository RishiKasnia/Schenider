using System.Collections.Generic;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;

namespace Thesaurus.DAL.Interfaces
{
    public interface IWordRepository : IGenericRepository<WordDTO>
    {
        Task<WordDTO> GetWordByTitleAsync(string title);

        Task<List<string>> GetAllWordTitlesAsync();

        Task<IReadOnlyList<WordDTO>> GetAllAsync(int pageSize, int pageNum);

        Task<int> GetTotalWordsCountAsync();
    }
}
