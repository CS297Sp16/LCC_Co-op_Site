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
        public DbSet<Company> Companies { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Instructions for SQL Server
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   //This is the Default Behavior unsure if we want or not
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();  //this is the Default Behavior
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Class Configurations            
            modelBuilder.Configurations.Add(new ETC.MajorConfiguration());
            modelBuilder.Configurations.Add(new ETC.OpportunityConfiguration());
            modelBuilder.Configurations.Add(new ETC.UserConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}