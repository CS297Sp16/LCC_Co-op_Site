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

namespace Coop_Listing_Site.Controllers
{
    public class CoopController : Controller
    {
        private CoopContext db;

        public CoopController()
        {
            db = new CoopContext();
        }

        private User CurrentUser
        {
            get
            {
                return db.Users.Find(User.Identity.GetUserId());
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
            List<Opportunity> oppList = null;
            db.Majors.Load();
            db.Departments.Load();

            if (User.IsInRole("Student"))
            {
                var sInfo = db.Students.SingleOrDefault(si => si.User.Id == userId);

                if (sInfo != null)
                {
                    oppList = db.Opportunities.Where(
                        o => o.DepartmentID == sInfo.Major.Department.DepartmentID
                        ).ToList();
                }
            }
            else if (User.IsInRole("Admin"))
            {
                oppList = db.Opportunities.ToList();
            }
            else if (User.IsInRole("Coordinator"))
            {
                var cInfo = db.Coordinators.Include(c => c.User).SingleOrDefault(ci => ci.User.Id == CurrentUser.Id);
                if (cInfo != null)
                {
                    var depts = cInfo.Majors.Select(m => m.Department.DepartmentID);
                    var opps = from opp in db.Opportunities
                               where depts.Contains(opp.DepartmentID)
                               select opp;
                    oppList = opps.ToList();
                }
            }

            return View(oppList);
        }

        public ActionResult Details(int id)
        {
            return View(db.Opportunities.Find(id));
        }

        //GET: CoopController/AddOpportunity
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddOpportunity()
        {
            ViewBag.DepartmentIDs = new SelectList(db.Departments.ToList(), "DepartmentID", "DepartmentName");
            return View();
        }

        //POST: CoopController/AddOpportunity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddOpportunity([Bind(Include = @"OpportunityId, UserID, CompanyID, CompanyName,
            ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment,
            CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Duration, OpeningsAvailable, TermAvailable, DepartmentID")] OpportunityModel opportunityVM)
        {
            if (ModelState.IsValid)
            {
                Opportunity opportunity = new Opportunity()
                {
                    UserID = opportunityVM.UserID,
                    CompanyID = opportunityVM.CompanyID,
                    CompanyName = opportunityVM.CompanyName,
                    ContactName = opportunityVM.ContactName,
                    ContactNumber = opportunityVM.ContactNumber,
                    ContactEmail = opportunityVM.ContactEmail,
                    Location = opportunityVM.Location,
                    CompanyWebsite = opportunityVM.CompanyWebsite,
                    AboutCompany = opportunityVM.AboutCompany,
                    AboutDepartment = opportunityVM.AboutDepartment,
                    CoopPositionTitle = opportunityVM.CoopPositionTitle,
                    CoopPositionDuties = opportunityVM.CoopPositionDuties,
                    Qualifications = opportunityVM.Qualifications,
                    GPA = opportunityVM.GPA,
                    Paid = opportunityVM.Paid,
                    Duration = opportunityVM.Duration,
                    OpeningsAvailable = opportunityVM.OpeningsAvailable,
                    TermAvailable = opportunityVM.TermAvailable,
                    DepartmentID = opportunityVM.DepartmentID
                };
                db.Opportunities.Add(opportunity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentIDs = new SelectList(db.Departments.ToList());
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
            Opportunity opportunity = db.Opportunities.Find(id);
            if (opportunity == null)
            {
                return HttpNotFound();
            }
            return View(opportunity);
        }

        //POST: CoopController/EditOpportunity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditOpportunity([Bind(Include = @"OpportunityId, UserID, CompanyID, CompanyName,
            ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment,
            CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Duration, OpeningsAvailable,
            TermAvailable, DepartmentID")] OpportunityModel opportunity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opportunity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentIDs = new SelectList(db.Departments.ToList());
            return View(opportunity);
        }

        //GET: CoopController/DeleteOpportunity
        [Authorize(Roles = "Coordinator")]
        public ActionResult DeleteOpportunity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opportunity opportunity = db.Opportunities.Find(id);
            if (opportunity == null)
            {
                return HttpNotFound();
            }
            return View(opportunity);
        }

        //POST: CoopController/DeleteOpportunity
        [HttpPost, ActionName("DeleteOpportunity")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Opportunity opportunity = db.Opportunities.Find(id);
            db.Opportunities.Remove(opportunity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //retrieve a single opportunity
        private Opportunity GetOpportunity(int opportunityID)
        {
            return db.Opportunities.Find(opportunityID);
        }

        //retrieve all opportunities
        private List<Opportunity> GetOpportunities()
        {
            return db.Opportunities.ToList();
        }

        public ActionResult Upload(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Application application, HttpPostedFileBase ResumeUpload, HttpPostedFileBase CoverLetterUpload, HttpPostedFileBase DriverLicenseUpload, HttpPostedFileBase OtherUpload, int id)
        {
            if (ModelState.IsValid)
            {
                //Gets the opportunity that is being applied for
                var internship = db.Opportunities.Find(id);

                //Attaches the opportunity that the student is applying for to the application
                application.Opportunity = internship;

                //Attaches the current student to the application that is being submitted
                application.User = CurrentUser;

                //Allows for the upload of a resume
                if (ResumeUpload != null && ResumeUpload.ContentLength > 0)
                {
                    application.FileName_Resume = System.IO.Path.GetFileName(ResumeUpload.FileName);
                    application.Resume_ContentType = ResumeUpload.ContentType;
                    using (var reader = new System.IO.BinaryReader(ResumeUpload.InputStream))
                    {
                        application.Resume = reader.ReadBytes(ResumeUpload.ContentLength);
                    }
                }
                else
                {
                    ModelState.AddModelError("ResumeUpload", "A Resume is required");
                    return View();
                }

                //Allows for the upload of a cover letter
                if (CoverLetterUpload != null && CoverLetterUpload.ContentLength > 0)
                {
                    application.FileName_CoverLetter = System.IO.Path.GetFileName(CoverLetterUpload.FileName);
                    application.CoverLetter_ContentType = CoverLetterUpload.ContentType;

                    using (var reader = new System.IO.BinaryReader(CoverLetterUpload.InputStream))
                    {
                        application.CoverLetter = reader.ReadBytes(CoverLetterUpload.ContentLength);
                    }
                }

                //Saves the Drivers License
                if (DriverLicenseUpload != null && DriverLicenseUpload.ContentLength > 0)
                {
                    application.FileName_DriverLicense = System.IO.Path.GetFileName(DriverLicenseUpload.FileName);
                    application.DriverLicense_ContentType = DriverLicenseUpload.ContentType;
                    using (var reader = new System.IO.BinaryReader(DriverLicenseUpload.InputStream))
                    {
                        application.DriverLicense = reader.ReadBytes(DriverLicenseUpload.ContentLength);
                    }
                }

                //Saves anything else that might be needed into the other
                if (OtherUpload != null && OtherUpload.ContentLength > 0)
                {
                    application.FileName_Other = System.IO.Path.GetFileName(OtherUpload.FileName);
                    application.Other_ContentType = OtherUpload.ContentType;
                    using (var reader = new System.IO.BinaryReader(OtherUpload.InputStream))
                    {
                        application.Other = reader.ReadBytes(OtherUpload.ContentLength);
                    }
                }

                db.Applications.Add(application);
                db.SaveChanges();

                var email = db.Emails.FirstOrDefault();
                if(email != null)
                    email.SendApplicationNotification(application);

                return View("Submitted");
            }

            return View();

        }
        public ActionResult Applications()
        {
            string userId = User.Identity.GetUserId();
            List<Application> appList = null;
            db.Applications.Load();
            //db.Majors.Load();
            //db.Departments.Load();

            /*if (User.IsInRole("Coordinator"))
            {
                //var cInfo = db.Students.SingleOrDefault(si => si.User.Id == userId);

                if (sInfo != null)
                {
                    appList = db.Opportunities.Where(
                        o => o.DepartmentID == sInfo.Major.Department.DepartmentID
                        ).ToList();
                }
            }*/
            /*else if (User.IsInRole("Admin"))
            {
                appList = db.Opportunities.ToList();
            }*/
            if (User.IsInRole("Coordinator"))
            {
                var cInfo = db.Coordinators.Include(c => c.User).SingleOrDefault(ci => ci.User.Id == CurrentUser.Id);
                if (cInfo != null)
                {
                    //var depts = cInfo.Majors.Select(m => m.Department.DepartmentID);
                    var apps = from app in db.Applications
                                   //where depts.Contains(app.DepartmentID)
                               select app;
                    appList = apps.ToList();
                }
            }

            return View(appList);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
