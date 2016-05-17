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
    public class StudentController : Controller
    {
        private IRepository<StudentInfo> studentsRepo;
        private IRepository<Major> majorsRepo;
        private IRepository<Department> departmentsRepo;

        public StudentController()
        {
            var db = new CoopContext();
            studentsRepo = new StudentRepo(db);
            majorsRepo = new MajorsRepo(db);
            departmentsRepo = new DepartmentRepo(db);
        }

        public StudentController(IRepository<StudentInfo> sRepo,
            IRepository<Major> mRepo, IRepository<Department> dRepo)
        {
            studentsRepo = sRepo;
            majorsRepo = mRepo;
            departmentsRepo = dRepo;
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