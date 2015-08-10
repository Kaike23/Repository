using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiroty.Base
{
    using Model;
    using Model.Base;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public abstract class Repository<T, TEntityKey> where T : EntityBase
    {
        private IUnitOfWork _uow;
        private List<T> entities;
        public Repository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(T entity)
        {
            _uow.RegisterNew(entity);
        }
        public void Update(T entity)
        {
            _uow.RegisterDirty(entity);
        }

        public void Remove(T entity)
        {
            _uow.RegisterRemoved(entity);
        }
        public T FindBy(TEntityKey id)
        {
            //TODO. Get specific entity from DB in CollectionName
            throw new NotImplementedException();
        }
        public IEnumerable<T> FindAll()
        {
            var documents = _uow.Database.FindAll(EntityName);
            var entities = new List<T>();
            foreach (var document in documents)
                entities.Add(ConvertDocumentToEntity(document));

            return entities;
        }
      
        protected abstract T ConvertDocumentToEntity(object document);
        protected abstract string EntityName { get; }
    }
}
