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
using Coop_Listing_Site.Repositories;

namespace Coop_Listing_Site.Controllers
{
    public class ControlPanelController : Controller
    {
        // Look into separating this controller into multiple. One for Student, Coordinator, and Admin. Maybe rename to cpanel to shorten the URL length, too
        private CoopContext db;
        private UserManager<User> userManager;

        //IControlPanelRepository icpr; //uncomment for testing

        public ControlPanelController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));

            // icpr = new ControlPanelRepository(); //uncomment for testing
        }

        /* public ControlPanelController(IControlPanelRepository contPanel)
         {
             icpr = contPanel;  //uncomment for testing
         }*/

        public ActionResult Index()
        {
            var currentUser = CurrentUser;
            return View(currentUser);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Majors()
        {
            var majors = db.Majors.Include(m => m.Department).OrderBy(m => m.Department.DepartmentName);

            return View(majors);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult AddMajor()
        {
            var depts = db.Departments.OrderBy(d => d.DepartmentName);

            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View();
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult AddMajor([Bind(Include = "DepartmentID, MajorName")] Major major)
        {
            if (ModelState.IsValid)
            {
                db.Majors.Add(major);
                db.SaveChanges();

                return RedirectToAction("Majors");
            }

            return View();
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult EditMajor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var depts = db.Departments.OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            // for if/when we add courses
            //var courses = db.Courses.OrderBy(c => c.CourseNumber);
            //var selectedCourses = db.Majors.Find(id).Courses.Select(c => c.CourseNumber);
            //ViewBag.Courses = new MultiSelectList(courses, "CourseID", "CourseNumber", selectedCourses);

            return View(db.Majors.Find(id));
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator"), ValidateAntiForgeryToken]
        public ActionResult EditMajor([Bind(Include = "MajorID, DepartmentID, MajorName")] Major major)
        {
            if (ModelState.IsValid)
            {
                db.Entry(major).State = EntityState.Modified;
                db.SaveChanges();
            }

            var depts = db.Departments.OrderBy(d => d.DepartmentName);
            ViewBag.Departments = new SelectList(depts, "DepartmentID", "DepartmentName");

            return View(major);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult DeleteMajor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }

        [HttpPost, ActionName("DeleteMajor"), Authorize(Roles = "Admin, Coordinator"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteMajor(int id)
        {
            Major major = db.Majors.Find(id);
            db.Majors.Remove(major);
            db.SaveChanges();

            return RedirectToAction("Majors");
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
        public ActionResult SMTP(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain)
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
        public ActionResult InviteStudent([Bind(Include = "Email")] RegisterInvite invitation)
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

            invitation.UserType = RegisterInvite.AccountType.Student;
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

        public ActionResult InviteList()
        {
            return View(db.Invites);
        }

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult Rescind(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterInvite inv = db.Invites.Find(id);
            if (inv == null)
            {
                return HttpNotFound();
            }
            return View(inv);
        }

        [HttpPost, ActionName("Rescind"), Authorize(Roles = "Admin, Coordinator"),
            ValidateAntiForgeryToken]
        public ActionResult ConfirmRescind(string id)
        {
            RegisterInvite inv = db.Invites.Find(id);
            db.Invites.Remove(inv);
            db.SaveChanges();

            return RedirectToAction("InviteList");
        }


        public ActionResult UpdateStudent()
        {
            ViewBag.Majors = new SelectList(db.Majors.ToList(), "MajorID", "MajorName");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateStudent([Bind(Include = "UserId,GPA,MajorID,CurrentPassword,NewPassword,ConfirmNewPassword")] StudentUpdateModel studentUpdateModel)
        {
            var user = CurrentUser;

            var studInfo = db.Students.FirstOrDefault(si => si.User == user);

            var major = db.Majors.FirstOrDefault(m => m.MajorID == studentUpdateModel.MajorID);

            var passwordValidated = userManager.CheckPassword(user, studentUpdateModel.CurrentPassword);

            //var passVerification = userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, studentUpdateModel.Password);

            if (!ModelState.IsValid) return View();

            if (ModelState.IsValid)
            {
                studInfo.GPA = studentUpdateModel.GPA;

                if (studInfo.Major.MajorID != studentUpdateModel.MajorID)
                {
                    studInfo.Major = major;
                }

                if (passwordValidated && studentUpdateModel.NewPassword == studentUpdateModel.ConfirmNewPassword)
                {
                    userManager.ChangePassword(user.Id, studentUpdateModel.CurrentPassword, studentUpdateModel.NewPassword);
                }

                db.Entry(user).State = EntityState.Modified;
                db.Entry(studInfo).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult DisableStudents()
        {
            var students = GetEnabledStudents();
            ViewBag.Students = new MultiSelectList(students, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DisableStudents(string[] Students)
        {
            foreach (var id in Students)
            {
                var user = db.Users.Find(id);

                user.Enabled = false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.Message = "Student(s) Successfully Disabled";

            var students = GetEnabledStudents();
            ViewBag.Students = new MultiSelectList(students, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Coordinator")]
        public ActionResult EnableStudents()
        {
            var students = GetDisabledStudents();
            ViewBag.Students = new MultiSelectList(students, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EnableStudents(string[] Students)
        {
            foreach (var id in Students)
            {
                var user = db.Users.Find(id);

                user.Enabled = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.Message = "Student(s) Successfully Enabled";

            var students = GetDisabledStudents();
            ViewBag.Students = new MultiSelectList(students, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DisableCoordinators()
        {
            var coordinators = GetEnabledCoordinators();
            ViewBag.Coordinators = new MultiSelectList(coordinators, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DisableCoordinators(string[] Coordinators)
        {
            foreach (var id in Coordinators)
            {
                var user = db.Users.Find(id);

                user.Enabled = false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.Message = "Coordinator(s) Successfully Disabled";

            var coordinators = GetEnabledCoordinators();
            ViewBag.Coordinators = new MultiSelectList(coordinators, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EnableCoordinators()
        {
            var coordinators = GetDisabledCoordinators();
            ViewBag.Coordinators = new MultiSelectList(coordinators, "Key", "Value");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EnableCoordinators(string[] Coordinators)
        {
            foreach (var id in Coordinators)
            {
                var user = db.Users.Find(id);

                user.Enabled = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.Message = "Coordinator(s) Successfully Enabled";

            var coordinators = GetDisabledCoordinators();
            ViewBag.Coordinators = new MultiSelectList(coordinators, "Key", "Value");

            return View();
        }

        private Dictionary<string, string> GetEnabledStudents()
        {
            var coordInfo = db.Coordinators.Find(CurrentUser.Id);
            var students = new Dictionary<string, string>();

            foreach (var dept in coordInfo.Departments)
            {
                foreach (var student in db.Students.Where(s => s.User.Enabled))
                {
                    if (dept.Majors.Contains(student.Major))
                    {
                        students[student.User.Id] = string.Format("{0} - {1} {2}", student.LNumber, student.User.FirstName, student.User.LastName);
                    }
                }
            }

            return students;
        }

        private Dictionary<string, string> GetDisabledStudents()
        {
            var coordInfo = db.Coordinators.Find(CurrentUser.Id);
            var students = new Dictionary<string, string>();

            foreach (var dept in coordInfo.Departments)
            {
                foreach (var student in db.Students.Where(s => !s.User.Enabled))
                {
                    if (dept.Majors.Contains(student.Major))
                    {
                        students[student.User.Id] = string.Format("{0} - {1} {2}", student.LNumber, student.User.FirstName, student.User.LastName);
                    }
                }
            }

            return students;
        }

        public ActionResult DepartmentList()
        {
            return View(db.Departments.ToList());
        }

        private Dictionary<string, string> GetEnabledCoordinators()
        {
            var coordinators = new Dictionary<string, string>();

            foreach (var coord in db.Coordinators)
            {
                var user = db.Users.Find(coord.UserId);
                if (user.Enabled)
                {
                    coordinators[coord.UserId] = string.Format("{0} - {1} {2}", user.Email, user.FirstName, user.LastName);
                }
            }

            return coordinators;
        }

        private Dictionary<string, string> GetDisabledCoordinators()
        {
            var coordinators = new Dictionary<string, string>();

            foreach (var coord in db.Coordinators)
            {
                var user = db.Users.Find(coord.UserId);
                if (!user.Enabled)
                {
                    coordinators[coord.UserId] = string.Format("{0} - {1} {2}", user.Email, user.FirstName, user.LastName);
                }
            }

            return coordinators;
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
        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        public ActionResult EditDepartment([Bind(Include = "DepartmentID, DepartmentName, Majors")] Department dept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DepartmentList");
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();

                //if (userManager != null)
                //    userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
