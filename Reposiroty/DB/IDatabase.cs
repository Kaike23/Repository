using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DB
{
    using Model.Base;

    public interface IDatabase
    {
        void Add(IList<EntityBase> newEntities);
        void Update(IList<EntityBase> dirtyEntities);
        void Delete(IList<EntityBase> removedEntities);
        IEnumerable<object> FindAll(string entityName);
        void Commit();
        void Rollback();
        void BeginTransation();
        void EndTransation();
    }
}
