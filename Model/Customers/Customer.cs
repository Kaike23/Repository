using System;
using System.Collections.Generic;

namespace Model.Customers
{
    using Infrastructure.Domain;
    using Infrastructure.Lock;
    using Model.Addresses;
    using Model.Base;

    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Address> Addresses { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public Customer(Guid id, long? versionId, string firstName, string lastName)
            : base(id, versionId)
        {
            FirstName = firstName;
            LastName = lastName;
            Addresses = new List<Address>();
        }

        public static Customer Create(string firstName, string lastName)
        {
            var customer = new Customer(Guid.NewGuid(), null, firstName, lastName);
            return customer;
        }

        public Address AddAddress(string street, string city, string country, int zipCode)
        {
            var address = Address.Create(Id, VersionLock, street, city, country, zipCode);
            Addresses.Add(address);
            return address;
        }
    }
}
