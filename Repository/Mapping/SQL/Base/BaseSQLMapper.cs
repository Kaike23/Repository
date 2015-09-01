using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Mapping.SQL.Base
{
    using Infrastructure.Database;
    using Infrastructure.Domain;
    using Model.Base;

    public abstract class BaseSQLMapper<T> : IDataMapper<T>
        where T : BaseEntity
    {
        protected SqlConnection _dbConnection;

        public BaseSQLMapper(SqlConnection dbConnection)
        {
            //TODO: Review
            _dbConnection = dbConnection;
        }

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
                using (var sqlCommand = new SqlCommand(FindStatement, _dbConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                    var reader = sqlCommand.ExecuteReader();
                    result = Load(reader);
                    reader.Close();
                }
                return result;
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
                var sqlCommand = new SqlCommand(source.Sql, _dbConnection);
                sqlCommand.Parameters.AddRange(source.Parameters.ToArray());
                var reader = sqlCommand.ExecuteReader();
                var result = LoadAll(reader);
                reader.Close();
                return result;
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public Guid Insert(T entity, IDbTransaction transaction = null)
        {
            try
            {
                var insertCommand = new SqlCommand(InsertStatement, _dbConnection, (SqlTransaction)transaction);
                insertCommand.Parameters.AddWithValue("@Id", entity.Id);

                entity.GetVersion(_dbConnection, (SqlTransaction)transaction).Insert(_dbConnection, (SqlTransaction)transaction);

                DoInsert(entity, insertCommand);
                var affectedRows = insertCommand.ExecuteNonQuery();
                loadedMap.Add(entity.Id, entity);
                return entity.Id;
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public int Update(T entity, IDbTransaction transaction = null)
        {
            try
            {
                var updateCommand = new SqlCommand(UpdateStatement, _dbConnection, (SqlTransaction)transaction);
                updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                DoUpdate(entity, updateCommand);

                entity.GetVersion(_dbConnection, (SqlTransaction)transaction).Increment(_dbConnection, (SqlTransaction)transaction);

                return updateCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }
        public virtual void Delete(T entity, IDbTransaction transaction = null)
        {
            try
            {
                var deleteCommand = new SqlCommand(DeleteStatement, _dbConnection, (SqlTransaction)transaction);
                deleteCommand.Parameters.AddWithValue("@Id", entity.Id);

                entity.GetVersion(_dbConnection, (SqlTransaction)transaction).Increment(_dbConnection, (SqlTransaction)transaction);

                deleteCommand.ExecuteNonQuery();
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
            loadedMap.Add(id, result);
            return result;
        }
        protected List<T> LoadAll(SqlDataReader reader)
        {
            var result = new List<T>();
            if (reader.HasRows)
            {
                while (reader.Read())
                    result.Add(Load(reader));
            }
            return result;
        }

        protected abstract T DoLoad(Guid id, SqlDataReader reader);
        protected abstract void DoInsert(T entity, SqlCommand insertCommand);
        protected abstract void DoUpdate(T entity, SqlCommand updateCommand);
    }
}
