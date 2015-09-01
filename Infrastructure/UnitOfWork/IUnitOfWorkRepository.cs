using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    using Infrastructure.Domain;

    public interface IUnitOfWorkRepository
    {
        void PersistCreationOf(IEntity entity, IDbTransaction transaction = null);
        void PersistUpdateOf(IEntity entity, IDbTransaction transaction = null);
        void PersistDeletionOf(IEntity entity, IDbTransaction transaction = null);
    }
}
