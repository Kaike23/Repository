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

    public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork uow) : base(uow) { }

        protected override string EntityName { get { return "Customer"; } }

        protected override Customer ConvertDocumentToEntity(object document)
        {
            throw new NotImplementedException();
        }
    }
}
