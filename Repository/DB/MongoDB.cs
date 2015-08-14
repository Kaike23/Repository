using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.DB
{
    using global::MongoDB.Bson;
    using global::MongoDB.Driver;
    using Infrastructure.Database;
    using Infrastructure.Domain;

    public class MongoDB : IAsyncDatabase
    {
        private string _dbName;
        private IMongoCollection<BsonDocument> _collection;

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public MongoDB(string name)
        {
            _dbName = name;
            _client = new MongoClient();
            _database = _client.GetDatabase(_dbName);
        }

        public Task AddAsync(IEntity entity)
        {
            var collection = GetCollection(entity);
            var document = CreateMongoDocument(entity);
            return collection.InsertOneAsync(document);
        }

        public Task UpdateAsync(IEntity entity)
        {
            var collection = GetCollection(entity);
            var update = Builders<BsonDocument>.Update.Set("Name", ((Model.Categories.Category)entity).Name);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);
            return collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(IEntity entity)
        {
            var collection = GetCollection(entity);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);
            return collection.DeleteManyAsync(filter);
        }

        async Task<IEnumerable<object>> IAsyncDatabase.FindAllAsync(string entityName)
        {
            var collection = _database.GetCollection<BsonDocument>(entityName + "Collection");
            var filter = new BsonDocument();
            return await collection.Find(filter).ToListAsync();
        }

        private IMongoCollection<BsonDocument> GetCollection(IEntity entity)
        {
            var collectionName = entity.GetType().Name + "Collection";
            if (_collection == null)
                _collection = _database.GetCollection<BsonDocument>(collectionName);
            return _collection.CollectionNamespace.Equals(collectionName) ? _collection : _database.GetCollection<BsonDocument>(collectionName);
        }

        private BsonDocument CreateMongoDocument(IEntity entity)
        {
            return entity.ToBsonDocument(entity.GetType());
        }
    }
}
