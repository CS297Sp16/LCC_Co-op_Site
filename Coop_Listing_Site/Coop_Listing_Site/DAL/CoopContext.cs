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
            //CoopContext Constructor Will eventually plug the Database Initializer here
        }


        public DbSet<Company> Companies { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<StudentInfo> Students { get; set; }
        public DbSet<CoordinatorInfo> Coordinators { get; set; }

        /*
         * Potential issues with User inheritance
         * Comment out for now
        public DbSet<Student> Students { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        */


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Instructions for SQL Server
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   //This is the Default Behavior unsure if we want or not
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();  //this is the Default Behavior unsure 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Class Configurations for the Database Entities
            modelBuilder.Configurations.Add(new ETC.CompanyConfiguration());
            modelBuilder.Configurations.Add(new ETC.CourseConfiguration());
            modelBuilder.Configurations.Add(new ETC.DepartmentConfiguration());
            modelBuilder.Configurations.Add(new ETC.MajorConfiguration());
            modelBuilder.Configurations.Add(new ETC.OpportunityConfiguration());
            modelBuilder.Configurations.Add(new ETC.UserConfiguration());

            // modelBuilder.Configurations.Add(new ETC.UserConfiguration()); // Duplicate??
            


            base.OnModelCreating(modelBuilder);
        }
    }
}