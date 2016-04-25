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
using System.Net;

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

            //var passVerification = userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, studentUpdateModel.Password);
                     
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

        //GET: ControlPanelController/AddDepartment
        [Authorize(Roles = "Coordinator")]
        public ActionResult AddDepartment()
        {
            //not sure if we need the viewBag or not, delete if not needed
            ViewBag.Departments = new SelectList(db.Departments.OrderBy(d => d.DepartmentName), "DepartmentID", "DepartmentName");
            return View();
        }

        //POST: ControlPanelController/AddDepartment
        [Authorize(Roles = "Coordinator")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddDepartment([Bind(Include = "DepartmentID, DepartmentName, Majors")] DepartmentModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department()
                {
                    DepartmentName = departmentVM.DepartmentName,
                    Majors = departmentVM.Majors
                };
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //not sure if we need the viewBag or not, delete if not needed
            ViewBag.Departments = new SelectList(db.Departments.OrderBy(d => d.DepartmentName), "DepartmentID", "DepartmentName");
            return View(departmentVM);
        }

        //GET: ControlPAnelController/EditDepartment
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //POST: ControlPAnelController/EditDepartment
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditDepartment ([Bind(Include = "DepartmentID, DepartmentName, Majors")] Department dept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //not sure if we need the viewBag or not, delete if not needed
            ViewBag.Departments = new SelectList(db.Departments.OrderBy(d => d.DepartmentName), "DepartmentID", "DepartmentName");
            return View(dept);
        }

        //GET: ControlPAnelController/DeleteDepartment
        [Authorize(Roles = "Coordinator")]
        public ActionResult DeleteDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //POST: ControlPanelController/DeleteDepartment
        [HttpPost, ActionName("DeleteDepartment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: ControlPanelController/Details
        public ActionResult DepartmentDetails(int id)
        {
            return View(db.Departments.Find(id));
        }

        private User CurrentUser
        {
            get
            {
                return db.Users.Find(User.Identity.GetUserId());
            }
        }
    }
}