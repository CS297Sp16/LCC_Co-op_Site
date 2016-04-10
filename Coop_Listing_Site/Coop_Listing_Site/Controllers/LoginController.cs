using System.Web;
using Coop_Listing_Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Coop_Listing_Site.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // TODO: Proper code to let the user log in; after View Models and a dummy database are created
        UserManager<User>userManager = new UserManager<User>(
            new UserStore<User>(new CoopContext()));
        // GET: Login
        public ActionResult Index()
        {

            return View();
        }

        // GET: Login/Student
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel userModel)
        {
            if (!ModelState.IsValid) return View();

            var user = userManager.FindByEmail(userModel.Email);
            if (user != null)
            {
                var passVerification = userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, userModel.Password);

                if (passVerification == PasswordVerificationResult.Failed)
                {
                    // user authentication failed
                    ModelState.AddModelError("", "Incorrect password");
                    return View();
                }

                SignIn(user);

                // Temporary redirect to the home page
                return RedirectToAction("Index", "Home");
            }

            // user authentication failed
            ModelState.AddModelError("", "User with provided email not found");
            return View();
        }

        /*
         * Company's can wait, as they are not essential to getting the site running.
         *
        // GET: Login/Company
        public ActionResult Company()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Company(CompanyLoginModel company)
        {
            return View();
        }
        */

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
