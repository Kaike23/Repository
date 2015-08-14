using System;

namespace Infrastructure.Domain
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
