using System.Collections.Generic;
using System.Threading.Tasks;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Services
{
    public interface IWordService
    {
        /// <summary>
        /// Add new word to Database
        /// </summary>
        /// <param name="entity">word</param>
        /// <returns></returns>
        Task<Word> AddAsync(Word entity);

        /// <summary>
        /// Get all words from DB along with synonym
        /// </summary>
        /// <returns></returns>
        Task<PagedOutput> GetAllAsync(int pageSize, int pageNum);

        /// <summary>
        /// Delete word by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Get the word by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<Word> GetByIdAsync(int id);

        /// <summary>
        /// Update word
        /// </summary>
        /// <param name="entity">word</param>
        /// <returns></returns>
        Task<Word> UpdateAsync(Word entity);

        /// <summary>
        /// Get the word by Title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<Word> GetWordByTitleAsync(string title);

        /// <summary>
        /// Get all the titles
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetAllWordTitlesAsync();

        /// <summary>
        /// Get all the related titles
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetTitleSuggestions(string input);
    }
}
