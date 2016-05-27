using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;
using System.Net;
using System.Data.Entity;
using Coop_Listing_Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Coop_Listing_Site.Controllers
{
    public class CoopController : Controller
    {
        private IRepository repo;

        public CoopController()
        {
            var db = new CoopContext();
            repo = new Repository(db);
        }

        public CoopController(IRepository r)
        {
            repo = r;
        }

        private User CurrentUser
        {
            get
            {
                return repo.GetByID<User>(User.Identity.GetUserId());
            }
        }

        // GET: Coop
        public ActionResult Index()
        {
            return RedirectToAction("Listings");
        }

        public ActionResult Listings()
        {
            string userId = User.Identity.GetUserId();
            IEnumerable<Opportunity> oppList = new List<Opportunity>(); // Setting it to null causes problems on the off chance someone accesses the page without being in one of the following roles

            if (User.IsInRole("Student"))
            {
                var sInfo = repo.GetOne<StudentInfo>(si => si.User.Id == userId);

                if (sInfo != null)
                {
                    var dept = repo.GetOne<Department>(o => o.Majors.Contains(sInfo.Major));
                    oppList = repo.GetWhere<Opportunity>(
                        o => o.Department.DepartmentID == dept.DepartmentID
                        );
                }
            }
            else if (User.IsInRole("Admin"))
            {
                oppList = repo.GetAll<Opportunity>();
            }
            else if (User.IsInRole("Coordinator"))
            {
                var cInfo = repo.GetOne<CoordinatorInfo>(ci => ci.User.Id == userId);

                if (cInfo != null)
                {
                    var depts = cInfo.Majors.Select(m => m.Department.DepartmentID);

                    oppList = repo.GetWhere<Opportunity>(o => depts.Contains(o.Department.DepartmentID));
                }
            }

            return View(oppList.Select(o => new OpportunityModel(o)));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opp = repo.GetByID<Opportunity>(id);

            if (opp == null)
            {
                return HttpNotFound();
            }

            var oppvm = new OpportunityModel(opp);
            return View(oppvm);
        }

        //GET: CoopController/AddOpportunity
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddOpportunity()
        {
            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>(), "DepartmentID", "DepartmentName");
            ViewBag.Majors = new SelectList(repo.GetAll<Major>(), "MajorID", "MajorName");
            return View();
        }

        //POST: CoopController/AddOpportunity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddOpportunity([Bind(Include = @"CompanyName,
            ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment,
            CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Wage, Amount, Duration, OpeningsAvailable, TermAvailable")] OpportunityModel opportunityVM, int? DepartmentIDs, int[] MajorIDs)

        {
            Department dept = null;
            var majorList = new List<Major>();

            if (DepartmentIDs == null && MajorIDs == null)
            {
                ModelState.AddModelError("", "You must select either a Department or at least one Major to list this opportunity under");
            }
            else
            {
                if (MajorIDs.Length > 0)
                {
                    foreach (var majorid in MajorIDs)
                    {
                        var major = repo.GetByID<Major>(majorid);
                        if (!majorList.Contains(major))
                            majorList.Add(major);
                    }
                    opportunityVM.Majors = majorList;
                }
                else
                {
                    dept = repo.GetByID<Department>(DepartmentIDs);
                    opportunityVM.Department = dept;
                }
            }

            if (ModelState.IsValid)
            {
                Opportunity opportunity = opportunityVM.ToOpportunity();
                opportunity.Approved = true;

                if (opportunity.Wage != null || opportunity.Amount != null)
                    opportunity.Paid = true; // Implement in JS using a Hidden field that to True/False depending on the pay type

                repo.Add(opportunity);

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>(), "DepartmentID", "DepartmentName");

            return View(opportunityVM);
        }

        //GET: CoopController/EditOpportunity
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditOpportunity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opportunity opportunity = repo.GetByID<Opportunity>(id);
            if (opportunity == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>(), "DepartmentID", "DepartmentName");

            var oppvm = new OpportunityModel(opportunity);
            return View(oppvm);
        }

        //POST: CoopController/EditOpportunity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditOpportunity([Bind(Include = @"OpportunityId, CompanyName,
            ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment,
            CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Wage, Amount, Duration, OpeningsAvailable, TermAvailable")] OpportunityModel opportunityvm, int? DepartmentIDs, int[] MajorIDs)
        {
            Department dept = null;
            var majorList = new List<Major>();

            if (DepartmentIDs == null && MajorIDs == null)
            {
                ModelState.AddModelError("", "You must select either a Department or at least one Major to list this opportunity under");
            }
            else
            {
                if (MajorIDs.Length > 0)
                {
                    foreach (var majorid in MajorIDs)
                    {
                        var major = repo.GetByID<Major>(majorid);
                        if (!majorList.Contains(major))
                            majorList.Add(major);
                    }
                    opportunityvm.Majors = majorList;
                }
                else
                {
                    dept = repo.GetByID<Department>(DepartmentIDs);
                    opportunityvm.Department = dept;
                }
            }

            if (ModelState.IsValid)
            {

                var opportunity = opportunityvm.ToOpportunity();

                /*
                // This should be changed but I needed to get something working
                opportunity.AboutCompany = opportunityvm.AboutCompany;
                opportunity.AboutDepartment = opportunityvm.AboutDepartment;
                opportunity.Amount = opportunityvm.Amount;
                opportunity.Approved = opportunityvm.Approved;
                opportunity.CompanyName = opportunityvm.CompanyName;
                opportunity.CompanyWebsite = opportunityvm.CompanyWebsite;
                opportunity.ContactEmail = opportunityvm.ContactEmail;
                opportunity.ContactName = opportunityvm.ContactName;
                opportunity.ContactNumber = opportunityvm.ContactNumber;
                opportunity.CoopPositionDuties = opportunityvm.CoopPositionDuties;
                opportunity.CoopPositionTitle = opportunityvm.CoopPositionTitle;
                opportunity.Duration = opportunityvm.Duration;
                opportunity.GPA = opportunityvm.GPA;
                opportunity.Location = opportunityvm.Location;
                opportunity.OpeningsAvailable = opportunityvm.OpeningsAvailable;
                opportunity.Paid = opportunityvm.Paid;
                opportunity.Qualifications = opportunityvm.Qualifications;
                opportunity.TermAvailable = opportunityvm.TermAvailable;
                opportunity.Wage = opportunityvm.Wage;

                if(major != null)
                    opportunity.Majors.Add(major); // Needs more logic for adding/removing majors
                */

                repo.Update(opportunity);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>(), "DepartmentID", "DepartmentName");

            return View(opportunityvm);
        }

        //GET: CoopController/DeleteOpportunity
        [Authorize(Roles = "Coordinator")]
        public ActionResult DeleteOpportunity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opportunity opportunity = repo.GetByID<Opportunity>(id);
            if (opportunity == null)
            {
                return HttpNotFound();
            }

            var oppvm = new OpportunityModel(opportunity);
            return View(oppvm);
        }

        //POST: CoopController/DeleteOpportunity
        [HttpPost, ActionName("DeleteOpportunity")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Opportunity opportunity = repo.GetByID<Opportunity>(id);
            opportunity.Majors.Clear();
            repo.Delete(opportunity);

            return RedirectToAction("Index");
        }

        public ActionResult Upload(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Application application, int id)
        {
            // Check if the user applying is even a student
            var student = repo.GetOne<StudentInfo>(s => s.User.Id == CurrentUser.Id);

            if (student == null)
                ModelState.AddModelError("", "You must be a student to apply for a Co-op Opportunity.");

            var files = HttpContext.Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                var uploadedFile = files.Get(i);
                var slot = files.GetKey(i);

                if (uploadedFile.ContentLength > 0)
                {
                    var file = new UserFile();
                    file.ContentType = uploadedFile.ContentType;
                    file.FileName = uploadedFile.FileName;

                    using (var stream = new BinaryReader(uploadedFile.InputStream))
                        file.FileData = stream.ReadBytes(uploadedFile.ContentLength);

                    application.Files.Add(file);
                }
                else
                {
                    if (slot == "ResumeUpload")
                    {
                        ModelState.AddModelError("", "A Resume is required to apply for an opportunity.");
                        break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                //Gets the opportunity that is being applied for
                var internship = repo.GetByID<Opportunity>(id);

                //Attaches the opportunity that the student is applying for to the application
                application.Opportunity = internship;

                repo.Add<Application>(application);

                var email = repo.GetOne<EmailInfo>();
                if(email != null)
                    email.SendApplicationNotification(application);

                return View("Submitted");
            }

            return View();

        }

        public ActionResult Applications()
        {
            string userId = User.Identity.GetUserId();
            IEnumerable<Application> appList = null;

            if (User.IsInRole("Coordinator"))
            {
                var cInfo = repo.GetOne<CoordinatorInfo>(ci => ci.User.Id == CurrentUser.Id);
                if (cInfo != null)
                {
                    appList = repo.GetAll<Application>();
                }
            }

            return View(appList);
        }
        public ActionResult AppDetails(int id)
        {
            return View(repo.GetByID<Application>(id));
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult AppDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = repo.GetByID<Application>(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        //POST: CoopController/DeleteOpportunity
        [HttpPost, ActionName("AppDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult AppDeleteConfirmed(int id)
        {
            Application application = repo.GetByID<Application>(id);
            repo.Delete(application);
            return RedirectToAction("Applications");
        }

        public ActionResult GetFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var file = repo.GetByID<UserFile>(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            return File(file.FileData, file.ContentType);
        }

        [HttpPost]
        public ActionResult GetMajorsForDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dept = repo.GetByID<Department>(id);
            if (dept == null)
            {
                return HttpNotFound();
            }

            // Majors link to Departments, which have a list of Majors, which link to a Department, etc.
            // This causes the Json serialization to fail, so make a list of dictionaries instead
            var majors = repo.GetWhere<Major>(m => m.Department == dept);
            var majordict = new List<Dictionary<string, string>>();

            foreach(var major in majors)
            {
                var dict = new Dictionary<string, string>();
                dict["MajorName"] = major.MajorName;
                dict["MajorID"] = major.MajorID.ToString();
                majordict.Add(dict);
            }

            return Json(majordict);
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
