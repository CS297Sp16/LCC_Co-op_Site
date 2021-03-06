using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
using System.Net;

namespace Coop_Listing_Site.Controllers
{
    [Authorize(Roles = "Admin, Coordinator")]
    public class StudentController : Controller
    {
        private IRepository repo;

        public StudentController()
        {
            var db = new CoopContext();
            repo = new Repository(db);
        }

        public StudentController(IRepository r)
        {
            repo = r;
        }

        public ActionResult Index()
        {
            //TODO: be more specific sbout who sees what students
            var studentVMs = repo.GetAll<StudentInfo>().Select(s => new StudentViewModel(s));

            return View(studentVMs);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = repo.GetByID<StudentInfo>(id);

            if (student == null)
                return HttpNotFound();

            return View(new StudentViewModel(student));
        }

        
        public ActionResult Enable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = repo.GetByID<StudentInfo>(id);

            if (student == null)
                return HttpNotFound();

            return View(new StudentViewModel(student));
        }

        [HttpPost, ActionName("Enable"),  ValidateAntiForgeryToken]
        public ActionResult ConfirmEnable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = repo.GetByID<StudentInfo>(id);

            if (student == null)
                return HttpNotFound();

            student.User.Enabled = true;
            repo.Update(student);

            ViewBag.Updated = true;

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Disable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = repo.GetByID<StudentInfo>(id);

            if (student == null)
                return HttpNotFound();

            return View(new StudentViewModel(student));
        }

        [HttpPost, ActionName("Disable"), ValidateAntiForgeryToken]
        public ActionResult ConfirmDisable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = repo.GetByID<StudentInfo>(id);

            if (student == null)
                return HttpNotFound();

            student.User.Enabled = false;
            repo.Update(student);

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Invitations()
        {
            return View(repo.GetWhere<RegisterInvite>(i => i.UserType == RegisterInvite.AccountType.Student));
        }

        public ActionResult Rescind(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var inv = repo.GetOne<RegisterInvite>(i => i.RegisterInviteID == id &&
                i.UserType == RegisterInvite.AccountType.Student);

            if (inv == null) return HttpNotFound();

            return View(inv);
        }

        [HttpPost, ActionName("Rescind"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmRescind(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var inv = repo.GetOne<RegisterInvite>(i => i.RegisterInviteID == id &&
                i.UserType == RegisterInvite.AccountType.Student);

            if (inv == null) return HttpNotFound();
            
            repo.Delete(inv);

            return RedirectToAction("Invitations");
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