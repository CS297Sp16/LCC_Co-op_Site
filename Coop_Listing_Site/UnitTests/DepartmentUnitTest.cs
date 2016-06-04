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
    public class DepartmentUnitTest
    {
        IRepository repo;

        [SetUp]
        public void Init()
        {
            repo = new TestRepository();

            Department dept1 = new Department() { DepartmentID = 1, DepartmentName = "Test Department" },
                dept2 = new Department() { DepartmentID = 2, DepartmentName = "Faux Department" };

            repo.Add(dept1);
            repo.Add(dept2);
        }

        [Test]
        public void Index_ModelNotEmpty()
        {
            var controller = new DepartmentController(repo);
            var index = (ViewResult)controller.Index();
            Assert.IsNotEmpty((IEnumerable<DepartmentModel>)index.Model);
        }

        [Test]
        public void Add_AddDepartment()
        {
            var controller = new DepartmentController(repo);
            var newDept = new DepartmentModel() { DepartmentName = "Test Data" };
            controller.Add(newDept, new int[] { 1 });
            Assert.IsNotNull(repo.GetOne<Department>(d => d.DepartmentName == newDept.DepartmentName));
        }

        [Test]
        public void Delete_DeleteDepartment()
        {
            var controller = new DepartmentController(repo);
            var get = (ViewResult)controller.Add();
            controller.Delete(1);
            controller.Delete(2);
            Assert.AreEqual(get.ViewBag.Departments, 0);
        }
    }
}
