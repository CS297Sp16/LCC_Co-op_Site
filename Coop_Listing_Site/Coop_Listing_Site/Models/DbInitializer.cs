using Coop_Listing_Site.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<CoopContext>
    {
        protected override void Seed(CoopContext context)
        {
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

            // add Users
            User user1 = new User {
                UserName = "test@test.com",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Testman"
            };
            User user2 = new User {
                UserName = "thinger@test.com",
                Email = "thinger@test.com",
                FirstName = "Jon",
                LastName = "Doe"
            };

            // create users
            var result1 = userManager.Create(user1, "password1");
            var result2 = userManager.Create(user2, "password1");

            if (!result1.Succeeded || !result2.Succeeded)
                throw new Exception("Account creation in seed failed");

            user1 = userManager.FindByName("test@test.com");
            user2 = userManager.FindByName("thinger@test.com");

            Major major = new Major { MajorName = "Testing" };
            Department dept = new Department { DepartmentName = "Test Dept" };
            dept.Majors.Add(major);

            context.Majors.Add(major);
            context.Departments.Add(dept);

            // save so our new objects have IDs
            context.SaveChanges();
            
            // add some info
            StudentInfo sInfo1 = new StudentInfo {
                LNumber = "L00000001",
                UserId = user1.Id,
                MajorID = major.MajorID
            };
            StudentInfo sInfo2 = new StudentInfo {
                LNumber = "L00000002",
                UserId = user2.Id,
                MajorID = major.MajorID
            };

            // test opportunities
            Opportunity opp1 = new Opportunity
            {
                DepartmentID = dept.DepartmentID,
                OpeningsAvailable = 1,
                TermAvailable = "Fall"
            };
            Opportunity opp2 = new Opportunity
            {
                DepartmentID = dept.DepartmentID,
                OpeningsAvailable = 5,
                TermAvailable = "Spring"
            };

            context.Students.Add(sInfo1);
            context.Students.Add(sInfo2);

            context.Opportunities.Add(opp1);
            context.Opportunities.Add(opp2);
            
            context.SaveChanges();

            base.Seed(context);
        }
    }
}