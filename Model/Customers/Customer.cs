using System;

namespace Model.Customers
{
    using Infrastructure.Domain;
    using Model.Addresses;

    public class Customer : IEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
    }
}
