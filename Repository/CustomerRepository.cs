using System;

namespace Repository
{
    using Base;
    using Infrastructure.UnitOfWork;
    using Model.Customers;
    using Repository.Mapping.SQL.Base;

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork uow, IDataMapper<Customer> mapper) : base(uow, mapper) { }

        protected override string TableName { get { return "Customers"; } }

        protected override Customer MapDataToEntity(object document)
        {
            throw new NotImplementedException();
        }
    }
}
