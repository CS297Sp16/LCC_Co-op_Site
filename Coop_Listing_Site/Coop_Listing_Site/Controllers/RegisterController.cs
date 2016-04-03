using Coop_Listing_Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        // GET: Register/Student
        public ActionResult Student()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Student(StudentRegistrationModel student)
        {
            return View();
        }

        // GET: Register/Company
        public ActionResult Company()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Company(CompanyRegistrationModel company)
        {
            return View();
        }
    }
}