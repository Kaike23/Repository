using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    using Model.Base;

    public interface ICategoryRepository : IRepository<Category, Guid>
    {
    }
}
