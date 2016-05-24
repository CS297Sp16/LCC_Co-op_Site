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
                    var deptid = sInfo.Major.Department.DepartmentID;
                    oppList = db.Opportunities.Where(
                        o => o.Department.DepartmentID == deptid
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
                               where depts.Contains(opp.Department.DepartmentID)
                               select opp;
                    oppList = opps.ToList();
                }
            }

            return View(oppList);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opp = db.Opportunities.Find(id);

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
            ViewBag.DepartmentIDs = new SelectList(db.Departments.OrderBy(d => d.DepartmentName).ToList(), "DepartmentID", "DepartmentName");
            ViewBag.MajorIDs = new SelectList(db.Majors.OrderBy(m => m.MajorName).ToList(), "MajorID", "MajorName");
            return View();
        }

        //POST: CoopController/AddOpportunity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddOpportunity([Bind(Include = @"CompanyName,
            ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment,
            CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Wage, Amount, Duration, OpeningsAvailable, TermAvailable")] OpportunityModel opportunityVM, int? DepartmentIDs, int? MajorIDs)
        {
            Major major = null;
            Department dept = null;

            if (DepartmentIDs == null && MajorIDs == null)
            {
                ModelState.AddModelError("", "You must select either a Department or at least on Major to list this opportunity under");
            }
            else
            {
                if (MajorIDs != null)
                    major = db.Majors.Find(MajorIDs);
                else
                    dept = db.Departments.Find(DepartmentIDs);
            }

            if (ModelState.IsValid)
            {
                Opportunity opportunity = opportunityVM.ToOpportunity();
                opportunity.Approved = true;

                if (major != null)
                    opportunity.Majors.Add(major);
                else
                    opportunity.Department = dept;

                if (opportunity.Wage != null || opportunity.Amount != null)
                    opportunity.Paid = true; // Implement in JS using a Hidden field that to True/False depending on the pay type

                db.Opportunities.Add(opportunity);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentIDs = new SelectList(db.Departments.OrderBy(d => d.DepartmentName).ToList(), "DepartmentID", "DepartmentName");
            ViewBag.MajorIDs = new SelectList(db.Majors.OrderBy(m => m.MajorName).ToList(), "MajorID", "MajorName");
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

            ViewBag.MajorIDs = new SelectList(db.Majors.OrderBy(m => m.MajorName).ToList(), "MajorID", "MajorName");
            var oppvm = new OpportunityModel(opportunity);
            return View(oppvm);
        }

        //POST: CoopController/EditOpportunity
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditOpportunity([Bind(Include = @"OpportunityId, CompanyName,
            ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment,
            CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Wage, Amount, Duration, OpeningsAvailable, TermAvailable")] OpportunityModel opportunityvm, int? MajorIDs)
        {
            if (ModelState.IsValid)
            {
                Major major = null;
                if (MajorIDs != null)
                    major = db.Majors.Find(MajorIDs);

                var opportunity = db.Opportunities.Find(opportunityvm.OpportunityID);

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

                db.Entry(opportunity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var opp = db.Opportunities.Find(opportunityvm.OpportunityID);

            opportunityvm.Department = opp.Department;
            opportunityvm.Majors = opp.Majors;

            ViewBag.MajorIDs = new SelectList(db.Majors.OrderBy(m => m.MajorName).ToList(), "MajorID", "MajorName");
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
            Opportunity opportunity = db.Opportunities.Find(id);
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
        public ActionResult AppDetails(int id)
        {
            return View(db.Applications.Find(id));
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult AppDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
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
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Applications");
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
