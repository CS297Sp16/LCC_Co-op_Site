using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Coop_Listing_Site.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;

namespace Coop_Listing_Site.DAL
{
    public class CoopContext : IdentityDbContext<User>
    {
        public CoopContext() : base("CoopContext")
        {
            //CoopContext Constructor
        }

        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Major> Majors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }
    }
}