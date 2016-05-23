using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Coop_Listing_Site.DAL
{
    /// <summary>
    /// A painfully thin layer of abstraction over the dbContext
    /// </summary>
    public class Repository
    {
        private CoopContext db;

        public Repository()
        {
            db = new CoopContext();
        }

        public Repository(CoopContext context)
        {
            db = context;
        }


        public T Add<T>(T dbObj) where T : class
        {
            db.Set<T>().Add(dbObj);
            db.SaveChanges();

            return dbObj;
        }

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

        public IEnumerable<T> GetAll<T>() where T : class
        {
            var results = db.Set<T>();
            return results.ToList();
        }

        public IEnumerable<T> GetWhere<T>(Func<T, bool> check) where T : class
        {
            var results = db.Set<T>().Where(check);
            return results.ToList();
        }

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

        // now totally usless, yay!
        private void LoadRefs(Type type)
        {
            // get the types properties
            var properties = type.GetProperties();
            // list of db types to load
            List<Type> props = new List<Type>();

            foreach (var prop in properties)
            {
                // if it's one of our models, add it to the list
                if (prop.PropertyType.IsClass &&
                    prop.PropertyType.Namespace == "Coop_Listing_Site.Models")
                {
                    props.Add(prop.PropertyType);
                }
            }

            // load all the dbSets in the list
            foreach (var t in props)
                db.Set(t).Load();
        }
    }
}