using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Coop_Listing_Site.Models;

namespace Coop_Listing_Site.DAL
{
    public class MajorsRepo : IRepository<Major>
    {
        private CoopContext db;

        public MajorsRepo()
        {
            db = new CoopContext();
        }

        public MajorsRepo(CoopContext context)
        {
            db = context;
        }

        public Major Add(Major major)
        {
            db.Majors.Add(major);
            db.SaveChanges();

            return major;
        }

        public Major Delete(Major major)
        {
            db.Majors.Remove(major);
            db.SaveChanges();
            return major;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Major> GetAll()
        {
            var majors = db.Majors.Include(m => m.Department);
            return majors.ToList();
        }

        public Major GetByID(object id)
        {
            db.Departments.Load();
            return db.Majors.Find(id);
        }

        public Major Update(Major major)
        {
            db.Entry(major).State = EntityState.Modified;
            db.SaveChanges();

            return major;
        }
    }
}