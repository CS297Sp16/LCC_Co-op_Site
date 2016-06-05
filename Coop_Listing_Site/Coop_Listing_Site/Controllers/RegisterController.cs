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
        private UserManager<User> userManager;
        private IRepository repo;

        public RegisterController()
        {
            var db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
            repo = new Repository(db);
        }

        // GET: Register
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home"); // No one should be accessing this page directly; Send them away
        }

        // GET: Register/Student/
        public ActionResult Student(string id)
        {
            var invite = repo.GetByID<RegisterInvite>(id);
            if (invite == null || invite.UserType != RegisterInvite.AccountType.Student)
            {
                return View("Invalid");
            }

            StudentRegistrationModel model = new StudentRegistrationModel { Email = invite.Email };

            ViewBag.Majors = new SelectList(repo.GetAll<Major>().ToList(), "MajorID", "MajorName");

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Student([Bind(Include = "FirstName,LastName,Email,LNumber,GPA,Password,ConfirmPassword")] StudentRegistrationModel student, int Majors)
        {
            ViewBag.Majors = new SelectList(repo.GetAll<Major>().ToList(), "MajorID", "MajorName");
            if (!ModelState.IsValid) return View(student);

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
                var major = repo.GetByID<Major>(Majors);

                var studentInfo = new StudentInfo
                {
                    User = user,
                    LNumber = student.LNumber,
                    GPA = student.GPA
                };

                if (major != null)
                    studentInfo.Major = major;

                repo.Add(studentInfo);

                userManager.AddToRole(user.Id, "Student");
                SignIn(user);

                var invite = repo.GetOne<RegisterInvite>(i => i.Email.ToLower() == student.Email.ToLower());
                if (invite != null) // Should never be null, but check anyway
                {
                    repo.Delete(invite);
                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(student);
        }

        // GET: Register/Coordinator/
        public ActionResult Coordinator(string id)
        {
            var invite = repo.GetByID<RegisterInvite>(id);
            if (invite == null || invite.UserType != RegisterInvite.AccountType.Coordinator)
            {
                return View("Invalid");
            }

            CoordRegModel model = new CoordRegModel { Email = invite.Email };
            ViewBag.Majors = new SelectList(repo.GetWhere<Major>(m => m.Coordinator == null).ToList(), "MajorID", "MajorName");

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Coordinator([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] CoordRegModel coordinator, int[] MajorIDs, int? Majors)
        {
            var nocoordMajors = repo.GetWhere<Major>(m => m.Coordinator == null);

            if(MajorIDs != null && MajorIDs.Length > 0)
            {
                nocoordMajors = nocoordMajors.Where(m => !MajorIDs.Contains(m.MajorID));

                foreach(var id in MajorIDs)
                {
                    var major = repo.GetByID<Major>(id);

                    if (major != null)
                        coordinator.Majors.Add(major);
                }
            }
            else if(Majors != null)
            {
                nocoordMajors = nocoordMajors.Where(m => m.MajorID != Majors);

                var major = repo.GetByID<Major>(Majors);
                coordinator.Majors.Add(major);
            }

            ViewBag.Majors = new SelectList(nocoordMajors, "MajorID", "MajorName");

            if (!ModelState.IsValid) return View(coordinator);

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
                var coordinfo = new CoordinatorInfo
                {
                    User = user,
                    Majors = coordinator.Majors
                };

                repo.Add(coordinfo);

                userManager.AddToRole(user.Id, "Coordinator");

                SignIn(user);

                var invite = repo.GetOne<RegisterInvite>(i => i.Email.ToLower() == coordinator.Email.ToLower());
                if (invite != null) // Should never be null, but check anyway
                {
                    repo.Delete(invite);
                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(coordinator);
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
    }
}