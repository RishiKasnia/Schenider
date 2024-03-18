using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;

namespace Thesaurus.DAL.Reposirories
{
    internal class ThesaurusRepository : RepositoryBase, IThesaurusRepository
    {


        public ThesaurusRepository(IDbTransaction transaction) : base(transaction)
        {

        }
        public Task<int> AddAsync(Entities.Thesaurus entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Entities.Thesaurus>> GetAllAsync()
        {
            throw new NotImplementedException();
           

            //var result = await Connection.ExecuteAsync(sql, entity, transaction: Transaction);
            //return result;
        }

        public Task<Entities.Thesaurus> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Entities.Thesaurus entity)
        {
            throw new NotImplementedException();
        }
    }
}
