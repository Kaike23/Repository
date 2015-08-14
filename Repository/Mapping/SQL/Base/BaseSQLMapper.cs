using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Mapping.SQL.Base
{
    using Infrastructure.Database;
    using Infrastructure.Domain;

    public abstract class BaseSQLMapper<T> : IDataMapper<T>
        where T : IEntity
    {
        public BaseSQLMapper(IDatabase database)
        {
            //TODO: Review
            Database = database;
        }

        public IDatabase Database { get; set; }

        protected Dictionary<Guid, T> loadedMap = new Dictionary<Guid, T>();
        protected abstract string FindStatement { get; }
        protected abstract string InsertStatement { get; }
        protected abstract string UpdateStatement { get; }
        protected abstract string DeleteStatement { get; }

        #region IDataMapper

        public T Find(Guid id)
        {
            var result = loadedMap[id];
            if (result != null) return result;

            try
            {
                var sqlCommand = new SqlCommand(FindStatement, Database.Connection);
                sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                var reader = sqlCommand.ExecuteReader();
                return Load(reader);
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public List<T> FindMany(IStatementSource source)
        {
            try
            {
                var sqlCommand = new SqlCommand(source.Sql);
                sqlCommand.Parameters.AddRange(source.Parameters.ToArray());
                var reader = Database.FindAll(sqlCommand);
                return LoadAll(reader);
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public Guid Insert(T entity)
        {
            try
            {
                var insertCommand = new SqlCommand(InsertStatement);
                insertCommand.Parameters.AddWithValue("@Id", entity.Id);
                DoInsert(entity, insertCommand);
                var affectedRows = Database.Execute(insertCommand);
                loadedMap.Add(entity.Id, entity);
                return entity.Id;
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public int Update(T entity)
        {
            try
            {
                var updateCommand = new SqlCommand(UpdateStatement);
                updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                DoUpdate(entity, updateCommand);
                return Database.Execute(updateCommand);
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public int Delete(T entity)
        {
            try
            {
                var deleteCommand = new SqlCommand(DeleteStatement);
                deleteCommand.Parameters.AddWithValue("@Id", entity.Id);
                return Database.Execute(deleteCommand);
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }

        #endregion

        protected T Load(SqlDataReader reader)
        {
            if (!reader.HasRows) return default(T);
            var id = reader.GetGuid(0);
            if (loadedMap.ContainsKey(id)) return loadedMap[id];
            var result = DoLoad(id, reader);
            reader.Close();
            loadedMap.Add(id, result);
            return result;
        }
        protected List<T> LoadAll(SqlDataReader reader)
        {
            var result = new List<T>();
            while (reader.Read())
                result.Add(Load(reader));
            reader.Close();
            return result;
        }

        protected abstract T DoLoad(Guid id, SqlDataReader reader);
        protected abstract void DoInsert(T entity, SqlCommand insertCommand);
        protected abstract void DoUpdate(T entity, SqlCommand updateCommand);
    }
}
