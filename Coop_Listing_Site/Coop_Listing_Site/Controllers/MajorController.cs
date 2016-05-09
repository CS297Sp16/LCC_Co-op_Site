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
        private CoopContext db;

        public MajorController()
        {
            db = new CoopContext();
        }

        public ActionResult Index()
        {
            var majors = db.Majors.Include(m => m.Department).OrderBy(m => m.Department.DepartmentName);

            return View(majors);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Add()
        {
            var depts = db.Departments.OrderBy(d => d.DepartmentName);

            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View();
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "MajorName")] Major major, int DepartmentID)
        {
            var dept = db.Departments.Find(DepartmentID);
            if (dept == null) ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                major.Department = dept;
                db.Majors.Add(major);
                db.SaveChanges();

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

            db.Departments.Load();
            var major = db.Majors.Find(id);
            var depts = db.Departments.OrderBy(d => d.DepartmentName);
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
            db.Majors.Load();
            var dept = db.Departments.Find(DepartmentID);
            var dbMajor = db.Majors.Find(major.MajorID);

            if (dept == null) ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                dbMajor.Department = dept;
                dbMajor.MajorName = major.MajorName;
                db.Entry(dbMajor).State = EntityState.Modified;
                db.SaveChanges();
            }

            var depts = db.Departments.OrderBy(d => d.DepartmentName);
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
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            db.Departments.Load();
            return View(major);
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin, Coordinator"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Major major = db.Majors.Find(id);
            db.Majors.Remove(major);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}