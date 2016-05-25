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

            if (failures < emailArray.Length)
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

        [Authorize(Roles = "Admin, Coordinator")]
        public ActionResult InviteList()
        {
            var inviteList = new List<RegisterInvite>();
            if (User.IsInRole("Admin"))
            {
                inviteList = db.Invites.ToList();
            }
            else
            {
                inviteList = db.Invites.Where(i => i.UserType == RegisterInvite.AccountType.Student).ToList();
            }

            return View(inviteList);
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
            if (inv.UserType == RegisterInvite.AccountType.Coordinator && !User.IsInRole("Admin"))
            {
                ViewBag.Message = "You must be an administrator to rescind a coordinator's registration invite.";
                return View(inv);
            }
            else
            {
                db.Invites.Remove(inv);
                db.SaveChanges();
            }

            return RedirectToAction("InviteList");
        }


        public ActionResult UpdateStudent()
        {
            var studInfo = db.Students
                .Where(si => si.User.Id == CurrentUser.Id)
                .Include(si => si.User)
                .Include(si => si.Major)
                .FirstOrDefault();

            ViewBag.Majors = new SelectList(db.Majors.ToList(), "MajorID", "MajorName", studInfo.Major.MajorID);

            var gpaList = new Dictionary<double, string>();
            double gpaMin = 2.00d;
            double inc = 0.01d;
            double key;
            string value;
            double gpaSelectedValue;

            while (gpaMin <= 4.5)
            {
                value = gpaMin.ToString("N2");  //used to format the displayed value
                key = Convert.ToDouble(value);  //produces the values => key

                gpaList.Add(key, value);
                gpaMin += inc;
            }

            var studentVM = new StudentUpdateModel()
            {
                UserId = studInfo.User.Id,
                MajorID = studInfo.Major.MajorID,
                GPA = studInfo.GPA
            };

            if (studentVM.GPA > 2)
            {
                gpaSelectedValue = studentVM.GPA;
            }
            else
            {
                gpaSelectedValue = 2; //assumes all students must have at least a 2.0 gpa.  This is a minimum requirement at lane, I think??? -LONNIE
            }

            ViewBag.GPAs = new SelectList(gpaList, "key", "value", gpaSelectedValue);

            return View(studentVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateStudent([Bind(Include = "UserId,GPA,MajorID,CurrentPassword,NewPassword,ConfirmNewPassword")] StudentUpdateModel studentUpdateModel)
        {
            bool newPasswordMatches = false;
            bool passwordValidated = false;
            bool passwordChangeRequested = false;

            var gpaList = new Dictionary<double, string>();
            double gpaMin = 2.00d;
            double inc = 0.01d;
            double key;
            string value;
            double gpaSelectedValue;

            while (gpaMin <= 4.5)
            {
                value = gpaMin.ToString("N2");  //used to format the displayed value
                key = Convert.ToDouble(value);  //produces the values => key

                gpaList.Add(key, value);
                gpaMin += inc;
            }

            var studInfo = db.Students
                .Where(si => si.User.Id == studentUpdateModel.UserId)
                .Include(si => si.User)
                .Include(si => si.Major)
                .FirstOrDefault();

            var major = db.Majors.FirstOrDefault(m => m.MajorID == studentUpdateModel.MajorID);

            ViewBag.Majors = new SelectList(db.Majors.ToList(), "MajorID", "MajorName", studInfo.Major.MajorID);

            if (studentUpdateModel.GPA > 2)
            {
                gpaSelectedValue = studentUpdateModel.GPA;
            }
            else
            {
                gpaSelectedValue = 2; //assumes all students must have at least a 2.0 gpa.  This is a minimum requirement at lane, I think??? -LONNIE
            }

            ViewBag.GPAs = new SelectList(gpaList, "key", "value", gpaSelectedValue);

            if (studentUpdateModel.CurrentPassword != null)
            {
                var user = userManager.Find(studInfo.User.Email, studentUpdateModel.CurrentPassword);  //userManager.checkPassword not working
                if (user != null)
                {
                    passwordValidated = true;
                }
                else
                {
                    ViewBag.NoMatch = "The Current Password You Provided Was Incorrect. Please retry or contact your department's coordinator. ";
                    return View("UpdateStudent");
                }
            }

            if (studentUpdateModel.NewPassword == studentUpdateModel.ConfirmNewPassword)
            {
                newPasswordMatches = true;
            }

            if (studentUpdateModel.NewPassword != null && userManager.Find(studInfo.User.Email, studentUpdateModel.NewPassword) == null)
            {
                passwordChangeRequested = true;
            }

            if (!ModelState.IsValid) return View();

            if (ModelState.IsValid)
            {
                if (studInfo.GPA != studentUpdateModel.GPA)
                {
                    studInfo.GPA = studentUpdateModel.GPA;
                    db.Entry(studInfo).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if (studInfo.Major != major)
                {
                    studInfo.Major = major;
                    db.Entry(studInfo.Major).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if (passwordValidated && newPasswordMatches && passwordChangeRequested)
                {
                    userManager.ChangePassword(studInfo.User.Id, studentUpdateModel.CurrentPassword, studentUpdateModel.NewPassword);

                    //TODO: redirect back to index with message confirming
                    ViewBag.PassConfirm = "Your Password Has Successfully Been Updated";
                    return View("Index");
                }
            }
            return RedirectToAction("Index");
        }

        /* remove later
         * 
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
        }*/

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
            var coordInfo = db.Coordinators.Include(c => c.Majors).Include(c => c.User).FirstOrDefault(c => c.User.Id == CurrentUser.Id);
            var students = new Dictionary<string, string>();
            foreach (var student in db.Students.Include(s => s.Major).Include(s => s.User).Where(s => s.User.Enabled).ToList())
            {
                if (coordInfo.Majors.Contains(student.Major))
                {
                    students[student.User.Id] = string.Format("{0} - {1} {2}", student.LNumber, student.User.FirstName, student.User.LastName);
                }
            }

            return students;
        }

        private Dictionary<string, string> GetDisabledStudents()
        {
            var coordInfo = db.Coordinators.Include(c => c.Majors).Include(c => c.User).FirstOrDefault(c => c.User.Id == CurrentUser.Id);
            var students = new Dictionary<string, string>();
            foreach (var student in db.Students.Include(s => s.Major).Include(s => s.User).Where(s => !s.User.Enabled).ToList())
            {
                if (coordInfo.Majors.Contains(student.Major))
                {
                    students[student.User.Id] = string.Format("{0} - {1} {2}", student.LNumber, student.User.FirstName, student.User.LastName);
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

            foreach (var coord in db.Coordinators.Include(c => c.User))
            {
                if (coord.User.Enabled)
                {
                    coordinators[coord.User.Id] = string.Format("{0} - {1} {2}", coord.User.Email, coord.User.FirstName, coord.User.LastName);
                }
            }

            return coordinators;
        }

        private Dictionary<string, string> GetDisabledCoordinators()
        {
            var coordinators = new Dictionary<string, string>();

            foreach (var coord in db.Coordinators.Include(c => c.User))
            {
                if (!coord.User.Enabled)
                {
                    coordinators[coord.User.Id] = string.Format("{0} - {1} {2}", coord.User.Email, coord.User.FirstName, coord.User.LastName);
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

        //GET: ControlPanelController/EditDepartment
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

        //POST: ControlPanelController/EditDepartment
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

        //GET: ControlPanelController/DeleteDepartment
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

                if (userManager != null)
                    userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
