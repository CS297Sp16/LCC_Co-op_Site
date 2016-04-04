using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class CompanyRegistrationModel
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must be at least 8 characters and include at least one letter and one number")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]        
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "Enter the state abbreviation")]
        [Display(Name = "State")]
        public string State { get; set; }

        [StringLength(5, ErrorMessage = "The Zip Code should be 5 digits long.")]
        [DataType(DataType.PostalCode, ErrorMessage = "Please Enter a valid ZipCode")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [DataType(DataType.Url, ErrorMessage = "Please Enter a valid URL")]
        [Display(Name = "Company Website")]
        public string Website { get; set; }
    }
}