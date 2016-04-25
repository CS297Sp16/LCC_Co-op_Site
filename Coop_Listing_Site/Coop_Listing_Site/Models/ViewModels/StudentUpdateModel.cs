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
        public int MajorID { get; set; }
        public double GPA { get; set; }
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must be at least 8 characters and include at least one letter and one number")]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}