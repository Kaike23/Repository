using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiroty
{
    using Model.Base;
    using Repository.DB;

    public interface IUnitOfWork
    {
        IDatabase Database { get; }
        void RegisterNew(EntityBase entity);
        void RegisterDirty(EntityBase entity);
        void RegisterRemoved(EntityBase entity);
        void RegisterClean(EntityBase entity);
        void Commit();
    }
}
