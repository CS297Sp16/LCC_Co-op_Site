namespace Coop_Listing_Site.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.CoopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DAL.CoopContext context)
        {
            var courses = new List<Course>()
            {              
                //Computer Programming              
                new Course { CourseNumber = "CS133N" },  //0
                new Course { CourseNumber = "MTH095" },  //1
                new Course { CourseNumber = "CIS195" },  //2
                new Course { CourseNumber = "ART288" },  //3
                new Course { CourseNumber = "CS233N" },  //4
                new Course { CourseNumber = "WR121" },   //5
                new Course { CourseNumber = "CG203" },   //6
                new Course { CourseNumber = "CS133JS" }, //7
                new Course { CourseNumber = "CS125D" },  //8
                new Course { CourseNumber = "CS234N" },  //9
                new Course { CourseNumber = "CIS244" },  //10
                new Course { CourseNumber = "CS295N" },  //11
                //Required for Both
                new Course { CourseNumber = "CIS100" },  //12
                //Computer Network Operations
                new Course { CourseNumber = "CS179" },   //13
                new Course { CourseNumber = "CIS140W" }  //14
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseNumber, s));

            var majors = new List<Major>()
            {
                new Major { MajorName = "Computer Programming", Courses = new List<Course>() }, //Initializes the List, can also be placed in the constructor
                new Major { MajorName = "Computer Network Operations", Courses = new List<Course>() },
                new Major { MajorName = "Computer Simulation and Game Development" },
                new Major { MajorName = "Computer Information Systems" }
            };
            majors.ForEach(s => context.Majors.AddOrUpdate(p => p.MajorName, s));

            //Programming <--The method below populates the Initialized Courses List... shown above ^^^^^^^^^^^^^^^^^^^
            for (int i = 0; i <= 12; i++)
            {
                AddOrUpdate_Major_Course(context, majors[0].MajorName, courses[i].CourseNumber);
            }
            //Networking
            for (int k = 12; k <= 14; k++)
            {
                AddOrUpdate_Major_Course(context, majors[1].MajorName, courses[k].CourseNumber);
            }

            var departments = new List<Department>()
            {
                new Department { DepartmentName = "Computer Information Technology", Majors = new List<Major>()}
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.DepartmentName, s));  //AddOrUpdates the Entity

            //AddOrUpdate the List in the Entity
            for (int i = 0; i <= 3; i++)
            {
                AddOrUpdate_Dept_Major(context, departments[0].DepartmentName, majors[i].MajorName);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /*                                                 User Manager Specific Stuff                                                    */
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        //helper method used like the AddOrUpdate method... this inhibits the migration from duplicating entries
        void AddOrUpdate_Major_Course(DAL.CoopContext context, string majorName, string courseNumber)
        {
            var major = context.Majors.SingleOrDefault(m => m.MajorName == majorName);
            var course = major.Courses.SingleOrDefault(c => c.CourseNumber == courseNumber);
            if (course == null)
                major.Courses.Add(context.Courses.Single(c => c.CourseNumber == courseNumber));
        }

        void AddOrUpdate_Dept_Major(DAL.CoopContext context, string deptName, string majorName)
        {
            var dept = context.Departments.SingleOrDefault(d => d.DepartmentName == deptName);
            var major = dept.Majors.SingleOrDefault(m => m.MajorName == majorName);
            if (major == null)
                dept.Majors.Add(context.Majors.Single(m => m.MajorName == majorName));
        }
    }
}
