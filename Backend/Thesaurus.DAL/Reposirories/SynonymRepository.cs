using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;

namespace Thesaurus.DAL.Reposirories
{

    /// <summary>
    /// Operations related Synonym table of DB
    /// </summary>
    internal class SynonymRepository : ISynonymRepository
    {
        private IUnitOfWork _unitOfWork;

        public SynonymRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Add new Synonym to Database
        /// </summary>
        /// <param name="entity">word</param>
        /// <returns></returns>
        public async Task<int> AddAsync(SynonymDTO entity)
        {
            string sql = "InsertSynonym";

            int synonymId = await _unitOfWork.Connection.ExecuteScalarAsync<int>(sql, new { Title = entity.Title, wordId= entity.WordId }, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return synonymId;
        }

        /// <summary>
        /// Add multiple synonyms
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(List<SynonymDTO> entityList)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Title");

            entityList.ForEach(item =>
            {
                dt.Rows.Add(item.WordId, item.Title);
            });
            string processQuery = "InsertSynonyms";

            var number = await _unitOfWork.Connection.ExecuteAsync(processQuery, new { SynonymEntries = dt.AsTableValuedParameter("IdTitleList") }, transaction:_unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            _ = int.TryParse(number.ToString(), out int numberOfRecords);

            return numberOfRecords;
        }

        /// <summary>
        /// Update multiple items
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<SynonymDTO> entityList)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Title");

            entityList.ForEach(item =>
            {
                dt.Rows.Add(item.SynonymId, item.Title);
            });
            string processQuery = "UpdateSynonyms";

            var number = await _unitOfWork.Connection.ExecuteAsync(processQuery, new { SynonymEntries = dt.AsTableValuedParameter("IdTitleList") },transaction:_unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            _ = int.TryParse(number.ToString(), out int numberOfRecords);

            return numberOfRecords;
        }

        /// <summary>
        /// Delete Synonym by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id)
        {
            string sql = "DeleteSynonym";
            int numberRows = await _unitOfWork.Connection.ExecuteAsync(sql, new { SynonymId = id }, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return numberRows;
        }

        /// <summary>
        /// Delete all synonym's of a word
        /// </summary>
        /// <param name="wordId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAllByWordIdAsync(int wordId)
        {
            string sql = "DeleteSynonymByWord";
            int numberRows = await _unitOfWork.Connection.ExecuteAsync(sql, new { WordId = wordId }, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return numberRows;
        }

        /// <summary>
        /// Get the synonym by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<SynonymDTO> GetByIdAsync(int id)
        {
            string sql = "GetSynonymById";
            var synonym = await _unitOfWork.Connection.QuerySingleAsync<SynonymDTO>(sql, new { SynonymId = id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return synonym;
        }

        /// <summary>
        /// Update Synonym
        /// </summary>
        /// <param name="entity">Synonym</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(SynonymDTO entity)
        {
            string sql = "UpdateSynonym";
            int numberRows = await _unitOfWork.Connection.ExecuteAsync(sql, new { SynonymId = entity.SynonymId, Title = entity.Title }, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            return numberRows;
        }

    }
}
