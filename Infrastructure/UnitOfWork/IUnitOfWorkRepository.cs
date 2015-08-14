using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    using Infrastructure.Domain;

    public interface IUnitOfWorkRepository
    {
        void PersistCreationOf(IEntity entity);
        void PersistUpdateOf(IEntity entity);
        void PersistDeletionOf(IEntity entity);
    }
}
