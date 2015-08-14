namespace Infrastructure.UnitOfWork
{
    using Infrastructure.Database;
    using Infrastructure.Domain;

    public interface IUnitOfWork
    {
        IDatabase Database { get; }
        void RegisterNew(IEntity entity, IUnitOfWorkRepository unitofWorkRepository);
        void RegisterDirty(IEntity entity, IUnitOfWorkRepository unitofWorkRepository);
        void RegisterRemoved(IEntity entity, IUnitOfWorkRepository unitofWorkRepository);
        void Commit();
    }
}
