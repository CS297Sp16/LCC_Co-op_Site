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
        private UserManager<User> userManager;

        public HomeController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            /*
                This page will act as a redirect.
                If the user is not logged in, take them to the Login page (this should happen for all pages, really).
                If the user is a student, take them to the Listing page.
                If the user is a Co-op Coordinator or Company, take them to their control panel.
            */
            return View();
            //return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent, "The site is still under construction");
        }

        public ActionResult Listings()
        {
            if (CurrentUser.GetType().Name == "Student")
            {
                Student s = (Student)CurrentUser;

                Major studentMajor = db.Majors.Single(m => m.MajorID == s.MajorID);

                var x = db.Opportunities.Where(
                    o => o.DepartmentID == db.Departments.Single(
                        d => d.Majors.Contains(studentMajor)
                        ).DepartmentID
                    );
                return View(x.ToList());
            }

            return View();
        }

        private User CurrentUser
        {
            get
            {
                return db.Users.Single(u => u.UserName == User.Identity.Name);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();

                if (userManager != null)
                    userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}