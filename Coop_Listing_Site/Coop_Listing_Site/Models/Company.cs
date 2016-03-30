using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;

namespace Coop_Listing_Site.Models
{
    public class Company
    {
        public int CompanyID { get; set; }

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