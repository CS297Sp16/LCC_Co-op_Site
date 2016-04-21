using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;

namespace Coop_Listing_Site.Controllers
{
    public class ControlPanelController : Controller
    {
        // Might need a rename. This will have a Student and Advisor action result to start, which will point to their views.
        // GET: ControlPanel
        private CoopContext db;
        private UserManager<User> userManager;

        public ControlPanelController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Majors()
        {
            var majors = db.Majors.OrderBy(m => m.DepartmentID);

            return View(majors);
        }

        public ActionResult AddMajor()
        {
            var depts = db.Departments.OrderBy(d => d.DepartmentName);

            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View();
        }

        [HttpPost]
        public ActionResult AddMajor([Bind(Include = "DepartmentID, MajorName")] Major major)
        {
            if (ModelState.IsValid)
            {
                db.Majors.Add(major);
                db.SaveChanges();

                return View("Majors");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SMTP()
        {
            return View();
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult SMTP(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain)
        {
            var email = db.Emails.FirstOrDefault(e => e.SendAsEmail != "");

            if (email == null)
            {
                email = new EmailInfo();

                email.SMTPAddress = SMTPAddress;
                email.SMTPAccountName = SMTPUser;
                email.SMTPPassword = SMTPPassword;
                email.SendAsEmail = InviteEmail;
                email.Domain = Regex.Replace(Domain, "^https?://", "", RegexOptions.IgnoreCase);

                db.Emails.Add(email);
                db.SaveChanges();
                ViewBag.Message = "Email information successfully set.";
            }
            else
            {
                email.SMTPAddress = SMTPAddress;
                email.SMTPAccountName = SMTPUser;
                email.SMTPPassword = SMTPPassword;
                email.SendAsEmail = InviteEmail;
                email.Domain = Regex.Replace(Domain, "^https?://", "", RegexOptions.IgnoreCase);

                db.Entry(email).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "Email information successfully updated";
            }

            return View();
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult Invite()
        {
            var emailInfo = db.Emails.FirstOrDefault(e => e.SendAsEmail != "");

            ViewBag.SMTPReady = (emailInfo != null) ? emailInfo.ProperlySet : false;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult Invite([Bind(Include = "Email,UserType")] RegisterInvite invitation)
        {
            var emailInfo = db.Emails.FirstOrDefault(e => e.SendAsEmail != "");

            ViewBag.SMTPReady = (emailInfo != null) ? emailInfo.ProperlySet : false;

            if (!ModelState.IsValid) return View();

            var email = db.Invites.FirstOrDefault(i => i.Email.ToLower() == invitation.Email.ToLower());
            if (email != null)
            {
                ModelState.AddModelError("Email", "An invitation has already been sent to that e-mail!");
                return View();
            }

            var user = userManager.FindByEmail(invitation.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with that e-mail already exists!");
                return View();
            }

            invitation.RegisterInviteID = Guid.NewGuid().ToString("N");
            db.Invites.Add(invitation);
            db.SaveChanges();

            var response = invitation.SendInvite(emailInfo);
            var success = response.Keys.First();

            if (!success)
            {
                db.Invites.Remove(invitation);
                db.SaveChanges();
            }

            ViewBag.ReturnMessage = response[success];

            return View();
        }
    }
}