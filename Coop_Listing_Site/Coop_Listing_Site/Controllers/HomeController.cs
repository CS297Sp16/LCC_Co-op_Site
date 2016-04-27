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
        public ActionResult Index()
        {
            if (User.IsInRole("Student"))
                return RedirectToAction("Index", "Coop");
            else if (User.IsInRole("Coordinator"))
                return RedirectToAction("Index", "ControlPanel");

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