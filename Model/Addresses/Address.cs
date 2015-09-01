using System;

namespace Model.Addresses
{
    using Infrastructure.Domain;
    using Infrastructure.Lock;
    using Model.Base;

    public class Address : BaseEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int ZipCode { get; set; }
        public Guid CustomerId { get; set; }

        private Address(Guid id, VersionLock version, Guid customerId, string street, string city, string country, int zipCode)
            : base(id)
        {
            CustomerId = customerId;
            Street = street;
            City = city;
            ZipCode = zipCode;
            this.SetSystemFields(version, DateTime.Now);
        }

        public static Address Create(Guid customerId, VersionLock version, string street, string city, string country, int zipCode)
        {
            return new Address(Guid.NewGuid(), version, customerId, street, city, country, zipCode);
        }
    }
}