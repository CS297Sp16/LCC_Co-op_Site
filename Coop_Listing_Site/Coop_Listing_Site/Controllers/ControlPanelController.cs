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

<<<<<<< HEAD
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
=======
namespace Coop_Listing_Site.Controllers
{
    public class ControlPanelController : Controller, IControlPanelRepository
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
>>>>>>> parent of 0216dd2... forgot to take out the interface in the controller
        }

<<<<<<< HEAD
        [HttpGet, ValidateAntiForgeryToken]
        public ActionResult UpdateStudent()
        {
            ViewBag.Majors = new SelectList(db.Majors.ToList(), "MajorID", "MajorName");

            return View();
        }

       [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateStudent([Bind(Include = "UserId,GPA,MajorID,Password,ConfirmPassword")] StudentUpdateModel studentUpdateModel)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

            var studInfo = db.Students.FirstOrDefault(si => si.UserId == user.Id);

            var major = db.Majors.FirstOrDefault(mj => mj.MajorID == studInfo.MajorID);

            var passwordValidated = userManager.CheckPassword(user, studentUpdateModel.CurrentPassword);
                     
            if (!ModelState.IsValid) return View();

            if (ModelState.IsValid)
            {
                studInfo.GPA = studentUpdateModel.GPA;
                studInfo.MajorID = studentUpdateModel.MajorID;

                if(major.MajorID != studentUpdateModel.MajorID)
                {
                    studInfo.MajorID = studentUpdateModel.MajorID;
                }

                if(passwordValidated && studentUpdateModel.NewPassword == studentUpdateModel.ConfirmNewPassword)
                {
                    userManager.ChangePassword(user.Id, studentUpdateModel.CurrentPassword,studentUpdateModel.NewPassword);                   
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(studInfo).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            return RedirectToAction("Index");
        }
=======
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
>>>>>>> parent of ba0b1d3... UpdateSudent ActionResult
        private User CurrentUser
        {
            get
            {
                return db.Users.Find(User.Identity.GetUserId());
            }
        }
    }
}