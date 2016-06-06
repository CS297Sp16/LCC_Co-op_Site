using System;
using NUnit.Framework;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using Coop_Listing_Site.Models.ViewModels;

namespace UnitTests
{
    [TestFixture]
    public class OpportunityUnitTest
    {
        IRepository repo;

        [SetUp]
        public void Init()
        {
            repo = new TestRepository();

            Department dept1 = new Department() { DepartmentID = 1, DepartmentName = "Test Department" },
                dept2 = new Department() { DepartmentID = 2, DepartmentName = "Faux Department" };
            Major major1 = new Major() { MajorID = 1, MajorName = "Test Major" },
                major2 = new Major() { MajorID = 2, MajorName = "Faux Test Major" };
            Opportunity opp1 = new Opportunity() { OpportunityID = 1, AboutCompany = "Test Opportunity", AboutDepartment = "Test Opportunity", CompanyName = "Test Opportunity",
                CompanyWebsite = "www.Test Opportunity.com", ContactEmail = "Test@Opportunity.com", ContactName = "Tester", ContactNumber = "(111)111-1111",
                CoopPositionTitle = "Test Opportunity", CoopPositionDuties = "Test Opportunity", GPA = 2.5, Paid = true, Amount = "$10 per hour", Approved = true,
                Duration = "11 Weeks", Location = "1234 Test Site Rd. TestingTown TS 11111", Qualifications = "Meet test speccifications", OpeningsAvailable = 4,
                TermAvailable = "Spring", Department = dept1 };
            Opportunity opp2 = new Opportunity() { OpportunityID = 2, AboutCompany = "Faux Test Opportunity", AboutDepartment = "Faux Test Opportunity", CompanyName = "Faux Test Opportunity",
                CompanyWebsite = "www.FauxTestOpportunity.com", ContactEmail = "Faux@TestOpportunity.com", ContactName = " Faux Tester", ContactNumber = "(111)222-2222",
                CoopPositionTitle = "Faux Test Opportunity", CoopPositionDuties = "Faux Test Opportunity", GPA = 4.5, Paid = false, Approved = true,
                Duration = "11 Weeks", Location = "4321 Test Site Ave. TestingTown TS 22222", Qualifications = "Meet Faux Test Specifications", OpeningsAvailable = 1,
                TermAvailable = "Winter", Department = dept2 };

            dept1.Majors.Add(major1);
            dept2.Majors.Add(major2);
            major1.Department = dept1;
            major2.Department = dept2;
            major1.Opportunities.Add(opp1);
            major2.Opportunities.Add(opp2);

            repo.Add(dept1);
            repo.Add(dept2);
            repo.Add(major1);
            repo.Add(major2);
            repo.Add(opp1);
            repo.Add(opp2);
        }

        [Test]
        public void Add_DeptList()
        {
            var controller = new MajorController(repo);
            var add = (ViewResult)controller.Add();
            Assert.IsNotEmpty(add.ViewBag.Departments);
        }

        [Test]
        public void Add_AddMajor()
        {
            var controller = new MajorController(repo);
            var newMajor = new MajorViewModel() { MajorName = "Real Test Data" };
            controller.Add(newMajor, 1);
            Assert.IsNotNull(repo.GetOne<Major>(m => m.MajorName == newMajor.MajorName));
        }

        [Test]
        public void Add_AddOpportunity()
        {
            var controller = new CoopController(repo);
            var newOpportunity = new OpportunityModel()
            {
                CompanyName = "Implant Data",
                CompanyWebsite = "www.implantdata.com",
                ContactName = "Data",
                ContactNumber = "(111)333-3333",
                ContactEmail = "Data@aol.com",
                CoopPositionTitle = "Test Data",
                CoopPositionDuties = "Test Data",
                AboutCompany = "Test Data",
                AboutDepartment = "Test Data",
                Location = "2222 Test Way TestTown TS 44444",
                Qualifications = "Test Data",
                Paid = false,
                Duration = "11 Weeks",
                TermAvailable = "Winter",
                OpeningsAvailable = 3
            };
            controller.AddOpportunity(newOpportunity, 1, new int[] { 1 });
            Assert.IsNotNull(repo.GetOne<Opportunity>(o => o.OpportunityID == newOpportunity.OpportunityID));
        }

        [Test]
        public void Delete_DeleteOpportunity()
        {
            var controller = new CoopController(repo);
            controller.DeleteConfirmed(2);
            controller.DeleteConfirmed(1);
            var opp1 = repo.GetByID<Opportunity>(1);
            var opp2 = repo.GetByID<Opportunity>(2);
            Assert.IsNull(opp1);
            Assert.IsNull(opp2);
        }
    }
}
