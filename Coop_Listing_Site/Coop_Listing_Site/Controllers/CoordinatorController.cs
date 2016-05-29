using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoordinatorController : Controller
    {
        // GET: Coordinator
        private IRepository repo;

        public CoordinatorController()
        {
            var db = new CoopContext();
            repo = new Repository(db);
        }

        public CoordinatorController(IRepository r)
        {
            repo = r;
        }

        public ActionResult Index()
        {
            var coordinatorVMs = repo.GetAll<CoordinatorInfo>().Select(s => new CoordinatorViewModel(s));

            return View(coordinatorVMs);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var coordinator = repo.GetByID<CoordinatorInfo>(id);
            
            if (coordinator == null)
                return HttpNotFound();

            return View(new CoordinatorViewModel(coordinator));
        }


        public ActionResult Enable(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var coordinator = repo.GetByID<CoordinatorInfo>(id);

            if (coordinator == null)
                return HttpNotFound();

            return View(new CoordinatorViewModel(coordinator));
        }

        [HttpPost, ActionName("Enable"), ValidateAntiForgeryToken]
        public ActionResult ConfirmEnable(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var coordinator = repo.GetByID<CoordinatorInfo>(id);

            if (coordinator == null)
                return HttpNotFound();

            coordinator.User.Enabled = true;
            repo.Update(coordinator);

            ViewBag.Updated = true;

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Disable(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var coordinator = repo.GetByID<CoordinatorInfo>(id);

            if (coordinator == null)
                return HttpNotFound();

            return View(new CoordinatorViewModel(coordinator));
        }

        [HttpPost, ActionName("Disable"), ValidateAntiForgeryToken]
        public ActionResult ConfirmDisable(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var coordinator = repo.GetByID<CoordinatorInfo>(id);

            if (coordinator == null)
                return HttpNotFound();

            coordinator.User.Enabled = false;
            repo.Update(coordinator);

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Invitations()
        {
            return View(repo.GetWhere<RegisterInvite>(i => i.UserType == RegisterInvite.AccountType.Coordinator));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}