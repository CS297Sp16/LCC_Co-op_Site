using System;
using NUnit.Framework;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace UnitTests
{
    [TestFixture]
    public class MajorUnitTest
    {
        IRepository repo;

        [Test]
        public void IndexModelNotEmpty()
        {
            var controller = new MajorController(repo);

            var index = (ViewResult)controller.Index();

            Assert.IsNotEmpty((IEnumerable<Major>)index.Model);
        }


        [SetUp]
        public void Init()
        {
            // fresh repo
            repo = new TestRepository();

            // make test data
            Department dept1 = new Department() { DepartmentID = 1, DepartmentName = "Test Department" },
                dept2 = new Department() { DepartmentID = 2, DepartmentName = "Faux Department" };
            Major major1 = new Major() { MajorID = 1, MajorName = "Tester" },
                major2 = new Major() { MajorID = 2, MajorName = "Test Subject" },
                major3 = new Major() { MajorID = 3, MajorName = "Unemployment" },
                major4 = new Major() { MajorID = 4, MajorName = "Geting Rich Quick" };

            // set references
            dept1.Majors.Add(major1);
            dept1.Majors.Add(major2);
            dept2.Majors.Add(major3);
            dept2.Majors.Add(major4);
            major1.Department = dept1;
            major2.Department = dept1;
            major3.Department = dept2;
            major4.Department = dept2;

            // add data to repo
            repo.Add(dept1);
            repo.Add(dept2);
            repo.Add(major1);
            repo.Add(major2);
            repo.Add(major3);
            repo.Add(major4);
        }
    }
}
