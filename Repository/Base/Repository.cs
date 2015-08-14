using System;
using System.Collections.Generic;

namespace Repository.Base
{
    using System.Data.SqlClient;
    using Infrastructure.Domain;
    using Infrastructure.UnitOfWork;
    using Mapping.SQL.Base;
    using Model.Base;
    using Model.Categories;

    public abstract class Repository<T> : IRepository<T>, IUnitOfWorkRepository
        where T : IEntity
    {
        private IUnitOfWork _uow;

        public Repository(IUnitOfWork uow, IDataMapper<T> dataMapper)
        {
            _uow = uow;
            DataMapper = dataMapper;
        }

        #region IRepository

        public void Add(T entity)
        {
            _uow.RegisterNew(entity, this);
        }
        public void Update(T entity)
        {
            _uow.RegisterDirty(entity, this);
        }

        public void Remove(T entity)
        {
            _uow.RegisterRemoved(entity, this);
        }
        public T FindBy(Guid id)
        {
            return DataMapper.Find(id);
        }
        public IEnumerable<T> FindAll()
        {
            return DataMapper.FindMany(new FindAllStatement(TableName));

            //var documents = _uow.Database.FindAll(EntityName);
            //var entities = new List<T>();
            //foreach (var document in documents)
            //    entities.Add(MapDataToEntity(document));

            //return entities;
        }

        #endregion

        public IDataMapper<T> DataMapper { get; set; }

        protected abstract T MapDataToEntity(object document);

        protected abstract string TableName { get; }

        private class FindAllStatement : IStatementSource
        {
            private static string _tableName;
            public FindAllStatement(string tableName)
            {
                _tableName = tableName;
            }
            public List<SqlParameter> Parameters { get { return new List<SqlParameter>(); } }
            public string Sql { get { return string.Format("SELECT * FROM {0}", _tableName); } }
        }

        #region IUnitOfWorkRepository

        void IUnitOfWorkRepository.PersistCreationOf(IEntity entity) { DataMapper.Insert((T)entity); }
        void IUnitOfWorkRepository.PersistUpdateOf(IEntity entity) { DataMapper.Update((T)entity); }
        void IUnitOfWorkRepository.PersistDeletionOf(IEntity entity) { DataMapper.Delete((T)entity); }

        #endregion
    }
}
