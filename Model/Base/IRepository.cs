using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public interface IRepository<T, TId> : IReadOnlyRepository<T, TId>
        where T : EntityBase
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
