using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // TODO: Proper code to let the user log in; after View Models and a dummy database are created

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
        public ActionResult Student(Student student) // Might change to a View Model
        {
            return View();
        }

        // GET: Login/Company
        public ActionResult Company()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Company(Company company)
        {
            return View();
        }
    }
}