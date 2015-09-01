using System.Linq;

namespace Repository
{
    using Infrastructure.UnitOfWork;
    using Model.Categories;
    using MongoDB.Bson;
    using Repository.Base;
    using Repository.Mapping.SQL.Base;

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork uow, IDataMapper<Category> mapper) : base(uow, mapper) { }

        protected override string TableName { get { return "Categories"; } }

        //protected override Category MapDataToEntity(object document)
        //{
        //    var category = new Category();
        //    var mongoDocument = document as BsonDocument;
        //    category.Id = mongoDocument.ElementAt(0).Value.AsGuid;
        //    category.Name = mongoDocument.ElementAt(1).Value.AsString;
        //    return category;
        //}

        protected override Category MapDataToEntity(object document)
        {
            throw new System.NotImplementedException();
        }
    }
}
