using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public interface IReadOnlyRepository<T, TId> where T : EntityBase
    {
        T FindBy(TId id);
        IEnumerable<T> FindAll();
    }
}
