using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;

namespace Thesaurus.DAL.Reposirories
{
    internal class SynonymRepository : RepositoryBase, ISynonymRepository
    {
        public SynonymRepository(IDbTransaction transaction) : base(transaction)
        {

        }
        public Task<int> AddAsync(SynonymDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<SynonymDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SynonymDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SynonymDTO>> GetByWordIdAsync(int wordId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(SynonymDTO entity)
        {
            throw new NotImplementedException();
        }

    }
}
