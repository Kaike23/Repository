using System;
using System.Collections.Generic;

namespace Repository.Mapping.SQL.Base
{
    using Infrastructure.Database;
    using Infrastructure.Domain;

    public interface IDataMapper<T> where T : IEntity
    {
        T Find(Guid Id);
        List<T> FindMany(IStatementSource source);

        Guid Insert(T entity);
        int Update(T entity);
        int Delete(T entity);

        //TEMP
        IDatabase Database { get; set; }
    }
}
