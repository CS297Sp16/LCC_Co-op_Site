using System;
using System.Collections.Generic;
using System.Linq;
using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;

namespace UnitTests
{
    class TestRepository : IRepository
    {
        private Dictionary<Type, List<object>> fakeDB = new Dictionary<Type, List<object>>();

        public TestRepository()
        {
            fakeDB[typeof(Major)] = new List<object>();
            fakeDB[typeof(Department)] = new List<object>();
            fakeDB[typeof(StudentInfo)] = new List<object>();
        }

        public T Add<T>(T dbObj) where T : class
        {
            fakeDB[typeof(T)].Add(dbObj);

            return dbObj;
        }

        public T Delete<T>(T dbObj) where T : class
        {
            fakeDB[typeof(T)].Remove(dbObj);

            return dbObj;
        }

        public void Dispose()
        {
            // this doesn't need to do anything, but why not.
            foreach (var l in fakeDB)
                l.Value.Clear();

            fakeDB = null;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            var table = fakeDB[typeof(T)];

            return table.Cast<T>();
        }

        public T GetByID<T>(object id) where T : class
        {
            var table = fakeDB[typeof(T)];

            var entry = table.Cast<T>().SingleOrDefault(o => isKeyMatch(o, id));

            return entry;
        }

        public T GetOne<T>() where T : class
        {
            return fakeDB[typeof(T)].Cast<T>().FirstOrDefault();
        }

        public T GetOne<T>(Func<T, bool> check) where T : class
        {
            var table = fakeDB[typeof(T)].Cast<T>();

            return table.SingleOrDefault(check);
        }

        public IEnumerable<T> GetWhere<T>(Func<T, bool> check) where T : class
        {
            var table = fakeDB[typeof(T)].Cast<T>();

            return table.Where(check);
        }

        public T Update<T>(T dbObj) where T : class
        {
            //not sure if anything should be done here
            return dbObj;
        }

        private bool isKeyMatch<T>(T o, object id) where T : class
        {
            // add 'id' to the end of the class name for our guess
            string idFieldGuess = typeof(T).Name + "id";
            object key = null;

            // loop over the objects properties
            foreach (var i in o.GetType().GetProperties())
            {
                // if the property matches our guess, get its value and stop looking
                if (string.Equals(i.Name, idFieldGuess, StringComparison.CurrentCultureIgnoreCase))
                {
                    key = i.GetValue(o);
                    break;
                }
                // if it doesn't match our guess, see if it has any attributes
                else if (i.CustomAttributes.Count() > 0)
                {
                    foreach (var att in i.CustomAttributes)
                    {
                        // if it has a Key attribute, get its value and stop looking
                        if (att.AttributeType.Name == "KeyAttribute")
                        {
                            key = i.GetValue(o);
                            break;
                        }
                    }
                    // stop checking properties if we've found a key
                    if (key != null) break;
                }
            }

            if (key.GetType() == id.GetType())
            {
                if (id.Equals(key)) return true;
            }

            return false;
        }
    }
}
