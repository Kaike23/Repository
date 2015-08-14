using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    using Infrastructure.Domain;

    public interface IAsyncDatabase
    {
        Task AddAsync(IEntity entity);
        Task UpdateAsync(IEntity entity);
        Task DeleteAsync(IEntity entity);
        Task<IEnumerable<object>> FindAllAsync(string entityName);
    }
}
