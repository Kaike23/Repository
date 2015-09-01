using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Lock
{
    public class VersionLock
    {
        public static Dictionary<VersionLock, string> Versions = new Dictionary<VersionLock, string>();
        public static string SessionUser = "Kaike" + new Random().Next();

        private long id;
        private long value;
        private string modifiedBy;
        private DateTime modified;
        private bool locked;
        private bool isNew;
        private static string UPDATE_SQL = "UPDATE VersionLock SET Value = {0}, ModifiedBy = '{1}', Modified = '{2}' WHERE Id = {3} AND Value = {4}";
        private static string DELETE_SQL = "DELETE  FROM VersionLock WHERE Id = {0} AND Value = {1}";
        private static string INSERT_SQL = "INSERT INTO VersionLock VALUES ({0}, {1}, '{2}', '{3}')";
        private static string LOAD_SQL = "SELECT Id, Value, ModifiedBy, Modified FROM VersionLock WHERE Id = {0}";

        public static VersionLock Find(long id, SqlConnection dbConnection)
        {
            VersionLock version = null;
            if (Versions.Values.Count != 0)
                version = Versions.Keys.FirstOrDefault(x => x.id == id);
            if (version == null)
            {
                version = Load(id, dbConnection);
            }
            return version;
        }
        private static VersionLock Load(long id, SqlConnection dbConnection)
        {
            VersionLock versionLock = null;
            try
            {
                var query = string.Format(LOAD_SQL, id);
                using (var selectCommand = new SqlCommand(query, dbConnection))
                {
                    var reader = selectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        var value = reader.GetInt64(1);
                        var modifiedBy = reader.GetString(2);
                        var modified = reader.GetDateTime(3);
                        versionLock = new VersionLock(id, value, modifiedBy, modified);
                        Versions.Add(versionLock, SessionUser);
                    }
                    else
                    {
                        throw new SystemException(string.Format("Version {0} not found.", id));
                    }
                    reader.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                throw new SystemException("unexpected sql error loading version", sqlEx);
            }
            return versionLock;
        }
        public static VersionLock Create(SqlConnection dbConnection, SqlTransaction transaction)
        {
            var selectCommand = new SqlCommand("SELECT IIF(MAX(Id) is NULL, -1, MAX(Id)) + 1 FROM VersionLock", dbConnection, transaction);
            var reader = selectCommand.ExecuteReader();
            reader.Read();
            var newId = reader.GetInt64(0);
            reader.Close();

            VersionLock version = new VersionLock(newId, 0, SessionUser, DateTime.Now);
            version.isNew = true;
            return version;
        }
        private VersionLock(long id, long value, string modifiedBy, DateTime modified)
        {
            this.id = id;
            this.value = value;
            this.modifiedBy = modifiedBy;
            this.modified = modified;
        }

        public long Id { get { return id; } }
        public string ModifiedBy { get { return modifiedBy; } }
        public DateTime Modified { get { return modified; } }

        public void Insert(SqlConnection dbConnection, SqlTransaction transaction)
        {
            if (isNew)
            {
                try
                {
                    var query = string.Format(INSERT_SQL, id, value, modifiedBy, modified.ToString());
                    var insertCommand = new SqlCommand(query, dbConnection, transaction);
                    insertCommand.ExecuteNonQuery();
                    Versions.Add(this, SessionUser);
                    isNew = false;
                }
                catch (SqlException sqlEx)
                {
                    throw new SystemException("unexpected sql error inserting version", sqlEx);
                }
            }
        }
        public void Increment(SqlConnection dbConnection, SqlTransaction transaction)
        {
            if (!locked)
            {
                try
                {
                    var query = string.Format(UPDATE_SQL, value + 1, modifiedBy, modified.ToString(), id, value);
                    var updateCommand = new SqlCommand(query, dbConnection, transaction);
                    var rowCount = updateCommand.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        ThrowConcurrencyException(dbConnection);
                    }
                    value++;
                    locked = true;
                }
                catch (SqlException sqlEx)
                {
                    throw new SystemException("unexpected sql error incrementing version", sqlEx);
                }
            }
        }
        public void Delete(SqlConnection dbConnection, SqlTransaction transaction)
        {
            try
            {
                var query = string.Format(DELETE_SQL, id, value);
                var deleteCommand = new SqlCommand(query, dbConnection, transaction);
                var rowCount = deleteCommand.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    ThrowConcurrencyException(dbConnection);
                }
                Versions.Remove(this);
            }
            catch (SqlException sqlEx)
            {
                throw new SystemException("unexpected sql error incrementing version", sqlEx);
            }
        }
        public void Release()
        {
            locked = false;
        }
        private void ThrowConcurrencyException(SqlConnection dbConnection)
        {
            VersionLock currentVersion = Load(this.id, dbConnection);
            throw new SystemException(string.Format("version modified by {0} at {1}", currentVersion.modifiedBy, currentVersion.modified));
        }
    }
}
