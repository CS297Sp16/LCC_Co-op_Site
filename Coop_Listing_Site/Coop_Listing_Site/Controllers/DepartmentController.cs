using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    [Authorize(Roles = "Admin, Coordinator")]
    public class DepartmentController : Controller
    {
        private IRepository repo;


        public DepartmentController()
        {
            var db = new CoopContext();
            repo = new Repository(db);
        }

        // for unit testing
        public DepartmentController(IRepository r)
        {
            repo = r;
        }


        public ActionResult Index()
        {
            var depts = repo.GetAll<Department>();

            return View(depts);
        }

        public ActionResult Add()
        {
            var majors = repo.GetAll<Department>();

            ViewBag.Majors = new SelectList(majors, "MajorID", "MajorName");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "DepartmentName")] Department dept)
        {
            if (ModelState.IsValid)
            {
                var newDept = repo.Add(dept);

                return RedirectToAction("Edit", new { id = newDept.DepartmentID });
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dept = repo.GetByID<Department>(id);
            var majors = repo.GetWhere<Major>(m => m.Department == dept || m.Department == null).OrderBy(m => m.MajorName);

            ViewBag.Majors = new MultiSelectList(majors, "MajorID", "MajorName", dept.Majors.Select(m => m.MajorID));

            return View(dept);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID, DepartmentName")] Department dept, int[] majorIDs)
        {
            List<Major> deptMajors = null;
            // handle if they haven't selected any majors
            if (majorIDs != null)
                deptMajors = repo.GetWhere<Major>(m => majorIDs.Contains(m.MajorID)).ToList();

            var dbDept = repo.GetByID<Department>(dept.DepartmentID);

            if (dept == null) ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                if (majorIDs != null)
                    dbDept.Majors = deptMajors;
                else
                    dbDept.Majors.Clear();
                dbDept.DepartmentName = dept.DepartmentName;

                repo.Update(dbDept);
            }

            var majors = repo.GetWhere<Major>(m => m.Department == dbDept || m.Department == null).OrderBy(m => m.MajorName);
            ViewBag.Majors = new MultiSelectList(majors, "MajorID", "MajorName", dbDept.Majors.Select(m => m.MajorID));
            ViewBag.Updated = true;

            return View(dept);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dept = repo.GetByID<Department>(id);
            if (dept == null)
            {
                return HttpNotFound();
            }
            return View(dept);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Department dept = repo.GetByID<Department>(id);
            dept.Majors.Clear();
            repo.Delete(dept);

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