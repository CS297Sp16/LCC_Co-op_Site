using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        // TODO: Implement Registration via invite only
        // TODO: Implement mass invite (likely goes under Co-op advisor's control panel)

        private CoopContext db = new CoopContext();
        private UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new CoopContext()));

        // GET: Register
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home"); // No one should be accessing this page directly; Send them away
        }

        // GET: Register/Student/
        public ActionResult Student(/*string registrationToken*/)
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Student([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] StudentRegistrationModel student)
        {
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
                var studentInfo = new StudentInfo
                {
                    UserId = user.Id,
                    LNumber = student.LNumber,
                    GPA = student.GPA
                    // Add Major when we have some created or get a dummy DB up
                };

                db.Students.Add(studentInfo);
                db.SaveChanges();

                //userManager.AddToRole(user.Id, "Student");

                SignIn(user);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        // GET: Register/Coordinator/
        public ActionResult Coordinator()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Coordinator([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] CoordRegModel coordinator)
        {
            if (!ModelState.IsValid) return View(/*string registrationToken*/);

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
                    UserId = user.Id
                    // Add department when we have some
                };

                db.Coordinators.Add(coordinfo);
                db.SaveChanges();

                //userManager.AddToRole(user.Id, "Coordinator");

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