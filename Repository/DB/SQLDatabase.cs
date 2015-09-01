//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlClient;

//namespace Repository.DB
//{
//    using Infrastructure.Database;
//    using Infrastructure.Domain;

//    public class SQLDatabase : IDatabase
//    {
//        SqlConnection _connection;
//        SqlTransaction _transaction;

//        public SQLDatabase(string connectionString)
//        {
//            _connection = new SqlConnection(connectionString);
//        }

//        public SqlConnection Connection { get { return _connection; } }

//        public SqlTransaction BeginTransation()
//        {
//            _transaction = _connection.BeginTransaction();
//            return _transaction;
//        }

//        public SqlDataReader FindAll(SqlCommand command)
//        {
//            command.Connection = _connection;
//            return command.ExecuteReader();
//        }

//        public int Execute(SqlCommand command)
//        {
//            command.Connection = _connection;
//            command.Transaction = _transaction;
//            return command.ExecuteNonQuery();
//        }
//    }
//}
