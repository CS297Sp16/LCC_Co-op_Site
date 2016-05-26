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
        private IRepository repo;


        public MajorController()
        {
            var db = new CoopContext();
            repo = new Repository(db);
        }

        // for unit testing
        public MajorController(IRepository r)
        {
            repo = r;
        }


        public ActionResult Index()
        {
            var majors = repo.GetAll<Major>();

            return View(majors);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Add()
        {
            var depts = repo.GetAll<Department>();

            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View();
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "MajorName")] Major major, int DepartmentID)
        {
            var dept = repo.GetByID<Department>(DepartmentID);

            if (dept == null)
                ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                major.Department = dept;
                repo.Add<Major>(major);

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

            var major = repo.GetByID<Major>(id);
            var depts = repo.GetAll<Department>().OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName", major.Department.DepartmentID);

            return View(major);
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MajorID, MajorName")] Major major, int DepartmentID)
        {
            var dept = repo.GetByID<Department>(DepartmentID);
            var dbMajor = repo.GetByID<Major>(major.MajorID);

            if (dept == null) ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                dbMajor.Department = dept;
                dbMajor.MajorName = major.MajorName;
                repo.Update<Major>(dbMajor);
            }

            var depts = repo.GetAll<Department>().OrderBy(d => d.DepartmentName);
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
            var major = repo.GetByID<Major>(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin, Coordinator"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Major major = repo.GetByID<Major>(id);
            repo.Delete<Major>(major);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}