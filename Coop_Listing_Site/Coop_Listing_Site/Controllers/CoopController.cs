using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;//added this
using Coop_Listing_Site.DAL;//added this
using System.Net;//added this
using System.Data.Entity;//added this, are we using something different?

namespace Coop_Listing_Site.Controllers
{
    public class CoopController : Controller
    {
        // This controller will have List, Details, and possibly Create, Delete, and Edit for all co-op opportunities

        private CoopContext db = new CoopContext();

        // GET: Coop
        public ActionResult Index()
        {
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
        public ActionResult AddOpportunity([Bind(Include = "OpportunityId, UserID, CompanyID, PDF, OpeningsAvailable, TermAvailable")] Opportunity opportunityVM)
        {
            if (ModelState.IsValid)
            {
                Opportunity opportunity = new Opportunity()
                {
                    UserID = opportunityVM.UserID,
                    CompanyID = opportunityVM.CompanyID,
                    PDF = opportunityVM.PDF,
                    OpeningsAvailable = opportunityVM.OpeningsAvailable,
                    TermAvailable = opportunityVM.TermAvailable
                };
                opportunity.Opportunities.Add(opportunity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //not sure if I should have a viewbag with users, and one for opportunities here 
            return View();
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
        public ActionResult EditOpportunity([Bind(Include = "OpportunityId, UserID, CompanyID, PDF, OpeningsAvailable, TermAvailable")] Opportunity opportunity)
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
            db.Opportunities.Remove(opportunity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //not sure if we need this, but here it is just in case
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //not sure if maybe these should be changed to use linq to get the information
        //retrieve a single opportunity
        private Opportunity GetOpportunity(int? opportunityID)
        {
            Opportunity opportunity = null;

            foreach(Opportunity o in db.Opportunities)//I'm curious if the db will just make it plural or change to opportunites
            {
                if (o.OpportunityID == opportunityID)
                {
                    opportunity = new Opportunity()
                    {
                        OpportunityID = o.OpportunityID,
                        UserID = o.UserID,
                        CompanyID = o.CompanyID,
                        PDF = o.PDF,
                        OpeningsAvailable = o.OpeningsAvailable,
                        TermAvailable = o.TermAvailable
                    };
                }
            }
            return opportunity;
        }

        //not sure if maybe these should be changed to use linq to get the information
        //retrieve all opportunities
        private List<Opportunity> GetOpportunities()
        {
            var opportunities = new List<Opportunity>();
            foreach(Opportunity o in db.Opportunities)//I'm curious if the db will just make it plural or change to opportunites
            {
                var opportunity = new Opportunity();
                opportunity.OpportunityID = o.OpportunityID;
                opportunity.UserID = o.UserID;
                opportunity.CompanyID = o.CompanyID;
                opportunity.PDF = o.PDF;
                opportunity.OpportunityID = o.OpportunityID;
                opportunity.TermAvailable = o.TermAvailable;
                opportunities.Add(opportunity);
            }
            return opportunities;
        }
    }
}