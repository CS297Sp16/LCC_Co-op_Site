using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;
using Coop_Listing_Site.Models.ViewModels;
using System.Data.Entity;
using System.Net;

namespace Coop_Listing_Site.Controllers
{
    public class ControlPanelController : Controller
    {
        private CoopContext db;
        private UserManager<User> userManager;

        public ControlPanelController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        public ActionResult Index()
        {
            // consider using this page to display relevant user specific information
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SMTP()
        {
            var email = db.Emails.FirstOrDefault();
            if (email != null)
                ViewBag.EmailInfo = email;

            return View();
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult SMTP(string SMTPAddress, string SMTPUser, string SMTPPassword, string Domain)
        {
            var email = db.Emails.FirstOrDefault();

            if (email == null)
            {
                email = new EmailInfo();

                email.SMTPAddress = SMTPAddress;
                email.SMTPAccountName = SMTPUser;
                email.SMTPPassword = SMTPPassword;
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
                email.Domain = Regex.Replace(Domain, "^https?://", "", RegexOptions.IgnoreCase);

                db.Entry(email).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "Email information successfully updated";
            }

            return View();
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult InviteStudent()
        {
            var emailInfo = db.Emails.FirstOrDefault();

            ViewBag.SMTPReady = (emailInfo != null) ? emailInfo.ProperlySet : false;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult InviteStudent(string Emails)
        {
            var emailInfo = db.Emails.FirstOrDefault();

            ViewBag.SMTPReady = (emailInfo != null) ? emailInfo.ProperlySet : false;

            if (!ModelState.IsValid) return View();

            var emailArray = Emails.Split('\n');

            var messages = new List<string>();
            int line = 0;
            int failures = 0; // Just like you <3

            foreach (var email in emailArray)
            {
                line++;
                var trimEmail = email.Trim();

                if (string.IsNullOrWhiteSpace(trimEmail))
                    continue;

                var isValidEmail = Regex.IsMatch(trimEmail,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase);

                if (!isValidEmail)
                {
                    messages.Add(string.Format("The email {0} on line {1} is not a valid email.", trimEmail, line));
                    failures++;
                    continue;
                }

                var emailCheck = db.Invites.FirstOrDefault(i => i.Email.ToLower() == trimEmail.ToLower());

                if (emailCheck != null)
                {
                    messages.Add(string.Format("An invitation has already been sent to {0}.", trimEmail));
                    failures++;
                    continue;
                }

                var user = userManager.FindByEmail(trimEmail);

                if (user != null)
                {
                    messages.Add(string.Format("A user with the e-mail {0} already exists.", trimEmail));
                    failures++;
                    continue;
                }

                var invitation = new RegisterInvite();
                invitation.Email = trimEmail;
                invitation.UserType = RegisterInvite.AccountType.Student;
                invitation.RegisterInviteID = Guid.NewGuid().ToString("N");

                db.Invites.Add(invitation);
                db.SaveChanges();

                var success = emailInfo.SendInviteEmail(invitation).Keys.First();

                if (!success)
                {
                    db.Invites.Remove(invitation);
                    db.SaveChanges();
                    messages.Add(string.Format("Failed to send invite to {0}", trimEmail));
                    failures++;
                }
            }

            if (failures == 0)
                messages.Add("Invite(s) successfully sent.");
            else if (failures < emailArray.Length)
                messages.Add("All other invites successfully sent.");
            else
                messages.Add("It appears none of the invites were sent successfully.");


            ViewBag.ReturnMessages = messages;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult InviteCoordinator()
        {
            var emailInfo = db.Emails.FirstOrDefault();

            ViewBag.SMTPReady = (emailInfo != null) ? emailInfo.ProperlySet : false;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult InviteCoordinator([Bind(Include = "Email")] RegisterInvite invitation)
        {
            var emailInfo = db.Emails.FirstOrDefault();

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

            invitation.UserType = RegisterInvite.AccountType.Coordinator;
            invitation.RegisterInviteID = Guid.NewGuid().ToString("N");
            db.Invites.Add(invitation);
            db.SaveChanges();

            var response = emailInfo.SendInviteEmail(invitation);
            var success = response.Keys.First();

            if (!success)
            {
                db.Invites.Remove(invitation);
                db.SaveChanges();
            }

            ViewBag.ReturnMessage = response[success];

            return View();
        }

        private User CurrentUser
        {
            get
            {
                return db.Users.Find(User.Identity.GetUserId());
            }
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
