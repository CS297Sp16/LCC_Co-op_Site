using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    public class CoopController : Controller
    {
        // This controller will have List, Details, and possibly Create, Delete, and Edit for all co-op opportunities

        // GET: Coop
        public ActionResult Index()
        {
            return View();
        }
    }
}