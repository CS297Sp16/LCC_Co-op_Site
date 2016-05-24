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
        public CoopContext() : base("CoopContext", false)
        {
            //CoopContext Constructor Will eventually plug the Database Initializer here
        }
        
        public DbSet<Department> Departments { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<StudentInfo> Students { get; set; }
        public DbSet<CoordinatorInfo> Coordinators { get; set; }
        public DbSet<RegisterInvite> Invites { get; set; }
        public DbSet<EmailInfo> Emails { get; set; }
        public DbSet<PasswordReset> ResetTokens { get; set; }
        public DbSet<Application> Applications { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Instructions for SQL Server
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   //This is the Default Behavior unsure if we want or not
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();  //this is the Default Behavior unsure
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Class Configurations for the Database Entities
            /*modelBuilder.Configurations.Add(new ETC.CourseConfiguration());
            modelBuilder.Configurations.Add(new ETC.DepartmentConfiguration());
            modelBuilder.Configurations.Add(new ETC.MajorConfiguration());
            modelBuilder.Configurations.Add(new ETC.OpportunityConfiguration());
            modelBuilder.Configurations.Add(new ETC.UserConfiguration());*/

            // testing a fix for scaffolding problem from stackoverflow
            modelBuilder.Entity<Department>();
            modelBuilder.Entity<Major>();
            modelBuilder.Entity<Opportunity>();
            modelBuilder.Entity<StudentInfo>();
            modelBuilder.Entity<CoordinatorInfo>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
