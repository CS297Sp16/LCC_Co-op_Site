using Coop_Listing_Site.DAL;
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
            base.Seed(context);

            var coordinators = new List<Coordinator>()
            {
                new Coordinator { FirstName = "Gerry", LastName = "Meenaghan",  LNumber = "L00000000",
                    Email = "meenaghang@lanecc.edu", PhoneNumber = "(541) 463-5883"}
            };

            var students = new List<Student>()
            {
                new Student { MajorID = 1, LNumber = "L91234560", FirstName = "Lonnie", LastName = "Teter-Davis",
                    Email = "lonnie@email.com", PhoneNumber = "(541) 123-1111" },
                new Student { MajorID = 1, LNumber = "L91234561", FirstName = "Gabe", LastName = "Griffin",
                    Email = "gabe@email.com", PhoneNumber = "(541) 123-2222" },
                new Student { MajorID = 1, LNumber = "L91234562",FirstName = "Tony", LastName = "Plueard",
                    Email = "tony@email.com", PhoneNumber = "(541) 123-3333" },
                new Student { MajorID = 1, LNumber = "L91234563",FirstName = "Jason", LastName = "Prall",
                    Email = "jason@email.com", PhoneNumber = "(541) 123-4444" },
                new Student { MajorID = 1, LNumber = "L91234564",FirstName = "Ben", LastName = "Middleton-Rippberger",
                    Email = "ben@email.com", PhoneNumber = "(541) 123-5555" },
                new Student { MajorID = 2, LNumber = "L91234565",FirstName = "Sam", LastName = "Ple",
                    Email = "sample@email.com", PhoneNumber = "(541) 123-6666" },
                new Student { MajorID = 3, LNumber = "L91234566",FirstName = "Exam", LastName = "Ple",
                    Email = "example@email.com", PhoneNumber = "(541) 123-7777" },
                new Student { MajorID = 4, LNumber = "L91234567",FirstName = "Adam", LastName = "Sandler",
                    Email = "sandlera@lane.edu", PhoneNumber = "(541) 123-8888" },
            };

            var companies = new List<Company>()
            {
               new Company { CompanyName = "FakeCompany",Address = "Green & Yellow Brick Rd.", City = "Eugene",
                   State = "OR", ZipCode = "97405", Website = "https://FakeCompany.Example.com"},
               new Company { CompanyName = "AnotherCompany", Address = "Never Never Land Ave.", City = "Springfield",
                   State = "OR", ZipCode = "97477", Website = "https://AnotherCompany.com"},
               new Company { CompanyName = "Symantec", Address = "555 International Way", City = "Springfield",
                   State = "OR", ZipCode = "97477", Website = "https://www.symantec.com"}
            };

            var opportunities = new List<Opportunity>()
            {
                new Opportunity { CompanyID = 1, OpeningsAvailable = 2, TermAvailable = "Summer" },
                new Opportunity { CompanyID = 2, OpeningsAvailable = 1, TermAvailable = "Summer" },
                new Opportunity { CompanyID = 3, OpeningsAvailable = 3, TermAvailable = "Winter" },
                new Opportunity { CompanyID = 3, OpeningsAvailable = 1, TermAvailable = "Spring" }
            };
        }

    }
    }
}