using Coop_Listing_Site.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Coop_Listing_Site.Models;
using System.Net;
using Coop_Listing_Site.Models.ViewModels;

namespace Coop_Listing_Site.Controllers
{
    public class MajorController : Controller
    {
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

            var majorVMs = majors.Select(m => new MajorViewModel(m));

            return View(majorVMs);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Add()
        {
            var depts = repo.GetAll<Department>();

            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View();
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "MajorName")] MajorViewModel majorVM, int? DepartmentID)
        {
            var dept = repo.GetByID<Department>(DepartmentID);

            if (dept == null && DepartmentID != null)
                ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                majorVM.Department = dept;
                var major = majorVM.ToMajor();

                repo.Add(major);

                return RedirectToAction("Index");
            }

            return View(majorVM);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var major = repo.GetByID<Major>(id);

            if (major == null)
                return HttpNotFound();

            var majorVM = new MajorViewModel(major);

            var depts = repo.GetAll<Department>().OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName", major.Department?.DepartmentID);

            return View(majorVM);
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MajorID, MajorName")] MajorViewModel majorVM, int? DepartmentID)
        {
            var dept = repo.GetByID<Department>(DepartmentID);
            var dbMajor = repo.GetByID<Major>(majorVM.MajorID);

            if (dept == null && DepartmentID != null)
                ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                if (dept == null && dbMajor.Department != null)
                    dbMajor.Department.Majors.Remove(dbMajor);
                else
                    dbMajor.Department = dept;

                dbMajor.MajorName = majorVM.MajorName;
                repo.Update(dbMajor);

                majorVM = new MajorViewModel(dbMajor);
            }

            var depts = repo.GetAll<Department>().OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName", majorVM.Department?.DepartmentID);

            ViewBag.Updated = true;

            return View(majorVM);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var major = repo.GetByID<Major>(id);

            if (major == null)
                return HttpNotFound();

            var majorVM = new MajorViewModel(major);
            return View(majorVM);
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin, Coordinator"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Major major = repo.GetByID<Major>(id);

            if (major == null)
                return HttpNotFound();

            repo.Delete(major);

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