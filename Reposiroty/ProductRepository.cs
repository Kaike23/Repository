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

    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(IUnitOfWork uow) : base(uow) { }

        protected override string EntityName { get { return "Product"; } }

        protected override Product ConvertDocumentToEntity(object document)
        {
            throw new NotImplementedException();
        }
    }
}
