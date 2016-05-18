using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coop_Listing_Site.DAL
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll();

        T GetByID(object id);

        T Add(T dbObj);

        T Update(T dbObj);

        T Delete(T dbObj);
    }
}
