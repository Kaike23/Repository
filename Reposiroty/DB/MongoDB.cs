using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DB
{
    using global::MongoDB.Bson;
    using global::MongoDB.Driver;
    using Model.Base;

    public class MongoDB : IDatabase
    {
        private string _dbName;
        private IMongoCollection<BsonDocument> _collection;

        private Dictionary<IMongoCollection<BsonDocument>, List<BsonDocument>> newDocuments;
        private Dictionary<EntityBase, UpdateDefinition<BsonDocument>> updatedDocuments;
        private List<EntityBase> deletedDocuments;

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public MongoDB(string name)
        {
            _dbName = name;
            _client = new MongoClient();
            _database = _client.GetDatabase(_dbName);
        }

        public void Add(IList<EntityBase> newEntities)
        {
            newDocuments = new Dictionary<IMongoCollection<BsonDocument>, List<BsonDocument>>();
            if (newEntities.Count == 0) return;
            foreach (var entity in newEntities)
            {
                var collection = GetCollection(entity);
                if (!newDocuments.ContainsKey(collection))
                {
                    newDocuments.Add(collection, new List<BsonDocument>());
                }
                newDocuments[collection].Add(CreateMongoDocument(entity));
            }
        }

        public void Update(IList<EntityBase> dirtyEntities)
        {
            updatedDocuments = new Dictionary<EntityBase, UpdateDefinition<BsonDocument>>();
            if (dirtyEntities.Count == 0) return;
            foreach (var entity in dirtyEntities)
            {
                var update = Builders<BsonDocument>.Update.Set("Name", ((Model.Category)entity).Name);
                updatedDocuments.Add(entity, update);
            }
        }

        public void Delete(IList<EntityBase> removedEntities)
        {
            deletedDocuments = new List<EntityBase>();
            if (removedEntities.Count == 0) return;
            foreach (var entity in removedEntities)
            {
                deletedDocuments.Add(entity);
            }
        }

        IEnumerable<object> IDatabase.FindAll(string entityName)
        {
            return FindAllDocuments(entityName).Result;
        }

        public void Commit()
        {
            Add();
            Update();
            Delete();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void BeginTransation()
        {
            throw new NotImplementedException();
        }

        public void EndTransation()
        {
            throw new NotImplementedException();
        }

        public async Task<List<BsonDocument>> FindAllDocuments(string entityName)
        {
            var collection = _database.GetCollection<BsonDocument>(entityName + "Collection");
            var filter = new BsonDocument();
            return await collection.Find(filter).ToListAsync();
        }

        private IMongoCollection<BsonDocument> GetCollection(EntityBase entity)
        {
            var collectionName = entity.GetType().Name + "Collection";
            if (_collection == null)
                _collection = _database.GetCollection<BsonDocument>(collectionName);
            return _collection.CollectionNamespace.Equals(collectionName) ? _collection : _database.GetCollection<BsonDocument>(collectionName);
        }

        private BsonDocument CreateMongoDocument(EntityBase entity)
        {
            return entity.ToBsonDocument(entity.GetType());
        }

        private async void Add()
        {
            foreach (var document in newDocuments)
            {
                await document.Key.InsertManyAsync(document.Value);
            }
        }

        private async void Update()
        {
            foreach (var document in updatedDocuments)
            {
                var collection = GetCollection(document.Key);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", document.Key.Id);
                var result = await collection.UpdateOneAsync(filter, document.Value);
            }
        }

        private async void Delete()
        {
            foreach(var document in deletedDocuments)
            {
                var collection = GetCollection(document);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", document.Id);
                var result = await collection.DeleteManyAsync(filter);
            }
        }
    }
}
