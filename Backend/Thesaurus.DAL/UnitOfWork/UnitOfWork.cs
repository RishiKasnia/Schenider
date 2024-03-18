using System.Data;

namespace Thesaurus.DAL
{
    internal class UnitOfWork : IUnitOfWork
    {

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;


        public UnitOfWork()
        {
        }


        IDbConnection IUnitOfWork.Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }

        public void Begin()
        {
            if(_transaction==null)
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
                _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
        }
    }
}
