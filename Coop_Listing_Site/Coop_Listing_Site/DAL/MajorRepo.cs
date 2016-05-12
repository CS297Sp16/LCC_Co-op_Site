using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop_Listing_Site.Models;

namespace Coop_Listing_Site.DAL
{
    public class MajorRepo : IMajorRepo
    {
        private CoopContext db;

        public MajorRepo()
        {
            db = new CoopContext();
        }

        public MajorRepo(CoopContext context)
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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Major> GetAll()
        {
            throw new NotImplementedException();
        }

        public Major GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Major Update(Major major)
        {
            throw new NotImplementedException();
        }
    }
}