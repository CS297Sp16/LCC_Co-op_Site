using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            /*
                This page will act as a redirect.
                If the user is not logged in, take them to the Login page (this should happen for all pages, really).
                If the user is a student, take them to the Listing page.
                If the user is a Co-op Coordinator or Company, take them to their control panel.
            */
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent, "The site is still under construction");
        }
    }
}