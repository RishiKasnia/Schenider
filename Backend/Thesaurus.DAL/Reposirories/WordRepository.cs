using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;

namespace Thesaurus.DAL.Reposirories
{

    /// <summary>
    /// Operations related word table of DB
    /// </summary>
    internal class WordRepository : IWordRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public WordRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddAsync(WordDTO entity)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Title");

            entity.Synonyms.ForEach(item =>
            {
                if(!string.IsNullOrEmpty(item.Title))
                dt.Rows.Add(item.SynonymId, item.Title);
            });

            string sql = "InsertWord";
            int wordId = await _unitOfWork.Connection.ExecuteScalarAsync<int>(sql, new { Title = entity.Title, Description = entity.Description, SynonymEntries = dt.AsTableValuedParameter("IdTitleList") },
                transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return wordId;
        }

        public async Task<int> DeleteAsync(int id)
        {
            string sql = "DeleteWord";
            int numberRows = await _unitOfWork.Connection.ExecuteAsync(sql, new { WordId = id }, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return numberRows;
        }

        public async Task<IReadOnlyList<WordDTO>> GetAllAsync(int pageSize, int pageNum)
        {
            string query = "GetAllWords";
            var list = await GetPagedWordAndSynonyms(query, pageSize: pageSize, pageNum: pageNum).ConfigureAwait(false);
            return list?.Distinct()?.ToList()?.AsReadOnly();
        }

        public async Task<WordDTO> GetByIdAsync(int id)
        {
            var query = "GetWordById";
            var list = await GetWordAndSynonyms(query, id).ConfigureAwait(false);

            //Rather than calling DB multiple times using synonym repository we get the duplicate results because of join and there will not be many thesaurus for each word
            return list?.FirstOrDefault();
        }

        public async Task<int> UpdateAsync(WordDTO entity)
        {
            var query = "UpdateWord";
            var result = await _unitOfWork.Connection.ExecuteAsync(query, param: new { Title = entity.Title, WordId = entity.WordId, Description = entity.Description }, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return result;
        }

        public async Task<WordDTO> GetWordByTitleAsync(string title)
        {
            var query = "GetWordByTitle";
            var list = await GetWordAndSynonyms(query, 0, title).ConfigureAwait(false);
            return list?.FirstOrDefault();
        }

        public async Task<int> GetTotalWordsCountAsync()
        {
            string sql = "GetAllWordsCount";
            var synonym = await _unitOfWork.Connection.QuerySingleAsync<int>(sql,commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return synonym;
        }
        private async Task<IEnumerable<WordDTO>> GetPagedWordAndSynonyms(string query, string title = "", int pageSize = 1, int pageNum = 1)
        {

            var parms = new DynamicParameters();

            parms.Add("@PageSize", pageSize);
            parms.Add("@PageNum", pageNum);
            if (!string.IsNullOrEmpty(title)) parms.Add("@Title", title);

            var lookup = new Dictionary<int, WordDTO>();

            var wordList = await _unitOfWork.Connection.QueryAsync<WordDTO, SynonymDTO, WordDTO>(
                query,
                (word, synonyms) =>
                {
                    if (!lookup.TryGetValue(word.WordId, out WordDTO found))
                    {
                        found = word;
                        found.Synonyms ??= new List<SynonymDTO>();
                        lookup.Add(word.WordId, found = word);
                    }

                    found.Synonyms.Add(synonyms);
                    return found;
                }, param: (parms.ParameterNames.Any()) ? parms : null, commandType: CommandType.StoredProcedure,
                splitOn: "SynonymId").ConfigureAwait(false);

            return wordList;
        }

        private async Task<IEnumerable<WordDTO>> GetWordAndSynonyms(string query, int wordId = 0, string title = "")
        {

            var parms = new DynamicParameters();
            if (wordId > 0)
            {
                parms.Add("@WordId", wordId);
            }
            if (!string.IsNullOrEmpty(title)) parms.Add("@Title", title);

            var lookup = new Dictionary<int, WordDTO>();

            var wordList = await _unitOfWork.Connection.QueryAsync<WordDTO, SynonymDTO, WordDTO>(
                query,
                (word, synonyms) =>
                {
                    if (!lookup.TryGetValue(word.WordId, out WordDTO found))
                    {
                        found = word;
                        found.Synonyms ??= new List<SynonymDTO>();
                        lookup.Add(word.WordId, found = word);
                    }

                    found.Synonyms.Add(synonyms);
                    return found;
                }, param: (parms.ParameterNames.Any()) ? parms : null, commandType: CommandType.StoredProcedure,
                splitOn: "SynonymId").ConfigureAwait(false);

            return wordList;
        }

        public async Task<List<string>> GetAllWordTitlesAsync()
        {
            string sql = "GetAllWordsTitles";
            var allTitles = await _unitOfWork.Connection.QueryAsync<string>(sql, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return allTitles.ToList();
        }
    }
}
