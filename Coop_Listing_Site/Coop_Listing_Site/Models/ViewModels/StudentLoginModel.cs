using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class StudentLoginModel
    {
        [Required(ErrorMessage = "You must enter a valid L-Number")]
        [Display(Name = "L-Number")]
        [RegularExpression("^[Ll]\\d{8}", ErrorMessage = "Your L-Number must contain the letter L followed by 8 digits")]
        public string LNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must be at least 8 characters and include at least one letter and one number")]
        public string Password { get; set; }
    }
}