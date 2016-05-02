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
        public ActionResult Index(string returnURL)
        {
            if (User.Identity.IsAuthenticated) // User already logged in. They don't need to be here
                return RedirectToAction("Index", "Home");
            
            if (string.IsNullOrWhiteSpace(returnURL))
                returnURL = Request.UrlReferrer.LocalPath;

            var model = new LoginModel() { ReturnURL = returnURL };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Email,Password,ReturnURL")] LoginModel userModel)
        {
            if (!ModelState.IsValid) return View(userModel);

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
                        return View(userModel);
                    }

                    SignIn(user);
                    
                    return Redirect(GetRedirectUrl(userModel.ReturnURL));
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

            return View(userModel);
        }

        private void SignIn(User user)
        {
            var identity = userManager.CreateIdentity(
                user, DefaultAuthenticationTypes.ApplicationCookie);
            var authProps = new AuthenticationProperties { IsPersistent = false };

            GetAuthenticationManager().SignIn(authProps, identity);
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }
    }
}
