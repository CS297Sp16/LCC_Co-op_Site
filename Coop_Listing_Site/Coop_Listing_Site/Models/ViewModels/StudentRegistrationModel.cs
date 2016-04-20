using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class StudentRegistrationModel
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter a valid L-Number"), Display(Name = "L-Number")]
        [RegularExpression("^[Ll]\\d{8}", ErrorMessage = "Your L-Number must contain an L followed by 8 digits")]
        public string LNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public double GPA { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must be at least 8 characters and include at least one letter and one number")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Not required for now, until a dummy DB is set up
        public int MajorID { get; set; }
    }
}
