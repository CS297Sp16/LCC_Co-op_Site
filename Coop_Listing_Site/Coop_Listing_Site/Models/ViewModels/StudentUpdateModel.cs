using Coop_Listing_Site.DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Coop_Listing_Site.Models.ViewModels
{
    public class StudentUpdateModel
    {
        //new UserManager<User>(new UserStore<User>(new CoopContext()));

        public string UserId { get; set; }
        public int MajorID { get; set; }
        public double GPA { get; set; }
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must be at least 8 characters and include at least one letter and one number")]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //This is not concrete just a place holder for now -LONNIE
        //public int FileID { get; set; }

        public void UpdateCurrentUser(CoopContext db, User currentUser)//userManager)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == currentUser.Id);

            var studInfo = db.Students.FirstOrDefault(si => si.UserId == user.Id);

            var major = db.Majors.FirstOrDefault(mj => mj.MajorID == studInfo.MajorID);

            //var passVerification = userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, userModel.Password);

            studInfo.GPA = this.GPA;
            studInfo.MajorID = this.MajorID;

            if(this.Password != this.CurrentPassword && this.Password == this.ConfirmPassword)
            {

            }


        }



    }
}