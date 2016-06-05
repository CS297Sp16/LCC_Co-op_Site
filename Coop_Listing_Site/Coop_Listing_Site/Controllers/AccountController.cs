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

            ViewBag.Majors = new SelectList(repo.GetAll<Major>().OrderBy(m => m.MajorName), "MajorID", "MajorName", studInfo.Major.MajorID);

            var studentVM = new StudentUpdateModel(studInfo);

            return View(studentVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Student([Bind(Include = "LNumber,GPA,MajorID,Email")] StudentUpdateModel studentUpdateModel)
        {
            var studInfo = repo.GetByID<StudentInfo>(studentUpdateModel.LNumber);

            var major = repo.GetByID<Major>(studentUpdateModel.MajorID);

            ViewBag.Majors = new SelectList(repo.GetAll<Major>().OrderBy(m => m.MajorName), "MajorID", "MajorName", studInfo.Major.MajorID);

            if (ModelState.IsValid)
            {
                if (studentUpdateModel.GPA == null)
                    studentUpdateModel.GPA = 0;

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

                if(studentUpdateModel.Email != studInfo.User.Email)
                {
                    studInfo.User.Email = studentUpdateModel.Email;
                    userManager.Update(studInfo.User);
                }

                studentUpdateModel = new StudentUpdateModel(studInfo);
                ViewBag.Updated = true;
            }

            return View(studentUpdateModel);
        }

        public ActionResult Coordinator()
        {
            var coord = repo.GetOne<CoordinatorInfo>(c => c.User.Id == CurrentUser.Id);
            var coordVM = new CoordUpdateModel(coord);

            var nocoordMajors = repo.GetWhere<Major>(m => m.Coordinator == null).OrderBy(m => m.MajorName);
            ViewBag.Majors = new SelectList(nocoordMajors, "MajorID", "MajorName");

            return View(coordVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Coordinator([Bind(Include = "CoordID,Email")]CoordUpdateModel updatedCoord, int[] MajorIDs, int? Majors)
        {
            var coordInfo = repo.GetByID<CoordinatorInfo>(updatedCoord.CoordID);

            if (ModelState.IsValid)
            {
                if(MajorIDs == null && Majors == null)
                {
                    foreach (var major in coordInfo.Majors)
                    {
                        major.Coordinator = null;
                    }

                    coordInfo.Majors.Clear();

                    repo.Update(coordInfo);
                }
                else if(MajorIDs != null)
                {
                    var coordMajors = repo.GetWhere<Major>(m => MajorIDs.Contains(m.MajorID)).ToList();

                    foreach(var major in repo.GetWhere<Major>(m => m.Coordinator == coordInfo))
                    {
                        if (!coordMajors.Contains(major))
                            major.Coordinator = null;
                    }

                    foreach(var major in coordMajors)
                    {
                        if (major.Coordinator == null)
                            major.Coordinator = coordInfo;
                    }

                    coordInfo.Majors = coordMajors;
                    repo.Update(coordInfo);
                }
                else
                {
                    var selectedMajor = repo.GetByID<Major>(Majors);

                    foreach (var major in coordInfo.Majors)
                        major.Coordinator = null;

                    coordInfo.Majors.Clear();
                    coordInfo.Majors.Add(selectedMajor);
                    selectedMajor.Coordinator = coordInfo;

                    repo.Update(coordInfo);
                }

                if (updatedCoord.Email != coordInfo.User.Email)
                {
                    coordInfo.User.Email = updatedCoord.Email;
                    userManager.Update(coordInfo.User);
                }

                ViewBag.Updated = true;
                updatedCoord = new CoordUpdateModel(coordInfo);
            }

            var nocoordMajors = repo.GetWhere<Major>(m => m.Coordinator == null).OrderBy(m => m.MajorName);
            ViewBag.Majors = new SelectList(nocoordMajors, "MajorID", "MajorName");

            return View(updatedCoord);
        }
    }
}