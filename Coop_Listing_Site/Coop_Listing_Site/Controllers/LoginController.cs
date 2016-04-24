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
        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new CoopContext()));

        // GET: /Login/
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) // User already logged in. They don't need to be here
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Email,Password")] LoginModel userModel)
        {
            if (!ModelState.IsValid) return View();

            var user = userManager.FindByEmail(userModel.Email); // Can probably be changed back to userManager.Find(username, password) at some point, since usernames are set to emails now
            if (user != null)
            {
                if (user.Enabled)
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
                else
                {
                    // This user's account is not enabled
                    ModelState.AddModelError("", "This account has been disabled");
                }
            }
            else
            {
                // user authentication failed
                ModelState.AddModelError("", "User with provided email not found");
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
    }
}
