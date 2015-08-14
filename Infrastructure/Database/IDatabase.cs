using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infrastructure.Database
{
    using Infrastructure.Domain;

    public interface IDatabase
    {
        int Execute(SqlCommand command);
        SqlDataReader FindAll(SqlCommand command);
        SqlTransaction BeginTransation();
        SqlConnection Connection { get; }
    }
}
