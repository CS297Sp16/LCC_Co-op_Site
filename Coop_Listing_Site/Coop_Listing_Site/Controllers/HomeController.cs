using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;

namespace Coop_Listing_Site.Controllers
{
    public class HomeController : Controller
    {
        private CoopContext db;
        //private UserManager<User> userManager;

        public HomeController()
        {
            db = new CoopContext();
            //userManager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: Home
        [AllowAnonymous] // Get rid of this to redirect to Login if the user is not authenticated
        public ActionResult Index()
        {
            /*
                This page will act as a redirect.
                -- If the user is not logged in, take them to the Login page (this should happen for all pages, really).
                -- If the user is a student, take them to the Listing page.
                -- If the user is a Co-op Coordinator or Company, take them to their control panel.
            */

            
            if (User.IsInRole("Student"))
                return RedirectToAction("Index", "Coop");
            /*else if (User.IsInRole("Coordinator"))
                return RedirectToAction("Index", "ControlPanel");
            */

            return View();
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