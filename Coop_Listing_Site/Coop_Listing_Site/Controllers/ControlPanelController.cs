using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;


namespace Coop_Listing_Site.Controllers
{
    public class ControlPanelController : Controller
    {
        // Might need a rename. This will have a Student and Advisor action result to start, which will point to their views.
        // GET: ControlPanel
        private CoopContext ctx;

        ControlPanelController()
        {
            ctx = new CoopContext();
        }

        //[Authorize]
        public ActionResult Index()
        {
            var users = ctx.Users;
            return View();
        }
    }
}