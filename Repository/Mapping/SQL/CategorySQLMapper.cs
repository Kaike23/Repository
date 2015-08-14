using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Mapping.SQL
{
    using Base;
    using Infrastructure.Database;
    using Model.Categories;

    public class CategorySQLMapper : BaseSQLMapper<Category>
    {
        public CategorySQLMapper(IDatabase database) : base(database) { }

        public static string Columns { get { return "*"; } }
        public static string TableName { get { return "Categories"; } }

        protected override string FindStatement { get { return string.Format("SELECT {0} FROM {1} WHERE Id = @Id", Columns, TableName); } }

        protected override Category DoLoad(Guid id, SqlDataReader reader)
        {
            var name = reader.GetString(1);
            return new Category() { Id = id, Name = name };
        }
        public List<Category> FindByName(string name)
        {
            return FindMany(new FindByNameStatement(name));
        }
        private class FindByNameStatement : IStatementSource
        {
            private static string _name;
            public FindByNameStatement(string name)
            {
                _name = name;
            }
            public List<SqlParameter> Parameters
            {
                get
                {
                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@Name", _name));
                    return parameters;
                }
            }
            public string Sql
            {
                get
                {
                    return "SELECT " + Columns +
                           " FROM " + TableName +
                           " WHERE UPPER(Name) like UPPER(@Name)" +
                           " ORDER BY Name";
                }
            }
        }

        protected override string InsertStatement { get { return string.Format("INSERT INTO {0} VALUES (@Id, @Name)", TableName); } }
        protected override string UpdateStatement { get { return string.Format("UPDATE {0} SET Name = @Name WHERE Id = @Id", TableName); } }
        protected override string DeleteStatement { get { return string.Format("DELETE FROM {0} WHERE Id = @Id", TableName); } }
        protected override void DoInsert(Category entity, SqlCommand insertCommand)
        {
            insertCommand.Parameters.AddWithValue("@Name", entity.Name);
        }
        protected override void DoUpdate(Category entity, SqlCommand updateCommand)
        {
            updateCommand.Parameters.AddWithValue("@Name", entity.Name);
        }
    }
}
