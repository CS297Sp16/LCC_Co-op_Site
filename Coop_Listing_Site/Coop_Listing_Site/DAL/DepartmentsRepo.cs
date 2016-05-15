using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Coop_Listing_Site.DAL
{
    public class DepartmentsRepo : IRepository<Department>
    {
        private CoopContext db;

        public DepartmentsRepo()
        {
            db = new CoopContext();
        }

        public DepartmentsRepo(CoopContext context)
        {
            db = context;
        }

        public Department Add(Department dept)
        {
            db.Departments.Add(dept);
            db.SaveChanges();

            return dept;
        }

        public Department Delete(Department dept)
        {
            db.Departments.Remove(dept);
            db.SaveChanges();
            return dept;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Department> GetAll()
        {
            var dept = db.Departments.Include(d => d.Majors);
            return dept.ToList();
        }

        public Department GetByID(int? id)
        {
            db.Majors.Load();
            return db.Departments.Find(id);
        }

        public Department Update(Department dept)
        {
            db.Entry(dept).State = EntityState.Modified;
            db.SaveChanges();

            return dept;
        }
    }
}