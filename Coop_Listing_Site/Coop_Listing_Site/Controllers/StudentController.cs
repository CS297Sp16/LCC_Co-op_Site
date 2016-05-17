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
        private IRepository<StudentInfo> studentsRepo;
        private IRepository<Major> majorsRepo;
        private IRepository<Department> departmentsRepo;
        private IRepository<RegisterInvite> invitationsRepo;

        public StudentController()
        {
            var db = new CoopContext();
            studentsRepo = new StudentRepo(db);
            majorsRepo = new MajorsRepo(db);
            departmentsRepo = new DepartmentRepo(db);
            invitationsRepo = new InvitationRepo(db);
        }

        public StudentController(IRepository<StudentInfo> sRepo,
            IRepository<Major> mRepo, IRepository<Department> dRepo, IRepository<RegisterInvite> iRepo)
        {
            studentsRepo = sRepo;
            majorsRepo = mRepo;
            departmentsRepo = dRepo;
            invitationsRepo = iRepo;
        }

        public ActionResult Index()
        {
            //TODO: be more specific sbout who sees what students
            var studentVMs = studentsRepo.GetAll().Select(s => new StudentViewModel(s));

            return View(studentVMs);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = studentsRepo.GetByID(id);

            return View(new StudentViewModel(student));
        }

        
        public ActionResult Enable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = studentsRepo.GetByID(id);

            return View(new StudentViewModel(student));
        }

        [HttpPost, ActionName("Enable"),  ValidateAntiForgeryToken]
        public ActionResult ConfirmEnable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = studentsRepo.GetByID(id);

            student.User.Enabled = true;
            studentsRepo.Update(student);

            ViewBag.Updated = true;

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Disable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = studentsRepo.GetByID(id);

            return View(new StudentViewModel(student));
        }

        [HttpPost, ActionName("Disable"), ValidateAntiForgeryToken]
        public ActionResult ConfirmDisable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = studentsRepo.GetByID(id);

            student.User.Enabled = false;
            studentsRepo.Update(student);

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Invitations()
        {
            return View(invitationsRepo.GetAll());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                majorsRepo.Dispose();
                departmentsRepo.Dispose();
                studentsRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}