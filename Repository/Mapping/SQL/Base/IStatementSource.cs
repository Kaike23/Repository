using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repository.Mapping.SQL.Base
{
    public interface IStatementSource
    {
        string Sql { get; }
        List<SqlParameter> Parameters { get; }
    }
}
