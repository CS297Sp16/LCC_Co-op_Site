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

            //Create roles
            context.Roles.Add(new IdentityRole() { Name = "Student" });
            context.Roles.Add(new IdentityRole() { Name = "Coordinator" });
            context.Roles.Add(new IdentityRole() { Name = "Admin" });

            // add Users
            User user1 = new User
            {
                UserName = "test@test.com",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Testman",
                Enabled = true
            };

            User user2 = new User
            {
                UserName = "thinger@test.com",
                Email = "thinger@test.com",
                FirstName = "Jon",
                LastName = "Doe",
                Enabled = true
            };

            User user3 = new User
            {
                UserName = "testcoord@test.com",
                Email = "testcoord@test.com",
                FirstName = "John",
                LastName = "Smith",
                Enabled = true
            };

            // create users
            var result1 = userManager.Create(user1, "password1");
            var result2 = userManager.Create(user2, "password1");
            var result3 = userManager.Create(user3, "password1");

            if (!result1.Succeeded || !result2.Succeeded || !result3.Succeeded)
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
            StudentInfo sInfo1 = new StudentInfo
            {
                LNumber = "L00000001",
                UserId = user1.Id,
                MajorID = major.MajorID
            };

            StudentInfo sInfo2 = new StudentInfo
            {
                LNumber = "L00000002",
                UserId = user2.Id,
                MajorID = major.MajorID
            };

            CoordinatorInfo cInfo1 = new CoordinatorInfo
            {
                UserId = user3.Id
            };

            cInfo1.Departments.Add(dept);

            // Add the student role to them
            userManager.AddToRole(user1.Id, "Student");
            userManager.AddToRole(user2.Id, "Student");
            userManager.AddToRoles(user3.Id, "Coordinator", "Admin");


            // test opportunities
            Opportunity opp1 = new Opportunity
            {
                CompanyName = "gabe's Grotto",
                ContactName = "Gabe Griffin",
                ContactNumber = "(541)914-2988",
                ContactEmail = "griffin.gabe@gmail.com",
                Location = "1800 W. 11th Ave. Eugene, OR 97404",
                CompanyWebsite = "http://GabesGrotto.com",
                AboutCompany = "This is for a test I don't know squat about Gabe's Grotto",
                AboutDepartment = "The newbie room",
                CoopPositionTitle = "Office lacky",
                CoopPositionDuties = "Anything your superiors decide to let you do besides get their coffee.",
                Qualifications = "Second year Programming major, Knowledge of C#, PHP, SQL are most helpful. Being able to type 40 WPS is also necessary.",
                GPA = 3.0,
                Paid = true,
                Duration = "one month",
                OpeningsAvailable = 1,
                TermAvailable = "Fall",
                DepartmentID = dept.DepartmentID
            };

            Opportunity opp2 = new Opportunity
            {
                CompanyName = "Big Al's House of Computers",
                ContactName = "Ron Jeremy",
                ContactNumber = "(541)913-3434",
                ContactEmail = "Ron@aol.com",
                Location = "2340 8th Ave. Eugene, OR 97404",
                CompanyWebsite = "http://BigAlsHouse.com",
                AboutCompany = "This is for a test I don't know squat about Big Al's house of computers",
                AboutDepartment = "Software Design",
                CoopPositionTitle = "developer 1",
                CoopPositionDuties = "Anything your superiors decide to let you do besides get their coffee.",
                Qualifications = "Second year Programming major, Knowledge of C#, PHP, SQL are most helpful. Being able to type 40 WPS is also necessary.",
                GPA = 3.5,
                Paid = true,
                Duration = "three months",
                OpeningsAvailable = 5,
                TermAvailable = "Spring",
                DepartmentID = dept.DepartmentID
            };

            // invites
            var inv1 = new RegisterInvite
            {
                RegisterInviteID = Guid.NewGuid().ToString("N"),
                Email = "new1@test.com",
                UserType = RegisterInvite.AccountType.Student
            };

            var inv2 = new RegisterInvite
            {
                RegisterInviteID = Guid.NewGuid().ToString("N"),
                Email = "new2@test.com",
                UserType = RegisterInvite.AccountType.Student
            };

            var inv3 = new RegisterInvite
            {
                RegisterInviteID = Guid.NewGuid().ToString("N"),
                Email = "new3@test.com",
                UserType = RegisterInvite.AccountType.Coordinator
            };

            context.Invites.Add(inv1);
            context.Invites.Add(inv2);
            context.Invites.Add(inv3);

            context.Students.Add(sInfo1);
            context.Students.Add(sInfo2);
            context.Coordinators.Add(cInfo1);

            context.Opportunities.Add(opp1);
            context.Opportunities.Add(opp2);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}