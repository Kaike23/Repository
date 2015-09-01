using System;

namespace Model.Categories
{
    using Infrastructure.Domain;
    using Infrastructure.Lock;
    using Model.Base;

    public class Category : BaseEntity
    {
        public Category(Guid id) : base(id) { }

        public string Name { get; set; }
    }
}