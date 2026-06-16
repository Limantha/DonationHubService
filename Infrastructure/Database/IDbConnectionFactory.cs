using System.Data.Common;

namespace Infrastructure.Database
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
