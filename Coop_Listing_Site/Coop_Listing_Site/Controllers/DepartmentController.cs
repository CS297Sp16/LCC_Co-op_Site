using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
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
            var majors = repo.GetWhere<Major>(m => m.Department == null).OrderBy(m => m.MajorName);

            ViewBag.Majors = new SelectList(majors, "MajorID", "MajorName");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "DepartmentName")] DepartmentModel dept, int[] MajorIDs)
        {
            var majorList = new List<Major>();

            if (MajorIDs != null)
            {
                foreach (var id in MajorIDs)
                {
                    var major = repo.GetByID<Major>(id);
                    if (major != null)
                        majorList.Add(major);
                }
            }

            dept.Majors = majorList;

            if (ModelState.IsValid)
            {
                var department = dept.ToDepartment();

                var newDept = repo.Add(department);

                return RedirectToAction("Edit", new { id = newDept.DepartmentID });
            }
            var majors = repo.GetWhere<Major>(m => m.Department == null).OrderBy(m => m.MajorName);

            ViewBag.Majors = new SelectList(majors, "MajorID", "MajorName");

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dept = repo.GetByID<Department>(id);

            if (dept == null)
                return HttpNotFound();

            var majors = repo.GetWhere<Major>(m => m.Department == null && !dept.Majors.Contains(m)).OrderBy(m => m.MajorName);
            ViewBag.Majors = new SelectList(majors, "MajorID", "MajorName");

            var deptvm = new DepartmentModel(dept);
            return View(deptvm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID, DepartmentName")] DepartmentModel dept, int[] MajorIDs)
        {
            List<Major> deptMajors = new List<Major>();

            if (MajorIDs != null)
                deptMajors = repo.GetWhere<Major>(m => MajorIDs.Contains(m.MajorID)).ToList();

            var dbDept = repo.GetByID<Department>(dept.DepartmentID);

            if (dbDept == null) ModelState.AddModelError("", "Unable to find the selected department. Please contact the administrator if this problem persists");

            if (ModelState.IsValid)
            {
                foreach (var major in repo.GetWhere<Major>(m => m.Department == dbDept))
                {
                    if (!deptMajors.Contains(major))
                    {
                        major.Department = null;
                        dept.Majors.Remove(major);
                        //db.Entry(major).State = EntityState.Modified;
                    }
                }

                foreach (var major in deptMajors)
                {
                    if (!dbDept.Majors.Contains(major))
                    {
                        major.Department = dbDept;
                        dbDept.Majors.Add(major);
                    }
                }

                dbDept.DepartmentName = dept.DepartmentName;

                repo.Update(dbDept);
            }
            dept = new DepartmentModel(dbDept);
            var majors = repo.GetWhere<Major>(m => m.Department == null && !dept.Majors.Contains(m)).OrderBy(m => m.MajorName);
            ViewBag.Majors = new SelectList(majors, "MajorID", "MajorName");
            ViewBag.Updated = true;

            return View(dept);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dept = repo.GetByID<Department>(id);

            if (dept == null)
                return HttpNotFound();

            var deptvm = new DepartmentModel(dept);
            return View(deptvm);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Department dept = repo.GetByID<Department>(id);

            foreach (var major in dept.Majors)
                major.Department = null;

            foreach (var opp in repo.GetWhere<Opportunity>(o => o.Department.DepartmentID == dept.DepartmentID))
                opp.Department = null;

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