using System;

namespace Model.Categories
{
    using Infrastructure.Domain;

    public class Category : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}