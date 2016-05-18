using Coop_Listing_Site.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Coop_Listing_Site.Models;
using System.Net;

namespace Coop_Listing_Site.Controllers
{
    public class MajorController : Controller
    {
        //private CoopContext db;
        private IRepository<Major> majorsRepo;
        private IRepository<Department> departmentsRepo;


        public MajorController()
        {
            var db = new CoopContext();
            majorsRepo = new MajorsRepo(db);
            departmentsRepo = new DepartmentRepo(db);
        }

        // for unit testing
        public MajorController(IRepository<Major> repo, IRepository<Department> deptRepo)
        {
            majorsRepo = repo;
            departmentsRepo = deptRepo;
        }


        public ActionResult Index()
        {
            var majors = majorsRepo.GetAll();

            return View(majors);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Add()
        {
            var depts = departmentsRepo.GetAll();

            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View();
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "MajorName")] Major major, int DepartmentID)
        {
            var dept = departmentsRepo.GetByID(DepartmentID);

            if (dept == null)
                ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                major.Department = dept;
                majorsRepo.Add(major);

                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var major = majorsRepo.GetByID(id);
            var depts = departmentsRepo.GetAll().OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName", major.Department.DepartmentID);

            /* For if/when we add courses
            var courses = db.Courses.OrderBy(c => c.CourseNumber);
            var selectedCourses = db.Majors.Find(id).Courses.Select(c => c.CourseNumber);
            ViewBag.Courses = new MultiSelectList(courses, "CourseID", "CourseNumber", selectedCourses);
            */

            return View(major);
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MajorID, MajorName")] Major major, int DepartmentID)
        {
            var dept = departmentsRepo.GetByID(DepartmentID);
            var dbMajor = majorsRepo.GetByID(major.MajorID);

            if (dept == null) ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                dbMajor.Department = dept;
                dbMajor.MajorName = major.MajorName;
                majorsRepo.Update(dbMajor);
            }

            var depts = departmentsRepo.GetAll().OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            ViewBag.Updated = true;

            return View(major);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var major = majorsRepo.GetByID(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            //db.Departments.Load();
            return View(major);
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin, Coordinator"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Major major = majorsRepo.GetByID(id);
            majorsRepo.Delete(major);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                majorsRepo.Dispose();
                departmentsRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}