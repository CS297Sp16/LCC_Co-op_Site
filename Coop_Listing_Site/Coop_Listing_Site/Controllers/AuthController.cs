using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using Coop_Listing_Site.Models.ViewModels;
using System.Collections.Generic;

namespace Coop_Listing_Site.Controllers
{
    public class AuthController : Controller
    {
        private CoopContext db;
        private UserManager<User> userManager;

        public AuthController()
        {
            db = new CoopContext();
            userManager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var emailInfo = db.Emails.FirstOrDefault();
            ViewBag.Error = (emailInfo == null || !emailInfo.ProperlySet);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult ForgotPassword(string email)
        {
            var user = userManager.FindByEmail(email);

            if (user == null)
            {
                ViewBag.Message = "User with specified email not found.";
            }
            else
            {
                Dictionary<bool, string> response;
                bool success;
                var emailInfo = db.Emails.First();
                var passReset = db.ResetTokens.FirstOrDefault(r => r.Email.ToLower() == email.ToLower());

                if (passReset != null)
                {
                    response = emailInfo.SendResetEmail(passReset);
                    success = response.Keys.First();
                }
                else
                {
                    passReset = new PasswordReset { Email = email, Id = Guid.NewGuid().ToString("N") };
                    db.ResetTokens.Add(passReset);
                    db.SaveChanges();

                    response = emailInfo.SendResetEmail(passReset);
                    success = response.Keys.First();
                }

                if (!success)
                {
                    db.ResetTokens.Remove(passReset);
                    db.SaveChanges();
                }

                ViewBag.Message = response[success];
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            var reset = db.ResetTokens.Find(id);
            if (reset == null)
            {
                return View("Invalid");
            }

            var resetModel = new PassResetModel { Email = reset.Email };

            return View(resetModel);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult ResetPassword([Bind(Include = "Email,Password,ConfirmPassword")] PassResetModel model)
        {
            if (!ModelState.IsValid) return View();

            var user = userManager.FindByEmail(model.Email);
            //var token = userManager.GeneratePasswordResetToken(user.Id);
            var newPass = userManager.PasswordHasher.HashPassword(model.Password);

            user.PasswordHash = newPass;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var reset = db.ResetTokens.FirstOrDefault(r => r.Email.ToLower() == model.Email.ToLower());
            db.ResetTokens.Remove(reset);
            db.SaveChanges();

            return View("ResetSuccess");
        }

        // Used to let the user log out
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        [HttpGet, Authorize(Roles = "Admin, Coordinator, Student")]
        public ActionResult ChangePassword()
        {

            var vm = new ChangePasswordViewModel()
            {
                Email = CurrentUser.Email
            };

            return View(vm);
        }

        [HttpPost, Authorize(Roles = "Admin, Coordinator, Student")]
        public ActionResult ChangePassword([Bind(Include = "CurrentPassword,ConfirmNewPassword,NewPassword")] ChangePasswordViewModel changePassVM)
        {
            bool newPasswordMatches = false;
            bool passwordValidated = false;
            bool passwordChangeRequested = false;

            if (changePassVM.CurrentPassword != null)
            {
                var user = userManager.Find(CurrentUser.Email, changePassVM.CurrentPassword);
                if (user != null)
                {
                    passwordValidated = true;
                }
                else
                {
                    if(User.IsInRole("Student"))
                    {
                        ViewBag.NoMatch = "The Current Password You Provided Was Incorrect. Please retry or contact your department's coordinator. ";
                        return RedirectToAction("ChangePassword","Auth");
                    }
                    else if(User.IsInRole("Coordinator"))
                    {
                        ViewBag.NoMatch = "The Current Password You Provided Was Incorrect. Please retry or contact your department's admin. ";
                        return RedirectToAction("ChangePassword", "Auth");
                    }
                    else
                    {
                        ViewBag.NoMatch = "Your an Admin";
                    }

                }
            }

            if (changePassVM.NewPassword == changePassVM.ConfirmNewPassword)
            {
                newPasswordMatches = true;
            }

            if (changePassVM.NewPassword != null && userManager.Find(CurrentUser.Email, changePassVM.NewPassword) == null)
            {
                passwordChangeRequested = true;
            }

            if (passwordValidated && newPasswordMatches && passwordChangeRequested)
            {
                ViewBag.NoMatch = "";
                userManager.ChangePassword(CurrentUser.Id, changePassVM.CurrentPassword, changePassVM.NewPassword);
                ViewBag.PassConfirm = "Your Password Has Successfully Been Updated";
                
                return RedirectToAction("Index","ControlPanel");
            }

            return RedirectToAction("Index","ControlPanel");
        }
        private User CurrentUser
        {
            get
            {
                return db.Users.Find(User.Identity.GetUserId());
            }
        }

    }
}