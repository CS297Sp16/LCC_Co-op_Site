using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coop_Listing_Site.DAL
{
    interface IMajorRepo : IDisposable
    {
        IEnumerable<Major> GetAll();

        Major GetByID(int id);

        Major Add(Major major);

        Major Update(Major major);

        Major Delete(Major major);
    }
}
