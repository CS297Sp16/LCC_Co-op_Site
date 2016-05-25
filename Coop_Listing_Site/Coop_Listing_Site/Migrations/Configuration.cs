namespace Coop_Listing_Site.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

            //Create roles
            IdentityRole studentrole = new IdentityRole() { Name = "Student" },
                coordrole = new IdentityRole() { Name = "Coordinator" },
                adminrole = new IdentityRole() { Name = "Admin" },
                superAdminRole = new IdentityRole() { Name = "SuperAdmin" };

            context.Roles.AddOrUpdate(r => r.Name, studentrole, coordrole, adminrole, superAdminRole);

            //Create LaneCC majors
            Major major = new Major { MajorName = "Computer Simulation and Game Development" };
            Major major1 = new Major { MajorName = "Computer Network Operations" };
            Major major2 = new Major { MajorName = "Computer Programming" };
            Major major3 = new Major { MajorName = "Health Informatics" };
            Major major4 = new Major { MajorName = "Retail Management" };
            Major major5 = new Major { MajorName = "Business Management" };
            Major major6 = new Major { MajorName = "Accounting" };
            Major major7 = new Major { MajorName = "Emergency Medical Technician(EMT)" };
            Major major8 = new Major { MajorName = "Respiratory Care" };
            Major major9 = new Major { MajorName = "Dental Assisting" };
            Major major10 = new Major { MajorName = "Dental Hygiene" };
            Major major11 = new Major { MajorName = "Medical Office Assistant(MOA)" };
            Major major12 = new Major { MajorName = "Respiratory Care Clinical Practice" };
            Major major13 = new Major { MajorName = "Physical Therapist Asistant(Clinical Affiliation)" };
            Major major14 = new Major { MajorName = "Nursing" };
            Major major15 = new Major { MajorName = "Auto Body & Fender" };
            Major major16 = new Major { MajorName = "Automotive Technology" };
            Major major17 = new Major { MajorName = "Aviation Maintenance" };
            Major major18 = new Major { MajorName = "Construction" };
            Major major19 = new Major { MajorName = "Diesel Technology" };
            Major major20 = new Major { MajorName = "Drafting" };
            Major major21 = new Major { MajorName = "Electronics Technology" };
            Major major22 = new Major { MajorName = "Energy Management" };
            Major major23 = new Major { MajorName = "Flight Technology" };
            Major major24 = new Major { MajorName = "Gen Work Experience(180 & 280)" };
            Major major25 = new Major { MajorName = "Health Occupations(Pharmacy Technician, Sterile Processing, Vet Assistant)" };
            Major major26 = new Major { MajorName = "Occupational Skills" };
            Major major27 = new Major { MajorName = "Landscape" };
            Major major28 = new Major { MajorName = "Manufacturing Technology" };
            Major major29 = new Major { MajorName = "Sustainability Coordinator" };
            Major major30 = new Major { MajorName = "Water Conservation Technology" };
            Major major31 = new Major { MajorName = "Watershed Science Technology" };
            Major major32 = new Major { MajorName = "Welding/ASLCC" };
            Major major33 = new Major { MajorName = "Pre-Law" };
            Major major34 = new Major { MajorName = "Ethnic Studies" };
            Major major35 = new Major { MajorName = "Psychology" };
            Major major36 = new Major { MajorName = "Sociology" };
            Major major37 = new Major { MajorName = "Service Learning +" };
            Major major38 = new Major { MajorName = "Criminal Justice/Academy" };
            Major major39 = new Major { MajorName = "Human Services" };
            Major major40 = new Major { MajorName = "Geographic Information Science(GIS)" };
            Major major41 = new Major { MajorName = "Education(K-12 Teacher Prep)" };
            Major major42 = new Major { MajorName = "Career Skills Training" };
            Major major43 = new Major { MajorName = "Gen Work Experience(280)" };
            Major major44 = new Major { MajorName = "Health Records Technology(HRT)" };
            Major major45 = new Major { MajorName = "Aerobics" };
            Major major46 = new Major { MajorName = "Athletic Training" };
            Major major47 = new Major { MajorName = "Athletics" };
            Major major48 = new Major { MajorName = "Coaching" };
            Major major49 = new Major { MajorName = "Corrective Fitness" };
            Major major50 = new Major { MajorName = "Fitness" };
            Major major51 = new Major { MajorName = "Fitness Management" };
            Major major52 = new Major { MajorName = "Physical Education" };
            Major major53 = new Major { MajorName = "Recreation" };
            Major major54 = new Major { MajorName = "Wellness" };
            Major major55 = new Major { MajorName = "Political Science" };
            Major major56 = new Major { MajorName = "Art & Applied Design" };
            Major major57 = new Major { MajorName = "Graphic Design" };
            Major major58 = new Major { MajorName = "Journalism" };
            Major major59 = new Major { MajorName = "Multimedia Design" };
            Major major60 = new Major { MajorName = "Music" };
            Major major61 = new Major { MajorName = "Performing Arts" };
            Major major62 = new Major { MajorName = "Administration Office Professional" };
            Major major63 = new Major { MajorName = "Medical Receptionist" };
            Major major64 = new Major { MajorName = "Phlebotomy" };
            Major major65 = new Major { MajorName = "Early Childhood Education" };
            Major major66 = new Major { MajorName = "Culinary Arts" };
            Major major67 = new Major { MajorName = "Hospitality Management" };

            //Create LaneCC Departments
            Department dept = new Department { DepartmentName = "Computer Information Technologies" };
            Department dept1 = new Department { DepartmentName = "Business" };
            Department dept2 = new Department {DepartmentName = "Health Professions" };
            Department dept3 = new Department { DepartmentName = "Advanced Technology" };
            Department dept4 = new Department { DepartmentName = "Social Science" };
            Department dept5 = new Department { DepartmentName = "Cooperative Education" };
            Department dept6 = new Department { DepartmentName = "Media Arts" };
            Department dept7 = new Department { DepartmentName = "Child & Family Education" };
            Department dept8 = new Department { DepartmentName = "Culinary/Foood Services" };

            dept.Majors.Add(major);
            dept.Majors.Add(major1);
            dept.Majors.Add(major2);
            dept.Majors.Add(major3);
            dept1.Majors.Add(major4);
            dept1.Majors.Add(major5);
            dept1.Majors.Add(major6);
            dept2.Majors.Add(major7);
            dept2.Majors.Add(major8);
            dept2.Majors.Add(major9);
            dept2.Majors.Add(major10);
            dept2.Majors.Add(major11);
            dept2.Majors.Add(major12);
            dept2.Majors.Add(major13);
            dept2.Majors.Add(major14);
            dept3.Majors.Add(major15);
            dept3.Majors.Add(major16);
            dept3.Majors.Add(major17);
            dept3.Majors.Add(major18);
            dept3.Majors.Add(major19);
            dept3.Majors.Add(major20);
            dept3.Majors.Add(major21);
            dept3.Majors.Add(major22);
            dept3.Majors.Add(major23);
            dept3.Majors.Add(major24);
            dept3.Majors.Add(major25);
            dept3.Majors.Add(major26);
            dept3.Majors.Add(major27);
            dept3.Majors.Add(major28);
            dept3.Majors.Add(major29);
            dept3.Majors.Add(major30);
            dept3.Majors.Add(major31);
            dept3.Majors.Add(major32);
            dept3.Majors.Add(major33);
            dept4.Majors.Add(major34);
            dept4.Majors.Add(major35);
            dept4.Majors.Add(major36);
            dept4.Majors.Add(major37);
            dept4.Majors.Add(major38);
            dept4.Majors.Add(major39);
            dept4.Majors.Add(major40);
            dept4.Majors.Add(major41);
            dept5.Majors.Add(major42);
            dept5.Majors.Add(major43);
            dept5.Majors.Add(major44);
            dept5.Majors.Add(major45);
            dept5.Majors.Add(major46);
            dept5.Majors.Add(major47);
            dept5.Majors.Add(major48);
            dept5.Majors.Add(major49);
            dept5.Majors.Add(major50);
            dept5.Majors.Add(major51);
            dept5.Majors.Add(major52);
            dept5.Majors.Add(major53);
            dept5.Majors.Add(major54);
            dept5.Majors.Add(major55);
            dept6.Majors.Add(major56);
            dept6.Majors.Add(major57);
            dept6.Majors.Add(major58);
            dept6.Majors.Add(major59);
            dept6.Majors.Add(major60);
            dept6.Majors.Add(major61);
            dept2.Majors.Add(major62);
            dept2.Majors.Add(major63);
            dept2.Majors.Add(major64);
            dept7.Majors.Add(major65);
            dept8.Majors.Add(major66);
            dept8.Majors.Add(major67);

            context.Majors.AddOrUpdate(m => m.MajorName, major, major1, major2, major3, major4, major5, major6, major7, major8, major9, major10,
                                        major11, major12, major13, major14, major15, major16, major17, major18, major19, major20, major21, major22,
                                        major23, major24, major25, major26, major27, major28, major29, major30, major31, major32, major33, major34,
                                        major35, major36, major37, major38, major39, major40, major41, major42, major43, major44, major45, major46,
                                        major47, major48, major49, major50, major51, major52, major53, major54, major55, major56, major57, major58,
                                        major59, major60, major61, major62, major63, major64, major65, major66, major67);

            context.Departments.AddOrUpdate(d => d.DepartmentName, dept, dept1, dept2, dept3, dept4, dept5, dept6, dept7, dept8);

            context.SaveChanges();

            // add Users
#if DEBUG
            IdentityResult result1, result2, result3;

            User user1 = userManager.FindByEmail("test@test.com");
            User user2 = userManager.FindByEmail("thinger@test.com");
            User user3 = userManager.FindByEmail("testcoord@test.com");

            if (user1 == null)
            {
                user1 = new User
                {
                    UserName = "test@test.com",
                    Email = "test@test.com",
                    FirstName = "Test",
                    LastName = "Testman",
                    Enabled = true
                };

                result1 = userManager.Create(user1, "password1");

                if (!result1.Succeeded)
                    throw new Exception("Account creation in seed failed");
            }

            if (user2 == null)
            {
                user2 = new User
                {
                    UserName = "thinger@test.com",
                    Email = "thinger@test.com",
                    FirstName = "Jon",
                    LastName = "Doe",
                    Enabled = true
                };

                result2 = userManager.Create(user2, "password1");

                if (!result2.Succeeded)
                    throw new Exception("Account creation in seed failed");
            }

            if (user3 == null)
            {
                user3 = new User
                {
                    UserName = "testcoord@test.com",
                    Email = "testcoord@test.com",
                    FirstName = "John",
                    LastName = "Smith",
                    Enabled = true
                };

                result3 = userManager.Create(user3, "password1");

                if (!result3.Succeeded)
                    throw new Exception("Account creation in seed failed");
            }

            // save so our new objects have IDs
            context.SaveChanges();

            // add some info
            StudentInfo sInfo1 = new StudentInfo
            {
                LNumber = "L00000001",
                User = user1,
                Major = major
            };

            StudentInfo sInfo2 = new StudentInfo
            {
                LNumber = "L00000002",
                User = user2,
                Major = major
            };

            CoordinatorInfo cInfo1 = new CoordinatorInfo
            {
                User = user3
            };

            cInfo1.Majors.Add(major);
            cInfo1.Majors.Add(major1);
            cInfo1.Majors.Add(major2);

            // Add the student role to them
            userManager.AddToRole(user1.Id, "Student");
            userManager.AddToRole(user2.Id, "Student");
            userManager.AddToRoles(user3.Id, "Coordinator", "Admin");

            context.Students.Add(sInfo1);
            context.Students.Add(sInfo2);
            context.Coordinators.Add(cInfo1);
#else
            var role = context.Roles.FirstOrDefault(r => r.Name == "SuperAdmin");

            if (role.Users.Count < 1)
            {
                var admin = userManager.FindByEmail("admin@admin.com");

                if (admin == null)
                {
                    admin = new User
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        FirstName = "Admin",
                        LastName = "Default",
                        Enabled = true
                    };

                    var result = userManager.Create(admin, "password1");

                    if (!result.Succeeded)
                        throw new Exception("Creation of default administrator account failed");
                }

                userManager.AddToRoles(admin.Id, "SuperAdmin", "Admin");
            }
#endif


            // test opportunities
            Opportunity opp1 = new Opportunity
            {
                CompanyName = "Oregon Research Institute",
                ContactName = "Nathen N.",
                ContactNumber = "(541)484-2123, (541)484-1108",
                ContactEmail = "nathann@ori.org",
                Location = "1776 Millrace, Eugene, OR 97403-1983",
                CompanyWebsite = "http://ori.org",
                AboutCompany = "Please follow link: http://ori.org/about_ori",
                AboutDepartment = "This is more about the type of projects we create: http://ori.org/infantnet",
                CoopPositionTitle = "Web Developer Intern",
                CoopPositionDuties = @"Oregon Research Institute is seeking students interested in working on web technologies and gaining practical programming experience.Those who have already been introduced to core web technologies will also have
                                        the opportunity to learn the full stack from front end responsive design to backend database schematics.The intern will also have opportunity to collaborate with a software development firm.This is an internship with a
                                        three or four term commitment and a weekly time commitment will ideally be around 10 hours with compensation at $10 / hr.",
                Qualifications = @"Suggested skills:  Web Design (HTML, CSS, JavaScript), Database Querying, Problem Solving Skills, Curiosity and willingness to learn, Ability to work within a team and take feedback,
                                   Attention to detail: Nice skills to have: Android Application Programming experience, Objective C and iOS programming experience, AngularJS, Node JS",
                Paid = true,
                Wage = "$10 per hour",
                Duration = "three to four terms",
                OpeningsAvailable = 1,
                TermAvailable = "Fall",
                DepartmentID = dept.DepartmentID
            };

            Opportunity opp2 = new Opportunity
            {
                CompanyName = "Get Found",
                ContactName = "Gerry Meenaghan",
                ContactNumber = "(541)463-5883",
                ContactEmail = "meenaghan@lanecc.edu",
                Location = "319 Goodpasture island rd. Eugene, OR 97401",
                CompanyWebsite = "http://getfoundeugene.com",
                AboutCompany = @"Get Found, Eugene, LLC is a small, local web development company based in Eugene.Find out more about what makes it a unique company here: http://getfoundeugene.com",
                CoopPositionTitle = " Web Development Intern",
                CoopPositionDuties = @"Website development using HTML5/CSS3, Javascript, JQuery, & PHP; Wordpress, Researching existing templates / layouts and markets for client's websites,
                                        Creating web pages aligned with customer order specifications and industry best, practices such as responsive design for mobile devices; search - engine optimization,
                                        Occasional and limited office support ranging from answering phones to assisting, technical support staff with hardware / software troubleshooting tasks.",
                Qualifications = @"2nd-year AAS in Computer Programming student (1st-year / certificate-seeking students considered individually), Specific interest in front - end web coding and design work,
                                    Requires high degree of attention to detail; detail - oriented; excellent knowledge of spelling, grammar, punctuation, Successful completion of Web Authoring, JavaScript courses,
                                    completion of Academic Writing (WR 121) and Technical Writing(WR 227) highly recommended, Students with skills in advanced web coding / programming technologies such as PHP,preferred",
                Paid = false,
                Duration = "11 weeks maximum",
                OpeningsAvailable = 1,
                TermAvailable = "Spring",
                DepartmentID = dept.DepartmentID
            };

            Opportunity opp3 = new Opportunity
            {
                CompanyName = "A Family For Every Child",
                ContactName = "Scott Corcoran",
                ContactNumber = "(541)343-2856",
                ContactEmail = "scott@afamilyforeverychild.org",
                Location = "1675 W 11th ave. Eugene, OR 97402",
                CompanyWebsite = "http://afamilyforeverychild.org",
                AboutCompany = "AFFEC is agreat company doing wonderful things for children and families.",
                CoopPositionTitle = "Intern",
                CoopPositionDuties = "not provided yet",
                Qualifications = @"Having enough self-confidence to be psychologically stressed during the first few weeks until they understand what is here,
                                     Not so headstrong as to think that they have time to change everything, Willing to use their skills to build better interfaces to what we already have.",
                Paid = false,
                Duration = "One term",
                OpeningsAvailable = 1,
                TermAvailable = "Spring",
                DepartmentID = dept.DepartmentID
            };

            Opportunity opp4 = new Opportunity
            {
                CompanyName = "Lane County Finacial Division",
                ContactName = "Mike Barnhart",
                ContactNumber = "(541)682-4199",
                ContactEmail = "michael.barnhart@co.lane.us",
                Location = "125 E. 8th Ave. Eugene, OR 97401",
                CompanyWebsite = "http://lanecounty.org",
                CoopPositionTitle = "Intern",
                CoopPositionDuties = @"Auditing the weekly accounts payable vendor checks for accuracy. Agreeing the invoices to the voucher in the computer system,
                                        utilizing Excel spreadsheets. Resolving invoice or voucher discrepancies: includes interaction with other departments, Folding, stuffing, and mailing vendor checks.
                                        Disbursing checks internally.",
                Qualifications = @"Must be available to work the following days: Monday, Tuesday, and Thursday",
                Paid = true,
                Wage = "$12 per hour",
                DepartmentID = dept1.DepartmentID
            };

            Opportunity opp5 = new Opportunity
            {
                CompanyName = "Budget Services",
                ContactName = "University Advancement Human Resources Manager",
                ContactNumber = "(541)346-3123",
                ContactEmail = "@uoregon.edu",
                Location = "1720 E 13th ave. Ste 312 Eugene, OR 97403",
                CompanyWebsite = "http://@uoregon.edu",
                AboutCompany = "Budget Services provides central support services for University Advancement of all financial transactions and contracts.",
                AboutDepartment = @"Budget Services provides central support services for University Advancement of all financial transactions and contracts.  These services include accounts payable, accounts receivable, contracts, financial statements and retention and archiving of all financial and contractual documents",
                CoopPositionTitle = "Assistant",
                CoopPositionDuties = @"Document intake processing, Financial reporting functions, Review, edit and proof documents (contracts, forms and intranet),
                                        Copy, scan and log accounting transactions and other documents, Prepare binders and meeting materials, Data entry, Editing/updating spreadsheets and PDFs,
                                        Maintain electronic libraries and back up documentation, Assist with record retention management, Assist with various other projects as assigned",
                Qualifications = @"High level proficiency with spreadsheet and word processing computer applications, Strong organizational skills, attention to detail, ability to prioritize, and exercise sound independent judgment
                                    Experience creating and maintaining computer spreadsheets and databases. Demonstrated independent problem solving skills; ability to maintain confidentiality and professionalism,
                                    Ability to work cooperatively and strategically in a team environment with all levels of professionals. Excellent oral and written communication skills and ability to communicate and work effectively with individuals from diverse backgrounds and cultures
                                    Commitment to and experience promoting and enhancing diversity and equity",
                Paid = false,
                DepartmentID = dept1.DepartmentID
            };

            context.Opportunities.AddOrUpdate(o => new { o.CompanyName, o.CoopPositionTitle, o.CoopPositionDuties }, opp1, opp2, opp3, opp4, opp5);

            context.SaveChanges();


#if false
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
                //new Major { MajorName = "Computer Programming", Courses = new List<Course>() }, //Initializes the List, can also be placed in the constructor
                //new Major { MajorName = "Computer Network Operations", Courses = new List<Course>() },
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
            /*                                                 User Manager Specific Stuff &
                                                             Open Web Interface for .Net (OWIN)                                               */
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            /*{
                new CoordinatorInfo
                {
                    FirstName = "Gerry",
                    LastName = "Meenaghan",
                    LNumber = "L00000000",
                    Email = "meenaghang@lanecc.edu",
                    PhoneNumber = "(541) 463-5883"
                };
            };

            var students = new List<StudentInfo>()
            {
                new StudentInfo { MajorID = 1, LNumber = "L91234560", FirstName = "Lonnie", LastName = "Teter-Davis",
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
            };*/

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
            //var course = major.Courses.SingleOrDefault(c => c.CourseNumber == courseNumber);
            //if (course == null)
               // major.Courses.Add(context.Courses.Single(c => c.CourseNumber == courseNumber));
        }

        void AddOrUpdate_Dept_Major(DAL.CoopContext context, string deptName, string majorName)
        {
            var dept = context.Departments.SingleOrDefault(d => d.DepartmentName == deptName);
            var major = dept.Majors.SingleOrDefault(m => m.MajorName == majorName);
            if (major == null)
                dept.Majors.Add(context.Majors.Single(m => m.MajorName == majorName));
#endif
        }
    }
}
