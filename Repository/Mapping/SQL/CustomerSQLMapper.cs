using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mapping.SQL
{
    using Infrastructure.Lock;
    using Model.Addresses;
    using Model.Customers;
    using Repository.Mapping.SQL.Base;

    public class CustomerSQLMapper : BaseSQLMapper<Customer>
    {
        public CustomerSQLMapper(SqlConnection dbConnection) : base(dbConnection) { }

        public static string Columns { get { return "*"; } }
        public static string TableName { get { return "Customers"; } }

        protected override string FindStatement { get { return string.Format("SELECT {0} FROM {1} WHERE Id = @Id", Columns, TableName); } }

        protected override Customer DoLoad(Guid id, SqlDataReader reader)
        {
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);
            var versionId = reader.GetInt64(3);
            //var version = VersionLock.Find(versionId, _dbConnection);
            var customer = new Customer(id, versionId, firstName, lastName);
            //customer.SetSystemFields(version, version.Modified);
            return customer;
        }
        public List<Customer> FindByName(string firstName, string lastName)
        {
            return FindMany(new FindByNameStatement(firstName, lastName));
        }
        private class FindByNameStatement : IStatementSource
        {
            private static string _firstName;
            private static string _lastName;
            public FindByNameStatement(string firstName, string lastName)
            {
                _firstName = firstName;
                _lastName = lastName;
            }
            public List<SqlParameter> Parameters
            {
                get
                {
                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@FirstName", _firstName));
                    parameters.Add(new SqlParameter("@LastName", _lastName));
                    return parameters;
                }
            }
            public string Sql
            {
                get
                {
                    return "SELECT " + Columns +
                           " FROM " + TableName +
                           " WHERE UPPER(FirstName) like UPPER(@FisrtName)" +
                           "   AND UPPER(LastName) like UPPER(@LastName)" +
                           " ORDER BY LastName";
                }
            }
        }

        protected override string InsertStatement { get { return string.Format("INSERT INTO {0} VALUES (@Id, @FirstName, @LastName, @Version)", TableName); } }
        protected override string UpdateStatement { get { return string.Format("UPDATE {0} SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id", TableName); } }
        protected override string DeleteStatement { get { return string.Format("DELETE FROM {0} WHERE Id = @Id", TableName); } }
        protected override void DoInsert(Customer entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.AddWithValue("@FirstName", entity.FirstName);
            insertCommand.Parameters.AddWithValue("@LastName", entity.LastName);
            insertCommand.Parameters.AddWithValue("@Version", entity.VersionLock.Id);
        }
        protected override void DoUpdate(Customer entity, SqlCommand updateCommand)
        {
            updateCommand.Parameters.AddWithValue("@FirstName", entity.FirstName);
            updateCommand.Parameters.AddWithValue("@LastName", entity.LastName);
        }
        public override void Delete(Customer entity, System.Data.IDbTransaction transaction = null)
        {
            base.Delete(entity, transaction);
            entity.GetVersion(_dbConnection, (SqlTransaction)transaction).Delete(_dbConnection, (SqlTransaction)transaction);
        }
    }
}
