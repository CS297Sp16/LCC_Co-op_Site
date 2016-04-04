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
            // Will be buttons letting the user decide which login page to go to
            return View();
        }

        // GET: Login/Student
        public ActionResult Student()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Student(StudentLoginModel student)
        {
            if (!ModelState.IsValid) return View();
            var user = userManager.Find(student.LNumber, student.Password);
            if (user != null)
            {
                var identity = userManager.CreateIdentity(
                    user, DefaultAuthenticationTypes.ApplicationCookie);

                GetAuthenticationManager().SignIn(identity);
                

                //TODO: Need to add a redirect after login 
            }
            

            // user authentication failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
            return View();
        }

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

        //Added Signin Method for the user
        private void SignIn(User user)
        {
            var identity = userManager.CreateIdentity(
                user, DefaultAuthenticationTypes.ApplicationCookie);

            GetAuthenticationManager().SignIn(identity);
        }

        //Added authenticationMangager
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
    }
}
 