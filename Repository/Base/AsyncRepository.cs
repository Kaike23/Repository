using System;
using System.Collections.Generic;

namespace Repository.Base
{
    using Infrastructure.Database;
    using Infrastructure.Domain;

    public abstract class AsyncRepository<T, TEntityKey> where T : IEntity
    {
        private IAsyncDatabase _database;
        public AsyncRepository(IAsyncDatabase database)
        {
            _database = database;
        }

        public async void AddAsync(T entity)
        {
            await _database.AddAsync(entity);
        }
        public async void UpdateAsync(T entity)
        {
            await _database.UpdateAsync(entity);
        }

        public async void RemoveAsync(T entity)
        {
            await _database.DeleteAsync(entity);
        }
        public T FindBy(TEntityKey id)
        {
            //TODO. Get specific entity from DB in CollectionName
            throw new NotImplementedException();
        }
        public IEnumerable<T> FindAll()
        {
            var dataResult = _database.FindAllAsync(EntityName).Result;
            var entities = new List<T>();
            foreach(var record in dataResult)
            {
                entities.Add(MapDataToEntity(record));
            }
            return entities;
        }

        protected abstract T MapDataToEntity(object document);
        protected abstract string EntityName { get; }
    }
}
