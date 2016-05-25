using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Coop_Listing_Site.DAL
{
    /// <summary>
    /// A painfully thin layer of abstraction over the dbContext
    /// </summary>
    public class Repository : IRepository
    {
        private CoopContext db;

        public Repository()
        {
            db = new CoopContext();
        }

        /// <summary>
        /// Creates a Repository using the provided dbContext
        /// </summary>
        /// <param name="context">The dbContext that the Repository will use</param>
        public Repository(CoopContext context)
        {
            db = context;
        }


        /// <summary>
        /// Adds a new entry to the database
        /// </summary>
        public T Add<T>(T dbObj) where T : class
        {
            db.Set<T>().Add(dbObj);
            db.SaveChanges();

            return dbObj;
        }

        /// <summary>
        /// Removes an entry from the database
        /// </summary>
        public T Delete<T>(T dbObj) where T : class
        {
            db.Set<T>().Remove(dbObj);
            db.SaveChanges();

            return dbObj;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        /// <summary>
        /// Returns all entries of the specified type
        /// </summary>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            var results = db.Set<T>();
            return results.ToList();
        }

        /// <summary>
        /// Returns all entries matched by the check function
        /// </summary>
        /// <param name="check">A function that returns true for entries that
        /// should be included in the returned list</param>
        public IEnumerable<T> GetWhere<T>(Func<T, bool> check) where T : class
        {
            var results = db.Set<T>().Where(check);
            return results.ToList();
        }

        /// <summary>
        /// Returns the first item of the supplied type, or null if there are none
        /// </summary>
        public T GetOne<T>() where T : class
        {
            return db.Set<T>().FirstOrDefault();
        }

        public T GetOne<T>(Func<T, bool> check) where T : class
        {
            return db.Set<T>().SingleOrDefault(check);
        }

        public T GetByID<T>(object id) where T : class
        {
            return db.Set<T>().Find(id);
        }

        public T Update<T>(T dbObj) where T : class
        {
            db.Entry(dbObj).State = EntityState.Modified;
            db.SaveChanges();

            return dbObj;
        }
    }
}