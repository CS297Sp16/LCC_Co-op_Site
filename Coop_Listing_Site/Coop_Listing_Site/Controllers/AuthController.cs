using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;

namespace Coop_Listing_Site.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        // Used to let the user log out
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        //GET: for uploading files
        public ActionResult UpLoad()
        {
            var model = new UpLoadModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpLoad(UpLoadModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            // now you could pass the byte array to your model and store wherever 
            // you intended to store it

            return Content("Thanks for uploading the file");
        }
    }
}