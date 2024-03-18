using System.Data;

namespace Thesaurus.DAL
{
    public interface IConnectionFactory
    {
        IDbConnection SQLConnection { get; }
    }
}
