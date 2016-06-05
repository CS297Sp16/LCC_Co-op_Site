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
    public class MajorUnitTest
    {
        IRepository repo;

        [Test]
        public void Index_ModelNotEmpty()
        {
            var controller = new MajorController(repo);

            var index = (ViewResult)controller.Index();

            Assert.IsNotEmpty((IEnumerable<MajorViewModel>)index.Model);
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

            var newMajor = new MajorViewModel() { MajorName = "Data Entry Grunt" };

            controller.Add(newMajor, 1);

            Assert.IsNotNull(repo.GetOne<Major>(m => m.MajorName == newMajor.MajorName));
        }

        [Test]
        public void Edit_ModelNotEmpty()
        {
            var controller = new MajorController(repo);

            var view = (ViewResult)controller.Edit(1);

            Assert.IsNotNull((MajorViewModel)view.Model);
        }

        [Test]
        public void Edit_CorrectModelInfo()
        {
            var controller = new MajorController(repo);

            var view = (ViewResult)controller.Edit(1);

            MajorViewModel model = (MajorViewModel)view.Model;

            Major major = repo.GetByID<Major>(1);

            Assert.AreEqual(model.MajorID, major.MajorID);
            Assert.AreEqual(model.MajorName, major.MajorName);
        }

        [Test]
        public void Edit_EditMajor()
        {
            var controller = new MajorController(repo);

            var majorVM = new MajorViewModel(repo.GetByID<Major>(3));

            majorVM.MajorName = "Unemployment Specialist";

            controller.Edit(majorVM, 1);

            Assert.AreEqual(repo.GetByID<Major>(3).MajorName, "Unemployment Specialist");
        }

        [Test]
        public void Delete_NullIdBadRequest()
        {
            var controller = new MajorController(repo);

            var test = (HttpStatusCodeResult)controller.Delete(null);

            Assert.AreEqual(test.StatusCode, 400);
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
