using System;
using System.Collections.Generic;
using System.Data;

namespace Repository.Mapping.SQL.Base
{
    using Infrastructure.Database;
    using Infrastructure.Domain;
    using Model.Base;

    public interface IDataMapper<T> where T : BaseEntity
    {
        T Find(Guid Id);
        List<T> FindMany(IStatementSource source);

        Guid Insert(T entity, IDbTransaction transaction = null);
        int Update(T entity, IDbTransaction transaction = null);
        void Delete(T entity, IDbTransaction transaction = null);
    }
}
