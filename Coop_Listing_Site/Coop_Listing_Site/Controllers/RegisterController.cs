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
        // TODO: Proper code to let the users register; after View Models and a dummy database are created
        // TODO: Implement Registration via invite only
        // TODO: Implement mass invite (likely goes under Co-op advisor's control panel)

        private CoopContext db = new CoopContext();
        private UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new CoopContext()));

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        // GET: Register/Student/
        public ActionResult Student()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult Student([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] StudentRegistrationModel student)
        {
            if (!ModelState.IsValid) return View();


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
            if (!ModelState.IsValid) return View();

            User user = new User
            {
                FirstName = coordinator.FirstName,
                LastName = coordinator.LastName,
                Email = coordinator.Email
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