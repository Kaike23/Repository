using System;

namespace Model.Orders
{
    using Infrastructure.Domain;
    using Infrastructure.Lock;
    using Model.Base;

    public class Order : BaseEntity
    {
        public Order(Guid id) : base(id) { }
    }
}
