using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Company : User
    {
        // Inherits from User, doesn't need its own ID
        // public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Company Name")]  //This stuff is specific the User Interface and should be placed on the CompanyViewModel
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "City")]          //UI Specific
        public string City { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "Enter the state abbreviation")]
        [Display(Name = "State")]          //UI Specific 
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