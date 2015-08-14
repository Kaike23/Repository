using System;

namespace Reposiroty
{
    using Infrastructure.UnitOfWork;
    using Model.Products;
    using Repository.Base;
    using Repository.Mapping.SQL.Base;

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork uow, IDataMapper<Product> mapper) : base(uow, mapper) { }

        protected override string TableName { get { return "Products"; } }

        protected override Product MapDataToEntity(object document)
        {
            throw new NotImplementedException();
        }
    }
}
