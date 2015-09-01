using System;

namespace Infrastructure.Domain
{
    using Infrastructure.Lock;

    public interface IEntity
    {
        Guid Id { get; }
        DateTime Modified { get; }
        string ModifiedBy { get; }
        VersionLock VersionLock { get; }
    }
}
