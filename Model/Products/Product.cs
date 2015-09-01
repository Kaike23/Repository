using System;

namespace Model.Products
{
    using Infrastructure.Domain;
    using Infrastructure.Lock;
    using Model.Base;
    using Model.Categories;

    public class Product : BaseEntity
    {
        public Product(Guid id) : base(id) { }

        public Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
