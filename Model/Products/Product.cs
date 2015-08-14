using System;

namespace Model.Products
{
    using Infrastructure.Domain;
    using Model.Categories;

    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
