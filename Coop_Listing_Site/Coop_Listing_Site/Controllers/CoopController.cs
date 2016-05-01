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

            if (User.IsInRole("Student"))
            {
                var sInfo = db.Students.SingleOrDefault(si => si.UserId == userId);

                if (sInfo != null)
                {
                    var sMajor = db.Majors.Find(sInfo.MajorID);
                    oppList = db.Opportunities.Where(
                        o => o.DepartmentID == sMajor.DepartmentID
                        ).ToList();
                }
            }
            else if (User.IsInRole("Admin"))
            {
                oppList = db.Opportunities.ToList();
            }
            else if (User.IsInRole("Coordinator"))
            {
                var cInfo = db.Coordinators.SingleOrDefault(ci => ci.UserId == userId);
                if (cInfo != null)
                {
                    var depts = cInfo.Departments.Select(d => d.DepartmentID);
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
            //not sure if I should have a viewbag with users, and one for opportunities here
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
                    GPA =opportunityVM.GPA,
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
            //not sure if I should have a viewbag with users, and one for opportunities here
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
            if(opportunity == null)
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
            //not sure if I should have a viewbag with users, and one for opportunities here
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

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Application application, HttpPostedFileBase R, HttpPostedFileBase CL)
        {
            if (ModelState.IsValid)
            {
                if (R != null && R.ContentLength > 0)
                {
                    application.FileName_Resume = System.IO.Path.GetFileName(R.FileName);
                    application.Resume_ContentType = R.ContentType;
                    using (var reader = new System.IO.BinaryReader(R.InputStream))
                    {
                        application.Resume = reader.ReadBytes(R.ContentLength);
                    }
                }
                
                if (CL != null && CL.ContentLength > 0)
                {
                    application.FileName_CoverLetter = System.IO.Path.GetFileName(CL.FileName);
                    application.CoverLetter_ContentType = R.ContentType;
                    
                    using (var reader = new System.IO.BinaryReader(CL.InputStream))
                    {
                        application.CoverLetter = reader.ReadBytes(CL.ContentLength);
                    }
                }                               
            }
            db.Applications.Add(application);
            db.SaveChanges();

            return View("Submitted");
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
