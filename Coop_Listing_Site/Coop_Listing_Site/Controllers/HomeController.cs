using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    public class HomeController : Controller
    {
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

            /*
            if (User.IsInRole("Student"))
                return RedirectToAction("Index", "Coop");
            else if (User.IsInRole("Coordinator"))
                return RedirectToAction("Index", "ControlPanel");
            */

            return View();
        }
    }
}