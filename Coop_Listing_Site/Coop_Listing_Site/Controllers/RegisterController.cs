using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Coop_Listing_Site.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        // TODO: Implement mass invite (likely goes under Co-op advisor's control panel)

        private CoopContext db;
        private UserManager<User> userManager;

        public RegisterController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: Register
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home"); // No one should be accessing this page directly; Send them away
        }

        // GET: Register/Student/
        public ActionResult Student(string id)
        {
            var invite = db.Invites.Find(id);
            if (invite == null || invite.UserType != RegisterInvite.AccountType.Student)
            {
                return View("Invalid");
            }

            StudentRegistrationModel model = new StudentRegistrationModel { Email = invite.Email };

            ViewBag.Majors = new SelectList(db.Majors.ToList(), "MajorID", "MajorName");

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Student([Bind(Include = "FirstName,LastName,Email,LNumber,GPA,Password,ConfirmPassword")] StudentRegistrationModel student, int Majors)
        {
            ViewBag.Majors = new SelectList(db.Majors.ToList(), "MajorID", "MajorName");
            if (!ModelState.IsValid) return View();

            User user = new User
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Enabled = true,
                UserName = student.Email // Cannot create user without a user name. We don't actually use user names, so just set it to the Email field.
            };

            var result = userManager.Create(user, student.Password);

            if (result.Succeeded)
            {
                var major = db.Majors.Find(Majors);

                var studentInfo = new StudentInfo
                {
                    User = user,
                    LNumber = student.LNumber,
                    GPA = student.GPA
                };

                if (major != null)
                    studentInfo.Major = major;

                db.Students.Add(studentInfo);
                db.SaveChanges();

                userManager.AddToRole(user.Id, "Student");
                SignIn(user);

                var invite = db.Invites.FirstOrDefault(i => i.Email.ToLower() == student.Email.ToLower());
                if (invite != null) // Should never be null, but check anyway
                {
                    db.Invites.Remove(invite);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        // GET: Register/Coordinator/
        public ActionResult Coordinator(string id)
        {
            var invite = db.Invites.Find(id);
            if (invite == null || invite.UserType != RegisterInvite.AccountType.Coordinator)
            {
                return View("Invalid");
            }

            CoordRegModel model = new CoordRegModel { Email = invite.Email };
            ViewBag.Department = new SelectList(db.Departments.ToList(), "DepartmentID", "DepartmentName");

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Coordinator([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] CoordRegModel coordinator, int Department)
        {
            ViewBag.Department = new SelectList(db.Departments.ToList(), "DepartmentID", "DepartmentName");
            if (!ModelState.IsValid) return View();

            User user = new User
            {
                FirstName = coordinator.FirstName,
                LastName = coordinator.LastName,
                Email = coordinator.Email,
                Enabled = true,
                UserName = coordinator.Email // Cannot create user without a user name. We don't actually use user names, so just set it to the Email field.
            };

            var result = userManager.Create(user, coordinator.Password);

            if (result.Succeeded)
            {
                var dept = db.Departments.Find(Department);

                var coordinfo = new CoordinatorInfo
                {
                    UserId = user.Id
                };

                if(dept != null)
                    coordinfo.Departments.Add(dept);

                db.Coordinators.Add(coordinfo);
                db.SaveChanges();

                userManager.AddToRole(user.Id, "Coordinator");

                SignIn(user);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }


        private void SignIn(User user)
        {
            var identity = userManager.CreateIdentity(
                user, DefaultAuthenticationTypes.ApplicationCookie);

            GetAuthenticationManager().SignIn(identity);
        }


        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        /*
         * Company's will wait, as they are not essential to getting the site running.
         * // GET: Register/Company
        public ActionResult Company()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Company(CompanyRegistrationModel company)
        {
            return View();
        }*/
    }
}