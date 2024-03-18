using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;
using Thesaurus.DAL;

namespace Thesaurus.DAL.Reposirories
{
    internal class WordRepository : IWordRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private IDbConnection Connection;

        public WordRepository(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            Connection = _unitOfWork.Connection;
        }

        public async Task<int> AddAsync(WordDTO entity)
        {
            int wordId = 0;
            using (var transaction = Connection.BeginTransaction())
            {
                var sql = @"Insert into Word (Title) VALUES (@Title);SELECT CAST(SCOPE_IDENTITY() as int)";

                wordId = await Connection.ExecuteScalarAsync<int>(sql, new { Title = entity.Title }, transaction: transaction).ConfigureAwait(false);

                entity.Synonyms.ForEach(x => x.WordId = wordId);

                string processQuery = "INSERT INTO Synonym VALUES (@WordId, @Ttile)";

                var wait = await Connection.ExecuteAsync(processQuery, entity.Synonyms, transaction: transaction).ConfigureAwait(false);
            }

            return wordId;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int numberRows=0;
            using (var transaction = Connection.BeginTransaction())
            {
                var sql = "DELETE FROM Synonym WHERE WordId = @WordId";

                numberRows = await Connection.ExecuteAsync(sql, new { WordId = id });

                sql = "DELETE FROM Word WHERE WordId = @WordId";
                numberRows = await Connection.ExecuteAsync(sql, new { WordId = id });
            }
            return numberRows;
        }

        public async Task<IReadOnlyList<WordDTO>> GetAllAsync()
        {
            var query = @"SELECT w.*, s.* FROM Word w INNER JOIN Synonym s ON w.WordId = S.SynonymId";

            var thesaurusDictionary = new Dictionary<int, WordDTO>();
            var list = await Connection.QueryAsync<WordDTO, SynonymDTO, WordDTO>(
                query,
                (word, synonym) =>
                {
                    if (!thesaurusDictionary.TryGetValue(word.WordId, out WordDTO wordEntry))
                    {
                        wordEntry = word;
                        wordEntry.Synonyms = wordEntry.Synonyms ?? new List<SynonymDTO>();
                        thesaurusDictionary.Add(wordEntry.WordId, wordEntry);
                    }

                    wordEntry.Synonyms.Add(synonym);
                    return wordEntry;
                },
                splitOn: "SynonymId").ConfigureAwait(false);

           return list.Distinct().ToList().AsReadOnly();
        }

        public async Task<WordDTO> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Word WHERE WordId = @WordId";
            var result = await Connection.QuerySingleOrDefaultAsync<WordDTO>(sql, new { WordId = id });
            return result;

        }

        public async Task<int> UpdateAsync(WordDTO entity)
        {
            var sql = "UPDATE Word SET Title = @Title WHERE WordId = @WordId";
            var result = await Connection.ExecuteAsync(sql, entity);
            return result;
        }
    }
}
