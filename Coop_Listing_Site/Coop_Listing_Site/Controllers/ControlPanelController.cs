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
using Coop_Listing_Site.Models.ViewModels;
using System.Diagnostics;
using System.Data.Entity;

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
            var currentUser = CurrentUser;
            return View(currentUser);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SMTP()
        {
            return View();
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public ActionResult SMTP(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain)
        {
            EmailInfo.SMTPAddress = SMTPAddress;
            EmailInfo.SMTPAccountName = SMTPUser;
            EmailInfo.SMTPPassword = SMTPPassword;
            EmailInfo.InviteEmail = InviteEmail;
            EmailInfo.Domain = Regex.Replace(Domain, "^https?://", "", RegexOptions.IgnoreCase);

            return View();
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult Invite()
        {
            ViewBag.SMTPReady = EmailInfo.ProperlySet;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public ActionResult Invite([Bind(Include = "Email,UserType")] RegisterInvite invitation)
        {
            ViewBag.SMTPReady = EmailInfo.ProperlySet;
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

            ViewBag.ReturnMessage = invitation.SendInvite();

            return View();
        }

        //public ActionResult UpdateStudent([Bind(Include = "UserId,GPA,MajorID,Password,ConfirmPassword")] StudentUpdateModel studentUpdateModel)
        //{
        //    //db.Database.Log = Console.Write;
        //    var studentInfo = db.Students
        //        .Where(si => si.UserId.Equals(CurrentUser.Id));

        //    var student = CurrentUser;

        //    if (!ModelState.IsValid) return View();

        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(member).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    User user = new User
        //    {
        //        FirstName = studentUpdateModel.FirstName,
        //        LastName = studentUpdateModel.LastName,
        //        Email = studentUpdateModel.Email,
        //        Enabled = true,
        //        UserName = student.Email // Cannot create user without a user name. We don't actually use user names, so just set it to the Email field.
        //    };
        //}
        private User CurrentUser
        {
            get
            {
                return db.Users.Find(User.Identity.GetUserId());
            }
        }
    }
}