using System;

namespace Model.Orders
{
    using Infrastructure.Domain;

    public class Order : IEntity
    {
        public Guid Id { get; set; }
    }
}
