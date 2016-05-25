﻿using System;
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
            IEnumerable<Opportunity> oppList = null;

            if (User.IsInRole("Student"))
            {
                var sInfo = repo.GetOne<StudentInfo>(si => si.User.Id == userId);

                if (sInfo != null)
                {
                    var dept = repo.GetOne<Department>(o => o.Majors.Contains(sInfo.Major));
                    oppList = repo.GetWhere<Opportunity>(
                        o => o.DepartmentID == dept.DepartmentID
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
                    //var opps = from opp in repo.GetAll<Opportunity>()
                    //           where depts.Contains(opp.DepartmentID)
                    //           select opp;

                    oppList = repo.GetWhere<Opportunity>(o => depts.Contains(o.DepartmentID));
                }
            }

            return View(oppList);
        }

        public ActionResult Details(int id)
        {
            return View(repo.GetByID<Opportunity>(id));
        }

        //GET: CoopController/AddOpportunity
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddOpportunity()
        {
            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>(), "DepartmentID", "DepartmentName");
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

                repo.Add(opportunity);

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>());
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
                // TODO: figure out what Gabe was thinking when he wrote this
                //db.Entry(opportunity).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            // TODO: actually use this, instead of dumping it on the floor
            ViewBag.DepartmentIDs = new SelectList(repo.GetAll<Department>());
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
            Opportunity opportunity = repo.GetByID<Opportunity>(id);
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
            Opportunity opportunity = repo.GetByID<Opportunity>(id);
            repo.Delete(opportunity);

            return RedirectToAction("Index");
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
                var internship = repo.GetByID<Opportunity>(id);

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

                repo.Add<Application>(application);

                var email = repo.GetOne<EmailInfo>();
                if(email != null)
                    email.SendApplicationNotification(application);

                return View("Submitted");
            }

            return View();

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
