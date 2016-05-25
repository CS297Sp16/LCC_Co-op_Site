using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class StudentInfo
    {
        [Key]
        public string LNumber { get; set; }
        public User User { get; set; }
        public Major Major { get; set; }
        public double GPA { get; set; }
    }
}