using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    using Infrastructure.Domain;
    using Infrastructure.Lock;

    public class BaseEntity : IEntity
    {
        private Guid _id;
        private DateTime _modified;
        private string _modifiedBy;
        private VersionLock _versionLock;
        private long? _versionId;

        public BaseEntity(Guid id, long? versionId = null)
        {
            _id = id;
            _versionId = versionId;
        }

        public void SetSystemFields(VersionLock versionLock, DateTime modified)
        {
            _versionLock = versionLock;
            _versionId = versionLock.Id;
            _modified = modified;
            _modifiedBy = VersionLock.SessionUser;
        }

        public VersionLock GetVersion(System.Data.SqlClient.SqlConnection dbConnection, System.Data.SqlClient.SqlTransaction transaction)
        {
            if (_versionId == null)
            {
                var versionLock = VersionLock.Create(dbConnection, transaction);
                SetSystemFields(versionLock, DateTime.Now);
            }
            return _versionLock;
        }

        public Guid Id { get { return _id; } }
        public DateTime Modified { get { return _modified; } }
        public string ModifiedBy { get { return _modifiedBy; } }
        public VersionLock VersionLock { get { return _versionLock; } }
    }
}
