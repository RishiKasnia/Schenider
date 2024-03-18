using System;
using System.Data;

namespace Thesaurus.DAL
{
    public sealed class DalSession : IDalSession,IDisposable
    {

        IDbConnection _connection = null;
        IUnitOfWork _unitOfWork = null;
        IConnectionFactory _connectionFactory = null;

        public DalSession(IConnectionFactory connectionFactory, IUnitOfWork unitOfWork)
        {
            _connectionFactory = connectionFactory;
            _connection =  _connectionFactory.SQLConnection;
            _connection.Open();
            _unitOfWork = unitOfWork;
            _unitOfWork.Connection = _connection;
        }


        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }

}
