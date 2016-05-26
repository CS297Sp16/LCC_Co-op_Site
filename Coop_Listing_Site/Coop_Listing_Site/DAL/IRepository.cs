using System;
using System.Collections.Generic;

namespace Coop_Listing_Site.DAL
{
    public interface IRepository : IDisposable
    {
        T Add<T>(T dbObj) where T : class;
        T Delete<T>(T dbObj) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        T GetByID<T>(object id) where T : class;
        T GetOne<T>() where T : class;
        T GetOne<T>(Func<T, bool> check) where T : class;
        IEnumerable<T> GetWhere<T>(Func<T, bool> check) where T : class;
        T Update<T>(T dbObj) where T : class;
    }
}