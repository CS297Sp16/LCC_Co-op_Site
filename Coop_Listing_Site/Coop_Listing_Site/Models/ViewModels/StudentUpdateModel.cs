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
        public string UserId { get; set; }
        [Display(Name = "Major")]
        public int MajorID { get; set; }
        public double GPA { get; set; }
        [Display(Name = "Do you want to change your password?")]
        public bool UpdatePass { get; set; }

        [DataType(DataType.Password), Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password), Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must be at least 8 characters and include at least one letter and one number")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}