using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Reposiroty.UnitOfWork
{
    using Model;
    using Model.Base;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Repository.DB;

    public class UnitOfWork : IUnitOfWork
    {
        protected static IDatabase _database;

        private List<EntityBase> newEntities = new List<EntityBase>();
        private List<EntityBase> dirtyEntities = new List<EntityBase>();
        private List<EntityBase> removedEntities = new List<EntityBase>();

        public UnitOfWork(IDatabase database)
        {
            _database = database;
        }

        public void RegisterNew(EntityBase entity)
        {
            Debug.Assert(entity != null);
            Debug.Assert(!dirtyEntities.Contains(entity), "object dirty");
            Debug.Assert(!removedEntities.Contains(entity), "object removed");
            Debug.Assert(!newEntities.Contains(entity), "object already registered new");
            newEntities.Add(entity);
        }

        public void RegisterDirty(EntityBase entity)
        {
            Debug.Assert(entity.Id != null, "id null");
            Debug.Assert(!removedEntities.Contains(entity), "object removed");
            if (!dirtyEntities.Contains(entity) && !newEntities.Contains(entity))
            {
                dirtyEntities.Add(entity);
            }
        }

        public void RegisterRemoved(EntityBase entity)
        {
            Debug.Assert(entity.Id != null, "id null");
            if (newEntities.Remove(entity)) return;
            dirtyEntities.Remove(entity);
            if (!removedEntities.Contains(entity))
            {
                removedEntities.Add(entity);
            }
        }

        public void RegisterClean(EntityBase entity)
        {
            Debug.Assert(entity.Id != null, "id null");
        }

        public void Commit()
        {
            //using (_database.BeginTransaction())
            try
            {
                _database.Add(newEntities);
                _database.Update(dirtyEntities);
                _database.Delete(removedEntities);
                _database.Commit();
            }
            catch
            {
                //_database.Rollback();
            }
            finally
            {
                newEntities.Clear();
                dirtyEntities.Clear();
                removedEntities.Clear();
            }
        }

        public IDatabase Database { get { return _database; } }
    }
}
