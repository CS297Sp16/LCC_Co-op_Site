﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class Student : User
    {
        [Required(ErrorMessage ="You must enter a valid L-Number")]
        [Display(Name ="L-Number")]
        [RegularExpression("^[Ll]\\d{8}", ErrorMessage ="Your L-Number must contain a capitol L followed by 8 digits")]
        public string LNumber { get; set; }

        public int MajorID { get; set; }
    }
}