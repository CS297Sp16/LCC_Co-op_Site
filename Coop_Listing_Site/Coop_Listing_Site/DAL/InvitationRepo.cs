using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop_Listing_Site.Models;
using System.Data.Entity;

namespace Coop_Listing_Site.DAL
{
    public class InvitationRepo : IRepository<RegisterInvite>
    {
        private CoopContext db;

        public InvitationRepo()
        {
            db = new CoopContext();
        }

        public InvitationRepo(CoopContext context)
        {
            db = context;
        }

        public RegisterInvite Add(RegisterInvite inv)
        {
            db.Invites.Add(inv);
            db.SaveChanges();

            return inv;
        }

        public RegisterInvite Delete(RegisterInvite inv)
        {
            db.Invites.Remove(inv);
            db.SaveChanges();
            return inv;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<RegisterInvite> GetAll()
        {
            return db.Invites.Where(i => i.UserType == RegisterInvite.AccountType.Student).ToList();
        }

        public RegisterInvite GetByID(object id)
        {
            return db.Invites.Find(id);
        }

        public RegisterInvite Update(RegisterInvite inv)
        {
            db.Entry(inv).State = EntityState.Modified;
            db.SaveChanges();

            return inv;
        }
    }
}