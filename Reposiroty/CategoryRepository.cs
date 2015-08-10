using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiroty
{
    using Model;
    using Model.Base;
    using MongoDB.Bson;
    using Reposiroty.Base;

    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork uow) : base(uow) { }

        protected override string EntityName { get { return "Category"; } }

        protected override Category ConvertDocumentToEntity(object document)
        {
            var category = new Category();
            var mongoDocument = document as BsonDocument;
            category.Id = mongoDocument.ElementAt(0).Value.AsGuid;
            category.Name = mongoDocument.ElementAt(1).Value.AsString;
            return category;
        }
    }
}
