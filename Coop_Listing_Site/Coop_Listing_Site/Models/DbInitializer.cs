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
    public class DbInitializer : DropCreateDatabaseIfModelChanges<CoopContext>
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

            Major major = new Major { MajorName = "Computer Simulation and Game Development" };
            Major major1 = new Major { MajorName = "Computer Network Operations" };
            Major major2 = new Major { MajorName = " Computer Programming" };
            Major major3 = new Major { MajorName = "Health Informatics" };
            Major major4 = new Major { MajorName = "Retail Management" };
            Major major5 = new Major { MajorName = "Business Management" };
            Major major6 = new Major { MajorName = "Accounting" };

            Department dept = new Department { DepartmentName = "Computer Information Technologies" };
            Department dept1 = new Department { DepartmentName = "Business" };

            dept.Majors.Add(major);
            dept.Majors.Add(major1);
            dept.Majors.Add(major2);
            dept.Majors.Add(major3);
            dept1.Majors.Add(major4);
            dept1.Majors.Add(major5);
            dept1.Majors.Add(major6);

            context.Majors.Add(major);
            context.Majors.Add(major1);
            context.Majors.Add(major2);
            context.Majors.Add(major3);
            context.Majors.Add(major4);
            context.Majors.Add(major5);
            context.Majors.Add(major6);

            context.Departments.Add(dept);
            context.Departments.Add(dept1);

            // save so our new objects have IDs
            context.SaveChanges();

            // add some info
            StudentInfo sInfo1 = new StudentInfo
            {
                LNumber = "L00000001",
                User = user1,
                Major = major
            };

            /*StudentInfo sInfo2 = new StudentInfo
            {
                LNumber = "L00000002",
                User = user2,
                Major = major
            };*/

            CoordinatorInfo cInfo0 = new CoordinatorInfo
            {
                User = user2
            };

            cInfo0.Majors.Add(major4);
            cInfo0.Majors.Add(major5);
            cInfo0.Majors.Add(major6);

            CoordinatorInfo cInfo1 = new CoordinatorInfo
            {
                User = user3
            };

            cInfo1.Majors.Add(major);
            cInfo1.Majors.Add(major1);
            cInfo1.Majors.Add(major2);

            // Add the student role to them
            userManager.AddToRole(user1.Id, "Student");
            userManager.AddToRole(user2.Id, "Coordinator");
            userManager.AddToRoles(user3.Id, "Coordinator", "Admin");


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
                Department = dept
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
                Department = dept
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
                Department = dept
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
                Department = dept1
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
                Department = dept1
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
            //context.Students.Add(sInfo2);
            context.Coordinators.Add(cInfo0);
            context.Coordinators.Add(cInfo1);

            context.Opportunities.Add(opp1);
            context.Opportunities.Add(opp2);
            context.Opportunities.Add(opp3);
            context.Opportunities.Add(opp4);
            context.Opportunities.Add(opp5);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
