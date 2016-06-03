using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    public class AccountController : Controller
    {
        private IRepository repo;
        private UserManager<User> userManager;

        public AccountController()
        {
            var db = new CoopContext();
            repo = new Repository(db);
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        private User CurrentUser
        {
            get
            {
                return repo.GetByID<User>(User.Identity.GetUserId());
            }
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Student()
        {
            var studInfo = repo.GetOne<StudentInfo>(s => s.User.Id == CurrentUser.Id);

            ViewBag.Majors = new SelectList(repo.GetAll<Major>(), "MajorID", "MajorName", studInfo.Major.MajorID);

            var studentVM = new StudentUpdateModel(studInfo);

            return View(studentVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Student([Bind(Include = "UserId,GPA,MajorID")] StudentUpdateModel studentUpdateModel)
        {
            var studInfo = repo.GetOne<StudentInfo>(s => s.User.Id == CurrentUser.Id);

            var major = repo.GetByID<Major>(studentUpdateModel.MajorID);

            ViewBag.Majors = new SelectList(repo.GetAll<Major>(), "MajorID", "MajorName", studInfo.Major.MajorID);

            if (ModelState.IsValid)
            {
                if (studInfo.GPA != studentUpdateModel.GPA)
                {
                    studInfo.GPA = (double)studentUpdateModel.GPA;
                    repo.Update(studInfo);
                }

                if (studInfo.Major != major)
                {
                    studInfo.Major = major;
                    repo.Update(studInfo);
                }

                studentUpdateModel = new StudentUpdateModel(studInfo);
                ViewBag.Updated = true;
            }

            return View(studentUpdateModel);
        }

        public ActionResult Coordinator()
        {
            return View();
        }
    }
}