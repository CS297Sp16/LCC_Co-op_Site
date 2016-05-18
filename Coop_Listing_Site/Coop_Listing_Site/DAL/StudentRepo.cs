using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Coop_Listing_Site.DAL
{
    public class StudentRepo : IRepository<StudentInfo>
    {
        private CoopContext db;

        public StudentRepo()
        {
            db = new CoopContext();
        }

        public StudentRepo(CoopContext context)
        {
            db = context;
        }

        public StudentInfo Add(StudentInfo sInfo)
        {
            db.Students.Add(sInfo);
            db.SaveChanges();

            return sInfo;
        }

        public StudentInfo Delete(StudentInfo sInfo)
        {
            db.Students.Remove(sInfo);
            db.SaveChanges();
            return sInfo;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<StudentInfo> GetAll()
        {
            var sInfo = db.Students.Include(s => s.Major).Include(s => s.User);
            return sInfo.ToList();
        }

        public StudentInfo GetByID(object id)
        {
            db.Majors.Load();
            db.Users.Load();
            return db.Students.Find(id);
        }

        public StudentInfo Update(StudentInfo sInfo)
        {
            db.Entry(sInfo).State = EntityState.Modified;
            db.SaveChanges();

            return sInfo;
        }
    }
}