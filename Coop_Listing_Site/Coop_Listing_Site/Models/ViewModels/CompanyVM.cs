using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class CompanyVM
    {

        [Required, Display(Name = "Company Name")] 
        public string CompanyName { get; set; }
        [Required, Display(Name = "City")]          
        public string City { get; set; }
        [StringLength(2, ErrorMessage = "Enter the state abbreviation")]
        [Required, Display(Name = "State")]
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