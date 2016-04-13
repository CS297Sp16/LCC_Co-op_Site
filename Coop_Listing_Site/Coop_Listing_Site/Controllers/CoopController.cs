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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Coop_Listing_Site.Controllers
{
    public class CoopController : Controller
    {
        // This controller will have List, Details, and possibly Create, Delete, and Edit for all co-op opportunities

        private CoopContext db;
        private UserManager<User> userManager;

        public CoopController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        private User CurrentUser
        {
            get
            {
                return db.Users.Single(u => u.UserName == User.Identity.Name);
            }
        }

        // GET: Coop
        public ActionResult Index()
        {
            return RedirectToAction("Listings");
        }

        public ActionResult Listings()
        {
            var sInfo = db.Students.SingleOrDefault(si => si.UserId == CurrentUser.Id);

            if (sInfo != null)
            {
                var sMajor = db.Majors.Find(sInfo.MajorID);
                var x = db.Opportunities.Where(o => o.DepartmentID == sMajor.DepartmentID);
                return View(x.ToList());
            }

            return View();
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
        public ActionResult AddOpportunity([Bind(Include = "OpportunityId, UserID, CompanyID, CompanyName, ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment, CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Duration, OpeningsAvailable, TermAvailable")] OpportunityModel opportunityVM)
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
                    TermAvailable = opportunityVM.TermAvailable
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
        public ActionResult EditOpportunity([Bind(Include = "OpportunityId, UserID, CompanyID, CompanyName, ContactName, ContactNumber, ContactEmail, Location, CompanyWebsite, AboutCompany, AboutDepartment, CoopPositionTitle, CoopPositionDuties, Qualifications, GPA, Paid, Duration, OpeningsAvailable, TermAvailable")] OpportunityModel opportunity)
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
            return RedirectToAction("Index");
        }

        //POST: CoopController/DeleteOpportunity
        [HttpPost, ActionName("DeleteOpportunity")]
        [ValidateAntiForgeryToken]
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();

                if (userManager != null)
                    userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
